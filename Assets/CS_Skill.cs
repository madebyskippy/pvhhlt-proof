using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Skill : MonoBehaviour {
	protected GameObject myCaster;
	protected int myCasterTeamNumber;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetMyCaster (GameObject g_caster) {
		myCaster = g_caster;
		myCasterTeamNumber = g_caster.GetComponent<CS_Player> ().GetMyTeamNumber ();
	}



}
