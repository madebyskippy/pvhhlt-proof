using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_CharacterSelect : MonoBehaviour {
	public PP_Global.Abilities ability;
	private GameObject sceneSelect;

	// Use this for initialization
	void Start () {
		sceneSelect = GameObject.Find("SelectManager");
	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {
			coll.gameObject.GetComponent<PP_Player> ().SetMyAbility (ability);
			sceneSelect.GetComponent<PP_SceneSelect> ().UpdateSelection (true);
		}
//		
//		Debug.Log ();
	}
}
