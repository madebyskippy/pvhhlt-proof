﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Cannon : MonoBehaviour {
	
	private static PP_Cannon instance = null;

	//========================================================================
	public static PP_Cannon Instance {
		get { 
			return instance;
		}
	}

	void Awake () {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
		} else {
			instance = this;
		}

//		DontDestroyOnLoad(this.gameObject);
	}
	//========================================================================

	[SerializeField] float myMaxScorePerSecond = 2;
	[SerializeField] Transform myCannon;
	[SerializeField] HingeJoint2D myHingeJoint2D;
	private float myLimitsMax;
	private float myLimitsMin;
	private float myLimitsCenter;
	private float myAngleMax;

	[SerializeField] Transform myCannonHole;
	[SerializeField] GameObject myCannonBallPrefab;
	[SerializeField] int myCannonBallMaxNumber = 30;
	private List<GameObject> myCannonBallPool = new List<GameObject> ();
	private float myCannonTimer = 0;

	[SerializeField] GameObject myCannonParticlePrefab;
	[SerializeField] int myCannonParticleMaxNumber = 5;
	private List<GameObject> myCannonParticlePool = new List<GameObject> ();


	[SerializeField] Animator mySnakeAnimator;
	[SerializeField] Animator myClamAnimator;

	[SerializeField] Transform[] myBases;

	[Header("Shell")]
	[SerializeField] Transform myClam;
	[SerializeField] Transform myShellLeft;
	[SerializeField] Transform myShellRight;
	private float myShellCharge = 0;
	[SerializeField] float myShakeThreshold = 35;
	[SerializeField] float myShakeThresholdHarder = 50;
	[SerializeField] float myShellChargeMax = 60;
	[SerializeField] float myShellChargePerSecond = 1;
	private float myShellTimer;
	[SerializeField] float myShellClosedTime = 10;
	private float myShellAngleTarget;
	[SerializeField] float myShellRotationSpeed = 10;
	[SerializeField] Vector2 myShellAngleRange = new Vector2 (60, 120);
	[SerializeField] float myShellAngleClosed = 0;

	[SerializeField] AudioClip mySFX_Close;
	[SerializeField] float myChargeSoundCharge = 0;
	[SerializeField] float myChargeSoundInterval = 5;
	[SerializeField] AudioClip mySFX_Charge;
	[SerializeField] AudioClip mySFX_ChargeDanger;

	// Use this for initialization
	void Start () {
		myLimitsMax = myHingeJoint2D.limits.max;
		myLimitsMin = myHingeJoint2D.limits.min;
		myLimitsCenter = (myLimitsMax + myLimitsMin) / 2.0f;
		myAngleMax = myLimitsMax - myLimitsCenter;

//		Debug.Log (myLimitsMax + ":" + myLimitsMin + ":" + myLimitsCenter + ":" + myAngleMax);

		//Init the ball pool
		for (int i = 0; i < myCannonBallMaxNumber; i++) {
			myCannonBallPool.Add (Instantiate (myCannonBallPrefab, this.transform));
		}

		//Init the particle pool
		for (int i = 0; i < myCannonParticleMaxNumber; i++) {
			myCannonParticlePool.Add (Instantiate (myCannonParticlePrefab, this.transform));
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		UpdateShoot ();
		UpdateShell ();
	}

	public void ShowCannonParticle (Vector3 t_position) {
		for (int i = 0; i < myCannonParticlePool.Count; i++) {
			if (myCannonParticlePool [i].activeSelf == false) {
				myCannonParticlePool [i].GetComponent<PP_CannonParticle> ().Init (t_position);
				break;
			}

			if (i == myCannonParticlePool.Count - 1) {
				Debug.Log ("Run out of cannon particle!");

				GameObject t_newParticle = Instantiate (myCannonParticlePrefab, this.transform) as GameObject;
				myCannonParticlePool.Add (t_newParticle);
				t_newParticle.GetComponent<PP_CannonParticle> ().Init (t_position);
				break;
			}
		}
	}

	private void UpdateShoot () {
		if (myShellTimer > 0)
			return;

		int t_myOwnerNumber = -1;

		float t_angle = myCannon.rotation.eulerAngles.z;
		if (t_angle > 180) {
			t_angle -= 360;
		}
		t_angle = Mathf.Clamp (t_angle, myLimitsMin, myLimitsMax);

		myCannonTimer += Mathf.Abs (t_angle - myLimitsCenter) / myAngleMax * myMaxScorePerSecond * Time.fixedDeltaTime;

		//		Debug.Log (t_angle);
		if (t_angle > myLimitsCenter) {
			t_myOwnerNumber = 0;
			//			Debug.Log ("0");
		} else if (t_angle < myLimitsCenter) {
			t_myOwnerNumber = 1;
			//			Debug.Log ("1");
		}

		if (t_myOwnerNumber != -1) {
			if (myCannonTimer > 1) {

				for (int i = 0; i < myCannonBallPool.Count; i++) {
					if (myCannonBallPool [i].activeSelf == false) {
						myCannonBallPool [i].GetComponent<PP_CannonBall>().Init (
							myCannonHole.position,
							t_myOwnerNumber, 
							myBases [t_myOwnerNumber].position, 
							t_angle
						);
						mySnakeAnimator.SetTrigger ("isShooting");
						myCannonTimer -= 1;
						break;
					}

					if (i == myCannonBallPool.Count - 1) {
						Debug.Log ("Run out of cannon ball!");
						GameObject t_newBall = Instantiate (myCannonBallPrefab, this.transform) as GameObject;
						myCannonBallPool.Add (t_newBall);
						t_newBall.GetComponent<PP_CannonBall>().Init (
							myCannonHole.position,
							t_myOwnerNumber, 
							myBases [t_myOwnerNumber].position, 
							t_angle
						);
						mySnakeAnimator.SetTrigger ("isShooting");
						myCannonTimer -= 1;
						break;
					}
				}
			}
		}

		myCannon.rotation.eulerAngles.Set (0, 0, t_angle);

//		myCannon.transform.localPosition = Vector3.zero;
	}

	private void UpdateShell () {
		//shell rotate
		myClam.rotation = myCannon.rotation;

		if (myShellTimer > 0) {
			myShellTimer -= Time.fixedDeltaTime;
			myClamAnimator.SetFloat ("timer", myShellTimer);
			if (myShellTimer <= 0) {
				//Open
				myShellTimer = 0;
				myCannon.gameObject.SetActive (true);
				myClamAnimator.SetTrigger ("opening");
			}
		} else {
			myShellAngleTarget = 
				(myShellAngleRange.y - myShellAngleRange.x) * myShellCharge / myShellChargeMax + myShellAngleRange.x;
		}

		myShellLeft.localRotation = Quaternion.Lerp (
			myShellLeft.localRotation, 
			Quaternion.Euler (0, 0, myShellAngleTarget), 
			Time.fixedDeltaTime * myShellRotationSpeed
		);

		myShellRight.localRotation = Quaternion.Lerp (
			myShellRight.localRotation, 
			Quaternion.Euler (0, 0, -myShellAngleTarget), 
			Time.fixedDeltaTime * myShellRotationSpeed
		);

	}

	public void ShellCharge () {
		if (myShellTimer > 0)
			return;
		
		myShellCharge += Time.fixedDeltaTime * myShellChargePerSecond;

		if (myShellCharge > myShakeThresholdHarder) {
			myClamAnimator.SetInteger ("chargeShake", 2);
		} else if (myShellCharge > myShakeThreshold) {
			myClamAnimator.SetInteger ("chargeShake", 1);
		}
			
		myChargeSoundCharge += Time.fixedDeltaTime * myShellChargePerSecond;

		if (myChargeSoundCharge > myChargeSoundInterval) {
			if (myShellCharge > myShakeThresholdHarder) {
				CS_AudioManager.Instance.PlaySFX (mySFX_ChargeDanger);
			} else {
				myClamAnimator.SetInteger ("chargeShake", 1);
				CS_AudioManager.Instance.PlaySFX (mySFX_Charge);
			}
			myChargeSoundCharge = 0;
		}

		if (myShellCharge > myShellChargeMax) {
			//Close
			myShellTimer = myShellClosedTime;
			myShellCharge = 0;
			myShellAngleTarget = myShellAngleClosed;
			myCannon.gameObject.SetActive (false);
			myChargeSoundCharge = 0;

			CS_AudioManager.Instance.PlaySFX (mySFX_Close);
//			Invoke ("PlayCloseSound", 0.3f);
			myClamAnimator.SetTrigger ("closing");
			myClamAnimator.SetInteger ("chargeShake", 0);
		}
	}

	private void PlayCloseSound () {
		CS_AudioManager.Instance.PlaySFX (mySFX_Close);
	}
}
