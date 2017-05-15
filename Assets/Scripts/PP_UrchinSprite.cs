using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_UrchinSprite : MonoBehaviour {


//	SpriteRenderer spriteRend;
	[SerializeField] GameObject mySpriteGO;
	[SerializeField] GameObject myDirectionGO;
	private Rigidbody2D myRigidbody2D;
	private Vector2 myMoveDireciton;
	private float myMoveSpeed = 0;
	[SerializeField] float myScaleRatio = 0.1f;
	[SerializeField] float myScaleSpeed = 1f;

	float moveSpeedX;
	float moveSpeedY;


	void Awake () {
		myRigidbody2D = GetComponent<Rigidbody2D> ();
//		spriteRend = GetComponentInChildren<SpriteRenderer> ();
	}
	

	void FixedUpdate () {
		myMoveDireciton = myRigidbody2D.velocity.normalized;
		myMoveSpeed = myRigidbody2D.velocity.sqrMagnitude;

		Quaternion t_quaternion = Quaternion.Euler (0, 0, 
			Vector2.Angle (Vector2.up, myMoveDireciton) * Mathf.Sign (myMoveDireciton.x * -1));

//		myDirectionGO.transform.rotation = t_quaternion;
		myDirectionGO.transform.rotation = 
			Quaternion.Lerp (this.transform.rotation, t_quaternion, Time.fixedDeltaTime * myScaleSpeed);
		float t_ratio = myMoveSpeed * myScaleRatio + 1;
		myDirectionGO.transform.localScale = new Vector3 (1 / t_ratio, t_ratio, 1);

		mySpriteGO.transform.rotation = this.transform.rotation;

//		moveSpeedX = Mathf.Abs (moveDir.x);
//		moveSpeedY = Mathf.Abs (moveDir.y);
//
//		int direction = 1;
//		if (moveSpeedX > moveSpeedY)
//			direction = 1;
//		else
//			direction = 1;
//
//		Vector2 newScale = new Vector2 (1+moveSpeedX*0.03f - moveSpeedY*0.03f, 1+moveSpeedY*0.03f - moveSpeedX*0.03f);



//		fixedGO.transform.localScale = newScale;
//		fixedGO.transform.rotation = Quaternion.Euler( new Vector2 (0, 0));
//		spriteGO.transform.rotation = gameObject.transform.rotation;

	}

//	void OnCollisionEnter2D () {
//
//
//	}
}
