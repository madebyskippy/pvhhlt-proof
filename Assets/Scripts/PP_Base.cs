using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Base : MonoBehaviour {

	[SerializeField] int myTeamNumber = 0;

	[SerializeField] int myScoreRatioBean = 10;
	[SerializeField] int myScoreRatioGrape = 100;

	[SerializeField] SpriteRenderer myScoreSpriteRenderer;

	[SerializeField] float myScoreSpeed = 1;
	[SerializeField] float myScoreDelta = 0.001f;
	private float myScoreTarget = 0;
	private float myScoreCurrent = 0;

	[SerializeField] Animator myAnimator;


	void Update () {
		Debug.Log (myScoreCurrent);
		if (myScoreCurrent != myScoreTarget) {
			myScoreCurrent = Mathf.Lerp (myScoreCurrent, myScoreTarget, Time.deltaTime * myScoreSpeed);
			if (Mathf.Abs(myScoreTarget - myScoreCurrent) <= myScoreDelta) {
				myScoreCurrent = myScoreTarget;
			}
			myScoreSpriteRenderer.material.SetFloat ("_Progress", 1 - myScoreCurrent);
		}
	}

	void OnTriggerEnter2D (Collider2D g_other) {
		if (g_other.tag == PP_Global.TAG_BUTT) {
//			Debug.Log ("POP");
			PP_ScenePlay.Instance.AddScore (myTeamNumber, g_other.GetComponent<PP_Butt> ().Pop () * myScoreRatioBean, PP_Global.ScoreMethod.Bean);
			ShowEat ();
//			PP_ScenePlay.Instance.AddScore (myTeamNumber, 1, g_other.GetComponent<PP_Butt> ().Pop () * myScoreRatioBean);
		} else if (g_other.tag == PP_Global.TAG_GRAPE) {
			g_other.GetComponent<PP_Grape> ().Kill ();
			PP_ScenePlay.Instance.AddScore (myTeamNumber, myScoreRatioGrape, PP_Global.ScoreMethod.Grape);
			ShowEat ();
//			PP_ScenePlay.Instance.AddScore (myTeamNumber, 0, myScoreRatioGrape);
		}
	}

	public void ShowScore (float g_scoreRatio) {
		myScoreTarget = g_scoreRatio;
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
}
