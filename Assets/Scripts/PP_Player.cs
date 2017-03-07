using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Player : MonoBehaviour {

	private int myTeamNumber = 1;
	private string myControl = "1";
	[SerializeField] Rigidbody2D myRigidbody2D;
	[SerializeField] SpriteRenderer mySpriteRenderer;
	[SerializeField] Animator myAnimator;
	private Color myColor;
	[SerializeField] SpriteRenderer mySpriteRendererBack;

	private Vector2 myDirection;
	private Vector2 myMoveAxis;
	[SerializeField] float mySpeed = 1;
	[SerializeField] float moveGravity;
	[SerializeField] float moveSensitivity;

	[Header("Status")]
	[SerializeField] Color myStunColor = Color.gray;
	private float myStatus_StunTimer;
	private bool myStatus_IsFrozen = false;
	private float myStatus_SpeedRatio = 1;
	private float myStatus_DashTimer;
	private float myCDTimer;

	[Header("Ability")]
	[SerializeField] PP_Global.Abilities myAbility = PP_Global.Abilities.Burp;
	[Header(" - Burp")]
	[SerializeField] GameObject myAbility_Burp_Prefab;
	[SerializeField] Sprite myAbility_Burp_Sprite;
	[SerializeField] float myAbility_Burp_CD = 5;
	[SerializeField] float myAbility_Burp_StunTime = 4;
	[Header(" - Dash")]
	[SerializeField] float myAbility_Dash_CD = 2;
	[SerializeField] float myAbility_Dash_SpeedRatio = 10;
	[SerializeField] float myAbility_Dash_Time = 0.5f;
	[Header(" - Freeze")]
	[SerializeField] Sprite myAbility_Freeze_Sprite;

	// Use this for initialization
	void Start () {
		myAbility = (PP_Global.Abilities)((int.Parse (myControl) - 1) % 3);
		myAnimator.Play ("Player_" + myAbility.ToString () + "_Idle");
	}

	public void Init (int g_teamNumber, Color g_color, Color g_borderColor, string g_myControl) {
		myColor = g_color;
		myTeamNumber = g_teamNumber;
		SetMyControl (g_myControl);
		mySpriteRenderer.color = g_color;
		mySpriteRendererBack.color = g_borderColor;
	}
	
	// Update is called once per frame
	void Update () {
		if (myStatus_StunTimer <= 0 && !myStatus_IsFrozen) {
			UpdateMove ();
		}
		UpdateStatus ();
		UpdateAbility ();

	}

	private void UpdateStatus () {
		if (myStatus_StunTimer > 0) {
			myStatus_StunTimer -= Time.deltaTime;
			if (myStatus_StunTimer <= 0) {
				myStatus_StunTimer = 0;
				mySpriteRenderer.color = myColor;
				myAnimator.Play ("Player_" + myAbility.ToString () + "_Idle");
			}
		}
		if (myStatus_DashTimer > 0) {
			myStatus_DashTimer -= Time.deltaTime;
			if (myStatus_DashTimer <= 0) {
				myStatus_DashTimer = 0;
				myStatus_SpeedRatio = 1f;
				myAnimator.Play ("Player_" + myAbility.ToString () + "_Idle");
			}
		}
	}

	private void UpdateAbility () {
		if (myCDTimer > 0) {
			myCDTimer -= Time.deltaTime;
			if (myCDTimer <= 0) {
				myCDTimer = 0;
			}
		}

		if (myAbility == PP_Global.Abilities.Burp && myCDTimer <= 0) {
			if (Input.GetButtonDown ("Skill" + myControl)) {
				myCDTimer = myAbility_Burp_CD;
				GameObject t_burp = Instantiate (myAbility_Burp_Prefab, this.transform.position, Quaternion.identity) as GameObject;
				t_burp.GetComponent<PP_Skill_Burp> ().Init (this.gameObject);
				myAnimator.Play ("Player_" + myAbility.ToString () + "_Effect");
			}
		} else if (myAbility == PP_Global.Abilities.Freeze) {
			if (Input.GetButtonDown ("Skill" + myControl)) {
				myStatus_IsFrozen = true;
				myRigidbody2D.isKinematic = true;
				myAnimator.Play ("Player_" + myAbility.ToString () + "_Effect");
			} else if (Input.GetButtonUp ("Skill" + myControl)) {
				myStatus_IsFrozen = false;
				myRigidbody2D.isKinematic = false;
				myAnimator.Play ("Player_" + myAbility.ToString () + "_Idle");
			}
		} else if (myAbility == PP_Global.Abilities.Dash && myCDTimer <= 0) {
			if (Input.GetButtonDown ("Skill" + myControl)) {
				myCDTimer = myAbility_Dash_CD;
				myStatus_SpeedRatio = myAbility_Dash_SpeedRatio;
				myStatus_DashTimer = myAbility_Dash_Time;
				myAnimator.Play ("Player_" + myAbility.ToString () + "_Effect");
			}
		}
	}

	private void UpdateMove () {
		float t_inputHorizontal = Input.GetAxis ("Horizontal" + myControl);
		float t_inputVertical = Input.GetAxis ("Vertical" + myControl);
		myDirection = (Vector3.up * t_inputVertical + Vector3.right * t_inputHorizontal).normalized;

		//		Camera.main.GetComponent<CS_Camera> ().SetPreMovePosition (myDirection);

		myMoveAxis += myDirection * moveSensitivity;
		if (myMoveAxis.magnitude > 1)
			myMoveAxis.Normalize ();

		//		Debug.Log ("ControlMove" + myDirection + " : " +myMoveAxis);

		//set the speed of the player

//		myRigidbody2D.AddForce (myMoveAxis * mySpeed);

		myRigidbody2D.velocity = myMoveAxis * mySpeed * myStatus_SpeedRatio;
	
		float t_moveAxisReduce = Time.deltaTime * moveGravity;
		if (myMoveAxis.magnitude < t_moveAxisReduce)
			myMoveAxis = Vector2.zero;
		else
			myMoveAxis *= (myMoveAxis.magnitude - t_moveAxisReduce);

		//Debug.Log ("ControlMove" + myDirection + " : " +myMoveAxis);
	}

	public void SetMyControl (string g_myControl) {
		myControl = g_myControl;
	}

	public int GetMyTeamNumber () {
		return myTeamNumber;
	}

	public void Stun () {
		if (myStatus_IsFrozen)
			return;
		myStatus_StunTimer = myAbility_Burp_StunTime;
		mySpriteRenderer.color = myStunColor;
	}
}
