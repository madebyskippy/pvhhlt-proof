using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SprtieMask : MonoBehaviour {
	[Range (0, 1)]
	[SerializeField] float myProgress;
	[SerializeField] bool isReversed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<SpriteRenderer> ().material.SetFloat ("_Progress", myProgress);
		if (!isReversed)
			this.GetComponent<SpriteRenderer> ().material.SetFloat ("_Reverse", 0);
		else
			this.GetComponent<SpriteRenderer> ().material.SetFloat ("_Reverse", 1);
	}
}
