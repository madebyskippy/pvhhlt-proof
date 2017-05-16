using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Grape : MonoBehaviour {

	[SerializeField] GameObject myEffect;
	private PP_GrapeManager myManager;

	[SerializeField] GameObject mySpriteGO;
	[SerializeField] GameObject myDirectionGO;
	private Rigidbody2D myRigidbody2D;
	private Vector2 myMoveDireciton;
	private float myMoveSpeed = 0;
	[SerializeField] float myScaleRatio = 1;
	[Range (1.01f, 2)]
	[SerializeField] float myScaleMax = 1.5f;
	private float myScaleOffset;
	[SerializeField] float myScaleSpeed = 1f;

	void Awake () {
		myRigidbody2D = GetComponent<Rigidbody2D> ();
		myScaleOffset = myScaleRatio / (myScaleMax - 1);
	}

	void FixedUpdate () {
		myMoveDireciton = myRigidbody2D.velocity.normalized;
		myMoveSpeed = myRigidbody2D.velocity.sqrMagnitude;
//		myMoveSpeed = myRigidbody2D.velocity.magnitude;

		Quaternion t_quaternion = Quaternion.Euler (0, 0, 
			Vector2.Angle (Vector2.up, myMoveDireciton) * Mathf.Sign (myMoveDireciton.x * -1));

//		myDirectionGO.transform.rotation = t_quaternion;
		myDirectionGO.transform.rotation = 
			Quaternion.Lerp (myDirectionGO.transform.rotation, t_quaternion, Time.fixedDeltaTime * myScaleSpeed);
		
		float t_ratio = myScaleRatio * -1 / (myMoveSpeed + myScaleOffset) + myScaleMax;

		Vector3 t_scale = new Vector3 (1 / t_ratio, t_ratio, 1);

		myDirectionGO.transform.localScale = 
			Vector3.Lerp (myDirectionGO.transform.localScale, t_scale, Time.fixedDeltaTime * myScaleSpeed);

		mySpriteGO.transform.rotation = this.transform.rotation;
	}

	public void SetMyManager (PP_GrapeManager g_manager) {
		myManager = g_manager;
	}

	public void Kill () {
		if (myEffect != null) {
			Instantiate (myEffect, this.transform.position, Quaternion.identity);
		}
		myManager.StartSpawnTimer ();
		this.gameObject.SetActive (false);
	}
}
