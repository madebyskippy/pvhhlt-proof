using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Bean : MonoBehaviour {

	[SerializeField] GameObject myEffect;
	[SerializeField] Animator myAnimator;
	private PP_BeanSpawnPoint myManager;
	private Vector3[] myTargets; //the two points it swims between
	private int myCurrentTarget;
	private float mySpeed = 0.05f;
	[SerializeField] float myIdleSpeed = 0.05f;
	[SerializeField] float myRunningSpeed = 0.1f;

	private GameObject[] myButts;
	[SerializeField] float myButtDistance = 6f;

	private PP_Global.BeanStatus myState;

	private float myStatus_FreezeTimer;
	[SerializeField] float myStatus_FreezeTime = 1f;

	private Vector3 myDirection;
	[SerializeField] float myRotationSpeed = 8f;
	private Quaternion myTargetRotation;

	void Start () {
		this.gameObject.SetActive (false);
	}

	public void Init (Vector3 g_position, Vector3 g_spawnPosition, float g_spawnRadius, PP_BeanSpawnPoint g_manager = null) {
		myManager = g_manager;

		this.transform.position = g_position;

		myButts = GameObject.FindGameObjectsWithTag ("Butt");

		myTargets = new Vector3[5];
		for (int i = 0; i < myTargets.Length; i++) {
			Vector3 t_targetPoint = g_spawnPosition + (Vector3)Random.insideUnitCircle.normalized * g_spawnRadius;
			t_targetPoint.x = Mathf.Clamp (t_targetPoint.x, PP_BeanManager.Instance.GetBounds ().x * -1f, PP_BeanManager.Instance.GetBounds ().x);
			t_targetPoint.y = Mathf.Clamp (t_targetPoint.y, PP_BeanManager.Instance.GetBounds ().y * -1f, PP_BeanManager.Instance.GetBounds ().y);
			myTargets [i] = t_targetPoint;
		}

		myState = PP_Global.BeanStatus.Idle;

		if (g_manager == null) {
			transform.position = g_spawnPosition;
			myCurrentTarget = 0;
		} else {
			transform.position = myTargets [0];
			myCurrentTarget = 1;
		}
		GetDirection (myTargets[myCurrentTarget]);
		Look();

		this.gameObject.SetActive (true);
		myAnimator.SetInteger ("Type", (int)Random.Range (0, 4));
	}

	void FixedUpdate () {
		if (myState == PP_Global.BeanStatus.Idle) {
			GetComponent<Animator> ().SetBool ("isStunned", false);
			GetComponent<SpriteRenderer> ().color = Color.white;
			GetDirection (myTargets [myCurrentTarget]);
			Look ();
			mySpeed = myIdleSpeed;

			if ((myTargets [myCurrentTarget] - transform.position).magnitude < 0.5f) {
				myCurrentTarget = (myCurrentTarget + 1) % myTargets.Length;
				GetDirection (myTargets [myCurrentTarget]);
				Look ();
			}

			//check if it should run away from a butt
			int t_ClosestButt = -1;
			float t_ClosestDistance = 0f;
			for (int i = 0; i < myButts.Length; i++) {
				if (((Vector2)(myButts [i].transform.position) - (Vector2)(transform.position)).sqrMagnitude < myButtDistance) {
					float t_Distance = ((Vector2)(myButts [i].transform.position) - (Vector2)(transform.position)).sqrMagnitude;
					if (t_Distance < t_ClosestDistance || t_ClosestDistance == 0f) {
						t_ClosestButt = i;
						t_ClosestDistance = t_Distance;
					}
				}
			}
			if (t_ClosestButt > -1) {
				myState = PP_Global.BeanStatus.Running;
				Vector3 t_direction = ((Vector2)(transform.position) - (Vector2)(myButts [t_ClosestButt].transform.position));
				Vector3 t_target = transform.position + t_direction.normalized * 100f;
				GetDirection (t_target);
				Look ();
			}

		} else if (myState == PP_Global.BeanStatus.Running) {
//			GetComponent<SpriteRenderer> ().color = Color.red;
			mySpeed = myRunningSpeed;
			PP_Global.BeanStatus t_MyState = PP_Global.BeanStatus.Idle;
			for (int i = 0; i < myButts.Length; i++) {
				//stay running if either butt is too close
				if (((Vector2)(myButts [i].transform.position) - (Vector2)(transform.position)).sqrMagnitude < myButtDistance * 2f) {
					Vector3 t_direction = ((Vector2)(transform.position) - (Vector2)(myButts [i].transform.position));
					Vector3 t_target = transform.position + t_direction.normalized * 100f;
					GetDirection (t_target);
					Look ();
					t_MyState = PP_Global.BeanStatus.Running;
				}
			}
			myState = t_MyState;
		} else if (myState == PP_Global.BeanStatus.Frozen) {
			mySpeed = 0f;
			myStatus_FreezeTimer += Time.deltaTime;
			if (myStatus_FreezeTimer > myStatus_FreezeTime) {
				myState = PP_Global.BeanStatus.Idle;
			}
		}

		transform.position += myDirection * mySpeed * Time.fixedDeltaTime;
		UpdateRotation ();
	}

	void GetDirection(Vector3 g_target){
		myDirection = g_target - transform.position;
		myDirection.z = 0f;
		myDirection.Normalize ();
	}

	void Look(){
		Quaternion t_quaternion = Quaternion.Euler (0, 0, 
			Vector2.Angle (Vector2.right, myDirection) * Mathf.Sign (myDirection.y));
//		transform.rotation = t_quaternion;
		myTargetRotation = t_quaternion;
	}


	public void Kill () {
		
		if (myEffect != null) {
			Instantiate (myEffect, this.transform.position, Quaternion.identity);
		}
		if (myManager != null) {
			myManager.RemoveBean ();
		}

		this.gameObject.SetActive (false);
	}

	public void Stun () {
		if (myState != PP_Global.BeanStatus.Frozen) {
			Debug.Log ("bean down");
			GetComponent<Animator> ().SetBool ("isStunned", true);
			GetComponent<SpriteRenderer> ().color = new Color (0.75f, 0.75f, 0.75f);
			myState = PP_Global.BeanStatus.Frozen;
			myStatus_FreezeTimer = 0f;
		}
	}

	void UpdateRotation () {
		this.transform.rotation = Quaternion.Lerp (this.transform.rotation, myTargetRotation, Time.fixedDeltaTime * myRotationSpeed);
	}
}
