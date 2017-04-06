using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Bean : MonoBehaviour {

	/*
	 * two states (set an enum from global)
	 * 		first state: 	idle (just moving around)
	 * 		second state:	running away
	 * 
	 * 
	 * todo: 
	 * 		avoid butt
	 */

	[SerializeField] GameObject myEffect;
	[SerializeField] Sprite[] mySprites; //can eventually become Animations instead
	private PP_BeanManager myManager;
	private Vector3[] myTargets; //the two points it swims between
	private int myCurrentTarget;
	private float mySpeed = 0.05f;

	private GameObject[] myButts;
	[SerializeField] float myButtDistance = 2f;

	private PP_Global.BeanStatus myState;

	private Vector3 myDirection;

	public void Init (Vector3 g_spawnPoint, float g_spawnRadius, bool g_fromButt) {
		myButts = GameObject.FindGameObjectsWithTag ("Butt");
		GetComponent<SpriteRenderer> ().sprite = mySprites [Random.Range (0, mySprites.Length)];

		myTargets = new Vector3[5];
		for (int i = 0; i < myTargets.Length; i++) {
			myTargets [i] = g_spawnPoint + (Vector3)Random.insideUnitCircle.normalized * g_spawnRadius;
		}

		myState = PP_Global.BeanStatus.Idle;

		if (g_fromButt) {
			transform.position = g_spawnPoint;
			myCurrentTarget = 0;
		} else {
			transform.position = myTargets [0];
			myCurrentTarget = 1;
		}
		getDirection (myTargets[myCurrentTarget]);
		Look();
	}

	void Update () {
		if (myState == PP_Global.BeanStatus.Idle) {
			getDirection (myTargets [myCurrentTarget]);

			if ((myTargets[myCurrentTarget] - transform.position).magnitude < 0.5f) {
				myCurrentTarget = (myCurrentTarget + 1) % myTargets.Length;
				getDirection (myTargets[myCurrentTarget]);
				Look ();
			}

			//running away doesn't work yet so it's not going in
//			for (int i = 0; i < myButts.Length; i++) {
//				if (((Vector2)(myButts [i].transform.position)-(Vector2)(transform.position)).sqrMagnitude < myButtDistance) {
//					myState = PP_Global.BeanStatus.Running;
//					Vector3 t_direction = ((Vector2)(transform.position) - (Vector2)(myButts [i].transform.position));
//					Vector3 t_target = transform.position + t_direction.normalized * 100f;
//					getDirection (t_target);
//					Look ();
//					Debug.Log ("running!!");
//					break;
//				}
//			}

		} else if (myState == PP_Global.BeanStatus.Running) {
			myState = PP_Global.BeanStatus.Idle;
			for (int i = 0; i < myButts.Length; i++) {
				//stay running if either butt is too close
				if (((Vector2)(myButts [i].transform.position)-(Vector2)(transform.position)).sqrMagnitude < myButtDistance) {
					myState = PP_Global.BeanStatus.Running;
				}
			}
		}
		transform.position += myDirection * mySpeed;
	}

	void getDirection(Vector3 g_target){
		myDirection = g_target - transform.position;
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
