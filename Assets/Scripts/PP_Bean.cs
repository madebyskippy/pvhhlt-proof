using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Bean : MonoBehaviour {

	/*
	 * will swim between two points set off screen
	 * 
	 * two states (set an enum)
	 * 		first state: 	idle (just moving around)
	 * 		second state:	running away
	 * 
	 * 
	 * todo: 
	 * 		avoid butt
	 * 		move when you poop them out
	 */

	[SerializeField] GameObject myEffect;
	[SerializeField] Sprite[] mySprites; //can eventually become Animations instead
	private PP_BeanManager myManager;
	private Vector3[] myTargets; //the two points it swims between
	private int myCurrentTarget;
	private float mySpeed = 0.05f;

	private Vector3 myDirection;

	void Start () {
		GetComponent<SpriteRenderer> ().sprite = mySprites [Random.Range (0, mySprites.Length)];

		myTargets = new Vector3[4];
		myTargets [0] = new Vector3 ( (Random.Range(0,2)*2-1) * 10f,Random.Range(-5f,5f),0f);
		myTargets [1] = new Vector3 ( myTargets[0].x * -1f,			Random.Range(-5f,5f),0f);
		myTargets [2] = new Vector3 ( myTargets[0].x,				Random.Range(-5f,5f),0f);
		myTargets [3] = new Vector3 ( myTargets[0].x * -1f,			Random.Range(-5f,5f),0f);

		transform.position = myTargets [0];
		myCurrentTarget = 1;
		getDirection (myCurrentTarget);
		Look();
	}

	void Update () {
		getDirection (myCurrentTarget);
		transform.position += myDirection * mySpeed;

		if ((myTargets[myCurrentTarget] - transform.position).magnitude < 0.5f) {
			myCurrentTarget = (myCurrentTarget + 1) % myTargets.Length;
			getDirection (myCurrentTarget);
			Look ();
		}
	}

	void getDirection(int g_target){
		myDirection = myTargets[g_target] - transform.position;
		myDirection.z = 0f;
		myDirection.Normalize ();
	}

	void Look(){
		Quaternion t_quaternion = Quaternion.Euler (0, 0, 
			Vector2.Angle (Vector2.right, myDirection) * Mathf.Sign (myDirection.y));
		transform.rotation = t_quaternion;
	}


	public void SetMyManager (PP_BeanManager g_manager) {
		myManager = g_manager;
	}

	public void Kill () {
		if (myEffect != null) {
			Instantiate (myEffect, this.transform.position, Quaternion.identity);
		}
		if (myManager != null) {
			myManager.RemoveBean (this.gameObject);
		}
		Destroy (this.gameObject);
	}
}
