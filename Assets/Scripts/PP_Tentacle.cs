using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Tentacle : MonoBehaviour {

	Rigidbody2D rb;
	float force = 5f;
	float dirX;
	float dirY;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {

		dirX = Input.GetAxis ("Horizontal");
		dirY = Input.GetAxis ("Vertical");
		
	}

	void FixedUpdate() {
		rb.AddForce (Vector2.right *force * dirX,ForceMode2D.Force);
		rb.AddForce (Vector2.up *force * dirY,ForceMode2D.Force);
	}
}
