using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Player : MonoBehaviour {
	
	private int myTeamNumber = 1;
	private string myControl = "1";
	[SerializeField] Rigidbody2D myRigidbody2D;
	[SerializeField] SpriteRenderer mySpriteRenderer;

	private Vector2 myDirection;
	private Vector2 myMoveAxis;
	[SerializeField] float mySpeed = 1;
	[SerializeField] float moveGravity;
	[SerializeField] float moveSensitivity;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		UpdateMove ();
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

		myRigidbody2D.velocity = myMoveAxis * mySpeed;
	
		float t_moveAxisReduce = Time.deltaTime * moveGravity;
		if (myMoveAxis.magnitude < t_moveAxisReduce)
			myMoveAxis = Vector2.zero;
		else
			myMoveAxis *= (myMoveAxis.magnitude - t_moveAxisReduce);

		//Debug.Log ("ControlMove" + myDirection + " : " +myMoveAxis);
	}

	public void Init (int g_teamNumber, Color g_color, string g_myControl) {
		myTeamNumber = g_teamNumber;
		SetMyControl (g_myControl);
		mySpriteRenderer.color = g_color;
	}

	public void SetMyControl (string g_myControl) {
		myControl = g_myControl;
	}

	public int GetMyTeamNumber () {
		return myTeamNumber;
	}
}
