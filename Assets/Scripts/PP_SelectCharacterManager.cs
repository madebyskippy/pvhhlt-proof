using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_SelectCharacterManager : MonoBehaviour {

	private bool freezerShow = false;
	private bool burperShow = false;
	private bool dasherShow = false;
	private GameObject burper;
	private GameObject dasher;
	private GameObject freezer;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SwitchShow(PP_Global.Abilities currentAAbility){
		switch (currentAAbility) {
		case PP_Global.Abilities.Dash:
			burperShow = false;
			freezerShow = false;
			dasherShow = true;
			break;
		case PP_Global.Abilities.Freeze:
			burperShow = false;
			freezerShow = true;
			dasherShow = false;
			break;
		case PP_Global.Abilities.Burp:
			burperShow = true;
			freezerShow = false;
			dasherShow = false;
			break;
		}

		ChangeCharacter ();
	}

	public void SetColors(Color color, Color colorDetail) {
		burper.GetComponent<SpriteRenderer> ().color = color;
		burper.transform.GetChild (0).gameObject.GetComponent<SpriteRenderer> ().color = colorDetail;

		freezer.GetComponent<SpriteRenderer> ().color = color;
		freezer.transform.GetChild (0).gameObject.GetComponent<SpriteRenderer> ().color = colorDetail;

		dasher.GetComponent<SpriteRenderer> ().color = color;
		dasher.transform.GetChild (0).gameObject.GetComponent<SpriteRenderer> ().color = colorDetail;
	}

	void ChangeCharacter() {
		burper = transform.FindChild ("SelectBurp").gameObject;
		dasher = transform.FindChild ("SelectDash").gameObject;
		freezer = transform.FindChild ("SelectFreezer").gameObject;
		burper.SetActive (burperShow);
		freezer.SetActive (freezerShow);
		dasher.SetActive (dasherShow);
	}
}
