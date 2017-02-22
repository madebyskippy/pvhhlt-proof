using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Player : MonoBehaviour {
	[SerializeField] bool isHeadUsing;

	[SerializeField] int myTeamNumber = 1;
	[SerializeField] Rigidbody2D myRigidbody2D;
	[SerializeField] SpriteRenderer mySpriteRenderer;
	[SerializeField] string myControlLeft = "1";
	[SerializeField] string myControlRight = "1";

	private Vector2 myDirection;
	private Vector2 myMoveAxis;
	private Vector2 myHeadDirection;
	private Vector2 myHeadMoveAxis;
	[SerializeField] float mySpeed = 1;
	[SerializeField] float moveGravity;
	[SerializeField] float moveSensitivity;

	[SerializeField] GameObject mySkillReady;
	[SerializeField] GameObject mySkill;
	[SerializeField] float mySkillCoolDown = 5;
	private float mySkillTimer = 0;

	[SerializeField] float myPushedTime = 2;
	[SerializeField] float myPushedSpeed = 1;
	private float myPushedTimer = 0;

	private Rigidbody2D myHeadRigidbody2D;
	[SerializeField] float myHeadSpeed = 1;
	[SerializeField] float myHeadPushedTime = 2;
	[SerializeField] float myHeadPushedSpeed = 1;
	private float myHeadPushedTimer = 0;

	[SerializeField] GameObject myHead;
	[SerializeField] GameObject myBody;


	// Use this for initialization
	void Start () {
//		myRigidbody2D = this.GetComponent<Rigidbody2D> ();
//		mySpriteRenderer = this.GetComponent<SpriteRenderer> ();
	}

	public void Init (int g_teamNumber, Color g_color, string g_myControlLeft, string g_myControlRight) {
		myTeamNumber = g_teamNumber;
		SetMyControl (g_myControlLeft, g_myControlRight);
		mySpriteRenderer.color = g_color;

		if (isHeadUsing) {
			//Head
			myHead = Instantiate (myHead, this.transform.position, Quaternion.identity) as GameObject;
			myHeadRigidbody2D = myHead.GetComponent<Rigidbody2D> ();
			myHead.GetComponent<CS_Head> ().SetMyPlayer (this.GetComponent<CS_Player> ());
			myHead.GetComponent<SpriteRenderer> ().color = g_color;
			myBody = Instantiate (myBody, this.transform.position, Quaternion.identity) as GameObject;
			myBody.GetComponent<SpriteRenderer> ().color = g_color;
		}
	}

	public void SetMyControl (string g_myControlLeft, string g_myControlRight) {
		myControlLeft = g_myControlLeft;
		myControlRight = g_myControlRight;
	}
	
	// Update is called once per frame
	void Update () {
		if (myPushedTimer > 0) {
			UpdatePushed ();
		} else {
			UpdateMove ();
		}

		if (isHeadUsing) {
			//Head
			if (myHeadPushedTimer > 0) {
				UpdateHeadPushed ();
			} else {
				UpdateHeadMove ();
			}
			UpdateBody ();

			UpdateSkill ();
		}
	}

	private void UpdateSkill () {
		if (mySkillTimer <= 0) {

			//can cast the skill

			if (Input.GetButtonDown ("Skill" + myControlRight)) {
				mySkillReady.SetActive (false);
				mySkillTimer = mySkillCoolDown;
//				Debug.Log ("Skill!!!" + myControlRight);
				GameObject t_skill = Instantiate (mySkill, this.transform.position, Quaternion.identity) as GameObject;
				t_skill.GetComponent<CS_Skill> ().SetMyCaster (this.gameObject);
			}

		} else {

			//in cool down, update cool down

			mySkillTimer -= Time.deltaTime;

			if (mySkillTimer <= 0) {
				mySkillReady.SetActive (true);
				mySkillTimer = 0;
			}
		}
	}

	private void UpdateBody () {
		Vector2 t_direction = this.transform.position - myHead.transform.position;
		Vector2 t_position = (this.transform.position + myHead.transform.position) / 2;

		Quaternion t_quaternion = Quaternion.Euler (0, 0, 
			Vector2.Angle (Vector2.up, t_direction) * Vector3.Cross (Vector3.up, (Vector3)t_direction).normalized.z);

		myBody.transform.position = t_position;
		myBody.transform.rotation = t_quaternion;
		myBody.transform.localScale = new Vector3 (myBody.transform.localScale.x, t_direction.magnitude, 1);
	}

	private void UpdateMove () {
		float t_inputHorizontal = Input.GetAxis ("Horizontal" + myControlLeft);
		float t_inputVertical = Input.GetAxis ("Vertical" + myControlLeft);
		myDirection = (Vector3.up * t_inputVertical + Vector3.right * t_inputHorizontal).normalized;

//		Camera.main.GetComponent<CS_Camera> ().SetPreMovePosition (myDirection);

		myMoveAxis += myDirection * moveSensitivity;
		if (myMoveAxis.magnitude > 1)
			myMoveAxis.Normalize ();

		//		Debug.Log ("ControlMove" + myDirection + " : " +myMoveAxis);

		//set the speed of the player
		myRigidbody2D.velocity = myMoveAxis * mySpeed;

		float t_moveAxisReduce = Time.deltaTime * moveGravity;
		if (myMoveAxis.magnitude < t_moveAxisReduce)
			myMoveAxis = Vector2.zero;
		else
			myMoveAxis *= (myMoveAxis.magnitude - t_moveAxisReduce);

		//Debug.Log ("ControlMove" + myDirection + " : " +myMoveAxis);
	}

	private void UpdateHeadMove () {
		float t_inputHorizontal = Input.GetAxis ("HorizontalR" + myControlRight);
		float t_inputVertical = Input.GetAxis ("VerticalR" + myControlRight);
		myHeadDirection = (Vector3.up * t_inputVertical + Vector3.right * t_inputHorizontal).normalized;

		//		Camera.main.GetComponent<CS_Camera> ().SetPreMovePosition (myHeadDirection);

		myHeadMoveAxis += myHeadDirection * moveSensitivity;
		if (myHeadMoveAxis.magnitude > 1)
			myHeadMoveAxis.Normalize ();

		//		Debug.Log ("ControlMove" + myHeadDirection + " : " +myHeadMoveAxis);

		//set the speed of the player
		myHeadRigidbody2D.velocity = myHeadMoveAxis * myHeadSpeed;

		float t_moveAxisReduce = Time.deltaTime * moveGravity;
		if (myHeadMoveAxis.magnitude < t_moveAxisReduce)
			myHeadMoveAxis = Vector2.zero;
		else
			myHeadMoveAxis *= (myHeadMoveAxis.magnitude - t_moveAxisReduce);

		//Debug.Log ("ControlMove" + myHeadDirection + " : " +myHeadMoveAxis);
	}

	private void UpdatePushed () {
		myRigidbody2D.velocity = Vector2.Lerp (myRigidbody2D.velocity.normalized * myPushedSpeed, Vector2.zero, (myPushedTime - myPushedTimer) / myPushedTime);
//		Debug.Log (myRigidbody2D.velocity);
		myPushedTimer -= Time.deltaTime;
		if (myPushedTimer <= 0) {
			myPushedTimer = 0;
		}
	}

	private void UpdateHeadPushed () {
		myHeadRigidbody2D.velocity = Vector2.Lerp (myHeadRigidbody2D.velocity.normalized * myHeadPushedSpeed, Vector2.zero, (myHeadPushedTime - myHeadPushedTimer) / myHeadPushedTime);
		//		Debug.Log (myRigidbody2D.velocity);
		myHeadPushedTimer -= Time.deltaTime;
		if (myHeadPushedTimer <= 0) {
			myHeadPushedTimer = 0;
		}
	}

	public int GetMyTeamNumber () {
		return myTeamNumber;
	}

	public void Pushed (int g_teamNumber, Vector2 g_direction) {
		if (g_teamNumber != myTeamNumber) {
			myRigidbody2D.velocity = g_direction * myPushedSpeed;
			myPushedTimer = myPushedTime;
		}
	}

	public void HeadPushed (int g_teamNumber, Vector2 g_direction) {
		if (g_teamNumber != myTeamNumber) {
			myHeadRigidbody2D.velocity = g_direction * myHeadPushedSpeed;
			myHeadPushedTimer = myHeadPushedTime;
		}
	}

	public void SetMyControllerLeft (string g_controller) {
		myControlLeft = g_controller;
	}

	public void SetMyControllerRight (string g_controller) {
		myControlRight = g_controller;
	}
}
