﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Player : MonoBehaviour {

	private int myTeamNumber = 1;
	private string myControl = "1";
	[SerializeField] Rigidbody2D myRigidbody2D;
	[SerializeField] SpriteRenderer mySpriteRenderer;
	[SerializeField] SpriteRenderer mySpriteRendererBack;

	private Vector2 myDirection;
	private Vector2 myMoveAxis;
	[SerializeField] float mySpeed = 1;
	[SerializeField] float moveGravity;
	[SerializeField] float moveSensitivity;

	[Header("Ability")]
	[SerializeField] PP_Global.Abilities myAbility = PP_Global.Abilities.Burp;
	private float myCDTimer;
	[Header(" - Burp")]
	[SerializeField] GameObject myAbility_Burp_Prefab;
	[SerializeField] Sprite myAbility_Burp_Sprite;
	[SerializeField] float myAbility_Burp_CD = 5;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		UpdateMove ();
		UpdateAbility ();
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
				Instantiate (myAbility_Burp_Prefab, this.transform.position, Quaternion.identity);
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

		myRigidbody2D.velocity = myMoveAxis * mySpeed;
	
		float t_moveAxisReduce = Time.deltaTime * moveGravity;
		if (myMoveAxis.magnitude < t_moveAxisReduce)
			myMoveAxis = Vector2.zero;
		else
			myMoveAxis *= (myMoveAxis.magnitude - t_moveAxisReduce);

		//Debug.Log ("ControlMove" + myDirection + " : " +myMoveAxis);
	}

	public void Init (int g_teamNumber, Color g_color, Color g_borderColor, string g_myControl) {
		myTeamNumber = g_teamNumber;
		SetMyControl (g_myControl);
		mySpriteRenderer.color = g_color;
		mySpriteRendererBack.color = g_borderColor;
	}

	public void SetMyControl (string g_myControl) {
		myControl = g_myControl;
	}

	public int GetMyTeamNumber () {
		return myTeamNumber;
	}
}
