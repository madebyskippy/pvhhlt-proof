using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_CharacterSelect : MonoBehaviour {
	public PP_Global.Abilities ability;
	private GameObject sceneSelect;
	public bool changeable = false;

	private Color defaultColor;
	private SpriteRenderer sprite;

	Vector3 myScale;

	// Use this for initialization
	void Start () {
		sprite = this.GetComponent<SpriteRenderer> ();
		defaultColor = sprite.color;
		sceneSelect = GameObject.Find("SelectManager");
		myScale = this.transform.localScale;
	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player" && changeable) {
			if (!coll.gameObject.GetComponent<PP_Player> ().GetReadyStatus ()) {
				coll.gameObject.GetComponent<PP_Player> ().PlaySFX_Select (ability);
				coll.gameObject.GetComponent<PP_Player> ().SetMyAbility (ability);
				sceneSelect.GetComponent<PP_SceneSelect> ().UpdateSelection (false);
				sprite.color = coll.gameObject.GetComponent<PP_Player> ().GetMyColor ();
				this.transform.localScale = myScale * 1.05f;
			}
		}
//		
//		Debug.Log ();
	}

	void OnCollisionExit2D(Collision2D coll){
		if (coll.gameObject.tag == "Player" && changeable) {
			if (!coll.gameObject.GetComponent<PP_Player> ().GetReadyStatus ()) {
				sprite.color = defaultColor;
				this.transform.localScale = myScale;
			}
		}
	}
}
