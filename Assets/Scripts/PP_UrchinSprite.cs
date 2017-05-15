using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_UrchinSprite : MonoBehaviour {

//	SpriteRenderer spriteRend;
	GameObject spriteGO;
	GameObject fixedGO;
	Rigidbody2D rb;
	Vector2 moveDir;

	float moveSpeedX;
	float moveSpeedY;
	float xMod;

	float yMod;


	void Start () {
		rb = GetComponent<Rigidbody2D> ();
//		spriteRend = GetComponentInChildren<SpriteRenderer> ();

		spriteGO = transform.GetChild (0).GetChild(0).gameObject;
		fixedGO = transform.GetChild (0).gameObject;
	}
	

	void Update () {
		moveDir = rb.velocity;

		moveSpeedX = Mathf.Abs (moveDir.x);
		moveSpeedY = Mathf.Abs (moveDir.y);

		int direction = 1;
		if (moveSpeedX > moveSpeedY)
			direction = 1;
		else
			direction = 1;

		Vector2 newScale = new Vector2 (1+moveSpeedX*0.03f - moveSpeedY*0.03f, 1+moveSpeedY*0.03f - moveSpeedX*0.03f);


		fixedGO.transform.localScale = newScale;
		fixedGO.transform.rotation = Quaternion.Euler( new Vector2 (0, 0));
		spriteGO.transform.rotation = gameObject.transform.rotation;

	}

//	void OnCollisionEnter2D () {
//
//
//	}
}
