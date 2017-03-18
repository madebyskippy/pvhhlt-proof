using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Body : MonoBehaviour {
	[SerializeField] SpriteRenderer mySpriteRenderer;
	[SerializeField] SpriteRenderer mySpriteRendererPattern;
//	// Use this for initialization
//	void Start () {
//		
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}

	public SpriteRenderer GetMySpriteRenderer () {
		return mySpriteRenderer;
	}

	public SpriteRenderer GetMySpriteRendererPattern () {
		return mySpriteRendererPattern;
	}
}
