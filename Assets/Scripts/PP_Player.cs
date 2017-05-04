﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Player : MonoBehaviour {

	[SerializeField] bool isActive = true;

	private int myTeamNumber = 1;
	private GameObject myButt;
	private string myControl = "1";
	[SerializeField] Rigidbody2D myRigidbody2D;
	[SerializeField] SpriteRenderer mySpriteRenderer;
	[SerializeField] SpriteRenderer mySpriteRendererPattern;
	[SerializeField] Animator myAnimator;
	private Color myColor;
	private Color myColorDetail;
	[SerializeField] SpriteRenderer mySpriteRendererBorder;

	private Vector2 myDirection;
	private Vector2 myRotation = Vector2.up;
	[SerializeField] float myRotationSpeed = 2;
	private Vector2 myMoveAxis;
	[SerializeField] float mySpeed = 1;
	[SerializeField] float moveGravity;
	[SerializeField] float moveSensitivity;

	[Header("Status")]
	[SerializeField] Color myStunMultiplier = Color.gray;
	private Color myStunColor;
	private bool myStatus_IsUsingAbility = false;
	private float myStatus_StunTimer;
	private bool myStatus_IsFrozen = false;
//	private Vector2 myStatus_FrozenPosition;
	private float myStatus_SpeedRatio = 1;
	private float myStatus_DashTimer;
	private float myCDTimer;
	private float myChargeTimer;

	[Header("Ability")]
	private PP_Global.Abilities myAbility = PP_Global.Abilities.Burp;
	[Header(" - Burp")]
	[SerializeField] Transform myAbility_Burp_Position;
	[SerializeField] GameObject myAbility_Burp_Prefab;
	[SerializeField] Sprite myAbility_Burp_Sprite;
	[SerializeField] float myAbility_Burp_CD = 1;
	[SerializeField] float myAbility_Burp_MaxChargeTime = 5;
	[SerializeField] Vector2 myAbility_Burp_Size = new Vector2 (1, 3);
	[SerializeField] float myAbility_Burp_StunTime = 2;
	[Header(" - Dash")]
	[SerializeField] float myAbility_Dash_CD = 1;
	[SerializeField] float myAbility_Dash_MaxChargeTime = 3;
	[SerializeField] Vector2 myAbility_Dash_SpeedRatio = new Vector2 (3, 10);
	[SerializeField] float myAbility_Dash_Time = 0.5f;
	[Header(" - Freeze")]
	[SerializeField] float myAbility_Freeze_CD = 1;
	[SerializeField] Sprite myAbility_Freeze_Sprite;
	[SerializeField] float myAbility_Freeze_MaxEnergy;
	private float myAbility_Freeze_Energy = 0;

	[Header("SFX")]
	[SerializeField] AudioClip mySFX_Burp;
	[SerializeField] AudioClip mySFX_Dash;
	[SerializeField] AudioClip mySFX_Freeze;

	[Header("Select Ready")]
	[SerializeField] bool selectReady = false;

	// Use this for initialization
	void Start () {
		SetMyAbility (PP_MessageBox.Instance.GetPlayerAbility (myControl));

		myAbility_Burp_Prefab = Instantiate (myAbility_Burp_Prefab, myAbility_Burp_Position.position, Quaternion.identity) as GameObject;
		myAbility_Burp_Prefab.GetComponent<PP_Skill_Burp> ().Init (this.gameObject, myAbility_Burp_Position);
		myAbility_Burp_Prefab.SetActive (false);
	}

	public void Init (int g_teamNumber, GameObject g_butt, PP_ColorSetPlayer g_colorSet, Color g_colorBorder, string g_myControl) {
		myColor = g_colorSet.myColors [0];
		myColorDetail = g_colorSet.myColors [1];
		myTeamNumber = g_teamNumber;
		myButt = g_butt;
		SetMyControl (g_myControl);
		mySpriteRenderer.color = g_colorSet.myColors [0];
		mySpriteRendererPattern.color = g_colorSet.myColors [1];
		mySpriteRendererBorder.color = g_colorBorder;

		myStunColor = myColor * myStunMultiplier.r; //assuming all rgb of stun color are the same cus it's gray
		myStunColor.a = 1f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (PP_MessageBox.Instance.GetIsPaused ())
			return;
		
		if (myStatus_StunTimer <= 0 && !myStatus_IsFrozen) {
			if (isActive)
				UpdateMove ();
			UpdateRotation ();
		}
	}

	void Update () {
		if (PP_MessageBox.Instance.GetIsPaused ())
			return;

		UpdateStatus ();
		UpdateAbility ();
	}

	private void UpdateStatus () {
		if (myStatus_StunTimer > 0) {
			myStatus_StunTimer -= Time.deltaTime;
			if (myStatus_StunTimer <= 0) {
				myStatus_StunTimer = 0;
				mySpriteRenderer.color = myColor;
				myAnimator.SetBool ("isStunned", false);
			}
		}
		if (myStatus_DashTimer > 0) {
			myStatus_DashTimer -= Time.deltaTime;
			if (myStatus_DashTimer <= 0) {
				myStatus_DashTimer = 0;
				myStatus_SpeedRatio = 1f;
			}
		}


//		if (myStatus_IsFrozen) {
//			this.transform.position = myStatus_FrozenPosition;
//		}
	}

	private void UpdateAbility () {
		if (myCDTimer > 0) {
			myCDTimer -= Time.deltaTime;
			if (myCDTimer <= 0) {
				myCDTimer = 0;
			}
		}

		if (myStatus_StunTimer > 0) {
			return;
		}

		if (myAbility == PP_Global.Abilities.Freeze && 
			myStatus_IsFrozen == false && 
			myAbility_Freeze_Energy < myAbility_Freeze_MaxEnergy) {
			Debug.Log (myAbility_Freeze_Energy);
			myAbility_Freeze_Energy += Time.deltaTime;
			if (myAbility_Freeze_Energy >= myAbility_Freeze_MaxEnergy) {
				myAbility_Freeze_Energy = myAbility_Freeze_MaxEnergy;
			}
		}

		if (myAbility == PP_Global.Abilities.Burp && myCDTimer <= 0) {
			UpdateAbility_Burp ();
		} else if (myAbility == PP_Global.Abilities.Freeze && myCDTimer <= 0) {
			UpdateAbility_Freeze ();
		} else if (myAbility == PP_Global.Abilities.Dash && myCDTimer <= 0) {
			UpdateAbility_Dash ();
		}

//		Debug.Log (myChargeTimer);
	}

	private void UpdateAbility_Burp () {
		if (Input.GetButton ("Skill" + myControl)) {
			if (!myStatus_IsUsingAbility) {
				myStatus_IsUsingAbility = true;
				myAnimator.SetBool ("isPressed", true);
			} else {
				myChargeTimer += Time.deltaTime;
				if (myChargeTimer > myAbility_Burp_MaxChargeTime) {
					myChargeTimer = myAbility_Burp_MaxChargeTime;
				}
			}
		}

		if (myStatus_IsUsingAbility && Input.GetButtonUp ("Skill" + myControl)) {
			myStatus_IsUsingAbility = false;

			CS_AudioManager.Instance.PlaySFX (mySFX_Burp);
			myAbility_Burp_Prefab.transform.localScale = 
				Vector3.one * 
				(
					(myChargeTimer / myAbility_Burp_MaxChargeTime * (myAbility_Burp_Size.y - myAbility_Burp_Size.x)) 
					+ myAbility_Burp_Size.x
				);
			myChargeTimer = 0;
			myAbility_Burp_Prefab.SetActive (true);
			myAbility_Burp_Prefab.GetComponent<PP_Skill_Burp> ().UpdateTransform ();
			myAnimator.SetBool ("isPressed", false);

			myCDTimer = myAbility_Burp_CD;
		}
	}

	private void UpdateAbility_Freeze () {
		if (Input.GetButton ("Skill" + myControl) && myAbility_Freeze_Energy > 0) {
			if (!myStatus_IsUsingAbility) {
				myStatus_IsUsingAbility = true;

				CS_AudioManager.Instance.PlaySFX (mySFX_Freeze);

				myStatus_IsFrozen = true;
				myRigidbody2D.isKinematic = true;
				myRigidbody2D.velocity = Vector3.zero;
				myAnimator.SetBool ("isPressed", true);
			} else {
				myAbility_Freeze_Energy -= Time.deltaTime;
				if (myAbility_Freeze_Energy <= 0) {
					UpdateAbility_Freeze_End ();
				}
			}
		} else if (myStatus_IsUsingAbility && Input.GetButtonUp ("Skill" + myControl)) {
			UpdateAbility_Freeze_End ();
		}
	}

	private void UpdateAbility_Freeze_End () {
		myStatus_IsUsingAbility = false;
		myStatus_IsFrozen = false;
		myRigidbody2D.isKinematic = false;
		myAnimator.SetBool ("isPressed", false);
		myCDTimer = myAbility_Freeze_CD;
	}

	private void UpdateAbility_Dash () {
		if (Input.GetButton ("Skill" + myControl)) {
			if (!myStatus_IsUsingAbility) {
				myStatus_IsUsingAbility = true;
				myAnimator.SetBool ("isPressed", true);
			} else {
				myChargeTimer += Time.deltaTime;
				if (myChargeTimer > myAbility_Dash_MaxChargeTime) {
					myChargeTimer = myAbility_Dash_MaxChargeTime;
				}
			}
		}

		if (myStatus_IsUsingAbility && Input.GetButtonUp ("Skill" + myControl)) {
			myStatus_IsUsingAbility = false;
			CS_AudioManager.Instance.PlaySFX (mySFX_Dash);
			myStatus_SpeedRatio = 
				(myChargeTimer / myAbility_Burp_MaxChargeTime * (myAbility_Dash_SpeedRatio.y - myAbility_Dash_SpeedRatio.x)) +
				myAbility_Dash_SpeedRatio.x;
			myStatus_DashTimer = myAbility_Dash_Time;
			myChargeTimer = 0;
			myAnimator.SetBool ("isPressed", false);

			myCDTimer = myAbility_Dash_CD;
		}
	}

	private void UpdateMove () {
		float t_inputHorizontal = Input.GetAxis ("Horizontal" + myControl);
		float t_inputVertical = Input.GetAxis ("Vertical" + myControl);

		if (myStatus_DashTimer > 0) {
			myDirection = myRotation.normalized;
		} else {
			myDirection = (Vector3.up * t_inputVertical + Vector3.right * t_inputHorizontal).normalized;
		}

		myMoveAxis += myDirection * moveSensitivity;
		if (myMoveAxis.magnitude > 1)
			myMoveAxis.Normalize ();


		//set the speed of the player

//		myRigidbody2D.AddForce (myMoveAxis * mySpeed);
		myRigidbody2D.velocity = myMoveAxis * mySpeed * myStatus_SpeedRatio;
	
		float t_moveAxisReduce = Time.fixedDeltaTime * moveGravity;
		if (myMoveAxis.magnitude < t_moveAxisReduce)
			myMoveAxis = Vector2.zero;
		else
			myMoveAxis *= (myMoveAxis.magnitude - t_moveAxisReduce);
	}

	public void UpdateRotation () {
		float t_inputHorizontal = Input.GetAxis ("Horizontal" + myControl);
		float t_inputVertical = Input.GetAxis ("Vertical" + myControl);
		if (Mathf.Abs(t_inputHorizontal) > 0.1f || Mathf.Abs(t_inputVertical) > 0.1f) {
			myRotation = Vector3.up * t_inputVertical + Vector3.right * t_inputHorizontal;
		}

		Quaternion t_quaternion = Quaternion.Euler (0, 0, 
			Vector2.Angle (Vector2.up, myRotation) * Mathf.Sign (myRotation.x * -1));

		this.transform.rotation = Quaternion.Lerp (this.transform.rotation, t_quaternion, Time.fixedDeltaTime * myRotationSpeed);
	}

	public void SetMyControl (string g_myControl) {
		myControl = g_myControl;
	}

	public void SetMyAbility (PP_Global.Abilities g_ability) {
		myAbility = g_ability;
		PP_MessageBox.Instance.SavePlayerAbility (myControl, g_ability);

		myAnimator.SetInteger ("ability", (int)myAbility);
//		myButt.GetComponent<PP_Butt>().SetBodySprite(int.Parse(myControl) % 3, )
	}

	public int GetMyTeamNumber () {
		return myTeamNumber;
	}

	public Color GetMyColor () {
		return myColor;
	}

	public Color GetMyColorDetail () {
		return myColorDetail;
	}

	public PP_Global.Abilities GetMyAbility () {
		return myAbility;
	}

	public GameObject GetMyButt () {
		return myButt;
	}

	public void Stun () {
		if (myStatus_IsFrozen)
			return;
		myStatus_StunTimer = myAbility_Burp_StunTime;
		myChargeTimer = 0;
		mySpriteRenderer.color = myStunColor;
		myAnimator.SetBool ("isStunned", true);
	}

	public void ToggleReady() {
		selectReady = !selectReady;
	}

	public bool GetReadyStatus() {
		return selectReady;
	}
}
