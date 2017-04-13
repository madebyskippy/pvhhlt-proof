using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_CannonBall : MonoBehaviour {

	private int myTeamNumber = -1;
	private Vector3 myTargetPosition;

	private float myPosition;

	[SerializeField] float myScore = 1;
	[SerializeField] float mySpeed = 1;
	[SerializeField] float mySpeedRatio = 1;
	private Vector2 myDirection;

	// Use this for initialization
	void Start () {
		this.gameObject.SetActive (false);
	}

	public void Init (Vector3 g_position, int g_teamNumber, Vector3 g_TargetPosition, float g_angle) {
		this.transform.position = g_position;

		myTeamNumber = g_teamNumber;

		myTargetPosition = g_TargetPosition;

		myDirection = Quaternion.Euler (0, 0, g_angle) * Vector2.up;
//		Debug.Log (myDirection);
		this.gameObject.SetActive (true);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		this.transform.position += (Vector3)myDirection * mySpeed * Time.fixedDeltaTime;
		float t_ratio = Mathf.Clamp01 (mySpeedRatio * Time.fixedDeltaTime);
		myDirection = ((Vector2)(myTargetPosition - this.transform.position) * t_ratio + myDirection * (1 - t_ratio)).normalized;
//		Debug.Log (myDirection.magnitude + " " + t_ratio);

		if (Mathf.Abs (this.transform.position.x) - Mathf.Abs (myTargetPosition.x) > 0) {
			PP_ScenePlay.Instance.AddScore (myTeamNumber, myScore, PP_Global.ScoreMethod.Cannon);
			this.gameObject.SetActive (false);
			this.transform.position = Vector2.zero;
		}
	}
}
