﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Skill_Burp : MonoBehaviour {
	private GameObject myCaster;
	private Transform myTransform;
	[SerializeField] SpriteRenderer mySprite;
	// Use this for initialization
	void Start () {
		//		this.GetComponent<Animator> ().Play ("Burp");
//		this.GetComponent<Animator> ().enabled = true;
	}
	
	// Update is called once per frame
//	void FixedUpdate () {
//		this.transform.position = myTransform.position;
//		this.transform.rotation = myTransform.rotation;
//	}

	public void UpdateTransform () {
		this.transform.position = myTransform.position;
		this.transform.rotation = myTransform.rotation;
	}

	public void Init (GameObject g_caster, Transform g_position) {
		myCaster = g_caster;
		myTransform = g_position;
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (myCaster != collider.gameObject && collider.tag == PP_Global.TAG_PLAYER){
			collider.gameObject.GetComponent<PP_Player> ().Stun ();
		}

		if (collider.tag == PP_Global.TAG_BEAN){
			collider.gameObject.GetComponent<PP_Bean> ().Stun ();
		}

		if (collider.tag == PP_Global.TAG_BUTT) {
			collider.gameObject.GetComponent<PP_Butt> ().Stun ();
		}
	}

	public void HideMyself () {
		Debug.Log ("HideMyself");
		this.gameObject.SetActive (false);
//		this.GetComponent<Animator> ().enabled = false;
	}

	public void SetColor (Color g_color) {
		mySprite.color = g_color;
	}
}
