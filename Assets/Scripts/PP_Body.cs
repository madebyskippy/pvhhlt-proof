using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Body : MonoBehaviour {
	[SerializeField] SpriteRenderer myPattern;
//	// Use this for initialization
//	void Start () {
//		
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}
	public SpriteRenderer GetMyPattern () {
		return myPattern;
	}
}
