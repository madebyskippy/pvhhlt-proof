using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Base : MonoBehaviour {

	[SerializeField] int myTeamNumber = 0;

	[SerializeField] int myScoreRatioBean = 10;
	[SerializeField] int myScoreRatioGrape = 100;

	[SerializeField] SpriteRenderer myScoreSpriteRenderer;
	[SerializeField] Gradient myScoreSpriteRenderer_Gradient;
	[SerializeField] float myScoreSpriteRenderer_DeltaScore = 0.1f;

	private enum Status : byte {
		Idle,
		Open,
		Close,
		Pop
	};
	private Status myStatus = Status.Idle;
	[SerializeField] Transform myScaleTransform;
	[SerializeField] float myScaleTime = 0.1f;
	[SerializeField] float myShrinkTime = 0.1f;
	[SerializeField] float myScaleScoreMax = 0.2f;
	private float myTimer = 0;
	private float myMaxSizeDelta = 0.2f;
	private float myNormalSize = 1;
	[SerializeField] Vector2 myDeltaSizeRange = new Vector2 (0.1f, 0.2f);

	[SerializeField] float myScoreSpeed = 1;
	[SerializeField] float myScoreDelta = 0.001f;
	private float myScoreTarget = 0;
	private float myScoreCurrent = 0;

	[SerializeField] Animator myAnimator;
	[SerializeField] GameObject myParticle;

	void Start () {
		myParticle.SetActive (false);
	}

	void Update () {
//		Debug.Log (myScoreCurrent);
		if (myScoreCurrent != myScoreTarget) {
			myScoreCurrent = Mathf.Lerp (myScoreCurrent, myScoreTarget, Time.deltaTime * myScoreSpeed);
			if (Mathf.Abs(myScoreTarget - myScoreCurrent) <= myScoreDelta) {
				myScoreCurrent = myScoreTarget;
			}
			myScoreSpriteRenderer.material.SetFloat ("_Progress", 1 - myScoreCurrent);

			myScoreSpriteRenderer.color = myScoreSpriteRenderer_Gradient.Evaluate (
				Mathf.Clamp ((Mathf.Abs (myScoreTarget - myScoreCurrent) / myScoreSpriteRenderer_DeltaScore), 0, 1));
		}

		UpdateSize ();
	}

	void OnTriggerEnter2D (Collider2D g_other) {
		if (g_other.tag == PP_Global.TAG_BUTT) {
			if (g_other.GetComponent<PP_Butt> ().GetBeansCurrent () > 0) {
				PP_ScenePlay.Instance.AddScore (myTeamNumber, g_other.GetComponent<PP_Butt> ().Pop () * myScoreRatioBean, PP_Global.ScoreMethod.Bean);
				ShowEat ();
			}
		} else if (g_other.tag == PP_Global.TAG_GRAPE) {
			g_other.GetComponent<PP_Grape> ().Kill ();
			PP_ScenePlay.Instance.AddScore (myTeamNumber, myScoreRatioGrape, PP_Global.ScoreMethod.Grape);
			ShowEat ();
//			PP_ScenePlay.Instance.AddScore (myTeamNumber, 0, myScoreRatioGrape);
		}
	}

	public void UpdateSize () {
		if (myStatus == Status.Open) {
			myScaleTransform.transform.localScale = Vector3.Lerp (Vector3.one * myNormalSize, Vector3.one * (myMaxSizeDelta + myNormalSize), 1 - myTimer / myScaleTime);			

			myTimer -= Time.deltaTime;
			if (myTimer <= 0) {
				myTimer = myShrinkTime;
				myStatus = Status.Close;
			}
		} else if (myStatus == Status.Close) {
			myTimer -= Time.deltaTime;
			if (myTimer <= 0) {
				myStatus = Status.Idle;
				myAnimator.SetInteger ("state", 0);
			}

			myScaleTransform.transform.localScale = Vector3.Lerp (Vector3.one * (myMaxSizeDelta + myNormalSize), Vector3.one * myNormalSize, 1 - myTimer / myShrinkTime);
		} 
	}

	public void StartSize (float g_score) {
		myMaxSizeDelta = Mathf.Clamp (g_score, 0, myScaleScoreMax) / myScaleScoreMax * (myDeltaSizeRange.y - myDeltaSizeRange.x) + myDeltaSizeRange.x;
		myTimer = myScaleTime;
		myStatus = Status.Open;
	}

	public void ShowScore (float g_scoreRatio) {
		myScoreTarget = g_scoreRatio;
		StartSize (myScoreTarget - myScoreCurrent);
	}

	public void ShowEat () {
		myAnimator.SetTrigger ("isEat");
	}

	public void ShowNibble () {
		myAnimator.SetTrigger ("isNibble");
	}

	public void ShowDead () {
		myAnimator.SetTrigger ("isDead");
	}

	public void ShowWinParticle () {
		myParticle.SetActive (true);
	}
}
