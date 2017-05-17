using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_CannonBall : MonoBehaviour {

	private int myTeamNumber = -1;
	private Vector3 myTargetPosition;

	private float myPosition;

	[SerializeField] float myScore = 1;
	[SerializeField] float mySpeedStart = 10;
	[SerializeField] float mySpeedMin = 1;
	[SerializeField] float mySpeedMaintainRatio = 0.5f;
	private float mySpeedCurrent;
	[SerializeField] float mySpeedRotateRatio = 0.2f;
	private Vector2 myDirection;

	[SerializeField] Sprite[] mySprites;
	private SpriteRenderer mySpriteRenderer;

	// Use this for initialization
	void Awake () {
		this.gameObject.SetActive (false);
		mySpriteRenderer = GetComponent<SpriteRenderer> ();
		mySpriteRenderer.sprite = mySprites [Random.Range (0, mySprites.Length)];
	}

	public void Init (Vector3 g_position, int g_teamNumber, Vector3 g_TargetPosition, float g_angle) {
		this.transform.position = g_position;

		myTeamNumber = g_teamNumber;

		myTargetPosition = g_TargetPosition;

		myDirection = Quaternion.Euler (0, 0, g_angle) * Vector2.up;
		//		Debug.Log (myDirection);
		transform.Rotate (0f, 0f, Random.Range (0,360));

		mySpeedCurrent = mySpeedStart;

		this.gameObject.SetActive (true);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		this.transform.position += (Vector3)myDirection * mySpeedCurrent * Time.fixedDeltaTime;
		float t_ratio = Mathf.Clamp01 (mySpeedRotateRatio * Time.fixedDeltaTime);
		myDirection = ((Vector2)(myTargetPosition - this.transform.position) * t_ratio + myDirection * (1 - t_ratio)).normalized;
//		Debug.Log (myDirection.magnitude + " " + t_ratio);
		this.transform.Rotate(new Vector3(0f,0f,0.25f));

		if (Mathf.Abs (this.transform.position.x) - Mathf.Abs (myTargetPosition.x) > 0) {
			//show paticle
			PP_Cannon.Instance.ShowCannonParticle (this.transform.position);

			PP_ScenePlay.Instance.AddScore (myTeamNumber, myScore, PP_Global.ScoreMethod.Cannon);
			PP_ScenePlay.Instance.ShowElderNibble (myTeamNumber);
			this.gameObject.SetActive (false);
			this.transform.position = Vector2.zero;
		}

		mySpeedCurrent = (mySpeedCurrent - mySpeedMin) * mySpeedMaintainRatio + mySpeedMin;
	}
}
