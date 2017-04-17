using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_CannonZone : MonoBehaviour {

//	// Use this for initialization
//	void Start () {
//		
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}

	void OnTriggerStay2D (Collider2D g_Collider2D) {
//		Debug.Log ("PP_CannonZone");
		if (g_Collider2D.tag == PP_Global.TAG_PLAYER) {
			PP_Cannon.Instance.ShellCharge ();
		}
	}
}
