using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Cannon : MonoBehaviour {
	[SerializeField] float myMaxScorePerSecond = 2;
	[SerializeField] Transform myCannon;
	[SerializeField] HingeJoint2D myHingeJoint2D;
	private float myLimitsMax;
	private float myLimitsMin;
	private float myLimitsCenter;
	private float myAngleMax;

	// Use this for initialization
	void Start () {
		myLimitsMax = myHingeJoint2D.limits.max;
		myLimitsMin = myHingeJoint2D.limits.min;
		myLimitsCenter = (myLimitsMax + myLimitsMin) / 2.0f;
		myAngleMax = myLimitsMax - myLimitsCenter;

		Debug.Log (myLimitsMax + ":" + myLimitsMin + ":" + myLimitsCenter + ":" + myAngleMax);
	}
	
	// Update is called once per frame
	void Update () {
		int t_myOwnerNumber = -1;

		float t_angle = myCannon.rotation.eulerAngles.z;
		if (t_angle > 180) {
			t_angle -= 360;
		}
		t_angle = Mathf.Clamp (t_angle, myLimitsMin, myLimitsMax);

		Debug.Log (t_angle);
		if (t_angle > myLimitsCenter) {
			t_myOwnerNumber = 0;
			Debug.Log ("0");
		} else if (t_angle < myLimitsCenter) {
			t_myOwnerNumber = 1;
			Debug.Log ("1");
		}

		if (t_myOwnerNumber != -1) {
			PP_ScenePlay.Instance.AddScore (t_myOwnerNumber, Mathf.Abs (t_angle - myLimitsCenter) / myAngleMax * myMaxScorePerSecond * Time.deltaTime);
		}
	}
}
