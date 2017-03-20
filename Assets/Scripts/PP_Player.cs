using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Player : MonoBehaviour {

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
	private PP_Global.Abilities myAbility = PP_Global.Abilities.Burp;
	[Header(" - Burp")]
	[SerializeField] Transform myAbility_Burp_Position;
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
		SetMyAbility ((PP_Global.Abilities)((int.Parse (myControl) - 1) % 3));
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
	}
	
	// Update is called once per frame
	void Update () {
		if (myStatus_StunTimer <= 0 && !myStatus_IsFrozen) {
			UpdateMove ();
		}
		UpdateStatus ();
		UpdateAbility ();

		UpdateRotation ();
	}

	private void UpdateStatus () {
		if (myStatus_StunTimer > 0) {
			myStatus_StunTimer -= Time.deltaTime;
			if (myStatus_StunTimer <= 0) {
				myStatus_StunTimer = 0;
				mySpriteRenderer.color = myColor;
			}
		}
		if (myStatus_DashTimer > 0) {
			myStatus_DashTimer -= Time.deltaTime;
			if (myStatus_DashTimer <= 0) {
				myStatus_DashTimer = 0;
				myStatus_SpeedRatio = 1f;
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

		if (myStatus_StunTimer > 0) {
			return;
		}

		if (myAbility == PP_Global.Abilities.Burp && myCDTimer <= 0) {
			if (Input.GetButtonDown ("Skill" + myControl)) {
				myCDTimer = myAbility_Burp_CD;
				GameObject t_burp = Instantiate (myAbility_Burp_Prefab, myAbility_Burp_Position.position, Quaternion.identity) as GameObject;
//				t_burp.transform.parent = this.transform;
				t_burp.GetComponent<PP_Skill_Burp> ().Init (this.gameObject);
				myAnimator.SetTrigger ("isButtonDown");
			}
		} else if (myAbility == PP_Global.Abilities.Freeze) {
			if (Input.GetButtonDown ("Skill" + myControl)) {
				myStatus_IsFrozen = true;
				myRigidbody2D.isKinematic = true;
				myRigidbody2D.velocity = Vector3.zero;
				myAnimator.SetTrigger ("isButtonDown");
			} else if (Input.GetButtonUp ("Skill" + myControl)) {
				myStatus_IsFrozen = false;
				myRigidbody2D.isKinematic = false;
				myAnimator.SetTrigger ("isButtonUp");
			}
		} else if (myAbility == PP_Global.Abilities.Dash && myCDTimer <= 0) {
			if (Input.GetButtonDown ("Skill" + myControl)) {
				myCDTimer = myAbility_Dash_CD;
				myStatus_SpeedRatio = myAbility_Dash_SpeedRatio;
				myStatus_DashTimer = myAbility_Dash_Time;
				myAnimator.SetTrigger ("isButtonDown");
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

		//Rotation
	}

	public void UpdateRotation () {
		float t_inputHorizontal = Input.GetAxis ("Horizontal" + myControl);
		float t_inputVertical = Input.GetAxis ("Vertical" + myControl);
		if (Mathf.Abs(t_inputHorizontal) > 0.1f || Mathf.Abs(t_inputVertical) > 0.1f) {

//			Debug.Log (t_inputHorizontal + " " + t_inputVertical);
			myRotation = Vector3.up * t_inputVertical + Vector3.right * t_inputHorizontal;
		}
//		Debug.Log (myRotation);

		Quaternion t_quaternion = Quaternion.Euler (0, 0, 
			Vector2.Angle (Vector2.up, myRotation) * Mathf.Sign (myRotation.x * -1));

		this.transform.rotation = t_quaternion;
	}

	public void SetMyControl (string g_myControl) {
		myControl = g_myControl;
	}

	public void SetMyAbility (PP_Global.Abilities g_ability) {
		myAbility = g_ability;

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

	public void Stun () {
		if (myStatus_IsFrozen)
			return;
		myStatus_StunTimer = myAbility_Burp_StunTime;
		mySpriteRenderer.color = myStunColor;
	}
}
