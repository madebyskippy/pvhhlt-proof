using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Skill_Burp : MonoBehaviour {
	private GameObject myCaster;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Init (GameObject g_caster) {
		myCaster = g_caster;
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (myCaster != collider.gameObject && collider.tag == PP_Global.TAG_PLAYER){
			collider.gameObject.GetComponent<PP_Player> ().Stun ();
		}
	}
}
