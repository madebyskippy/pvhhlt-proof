using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Point : MonoBehaviour {
	[SerializeField] Transform myTransform;
	[SerializeField] SpriteRenderer mySpriteRenderer;
	[SerializeField] Color myColorTeam1;
	[SerializeField] Color myColorTeam2;
	[SerializeField] Color myColorTempTeam1;
	[SerializeField] Color myColorTempTeam2;
	[SerializeField] float myHeadPower = 2;
	[SerializeField] float myButtPower = 1;
	private float myOwnershipLevel = 0; // 100 -> team1, -100 -> team2
	[SerializeField] float myOwnershipLevelMax = 10;

//	void Start () {
//	}

	void Update () {
		UpdateColor ();
	}

	void OnTriggerStay2D (Collider2D g_collider2D) {
		if (g_collider2D.tag == "Player") {
			CS_Player t_player = g_collider2D.gameObject.GetComponent<CS_Player> ();
			int t_teamNumber = t_player.GetMyTeamNumber ();
			if (t_teamNumber == 1) {
				myOwnershipLevel += Time.deltaTime * myButtPower;
			} else if (t_teamNumber == 2) {
				myOwnershipLevel -= Time.deltaTime * myButtPower;
			}
		}
		if (g_collider2D.tag == "Head") {
			CS_Head t_playerHead = g_collider2D.gameObject.GetComponent<CS_Head> ();
			int t_teamNumber = t_playerHead.GetMyTeamNumber ();
			if (t_teamNumber == 1) {
				myOwnershipLevel += Time.deltaTime * myHeadPower;
			} else if (t_teamNumber == 2) {
				myOwnershipLevel -= Time.deltaTime * myHeadPower;
			}
		}
//		Debug.Log (myOwnershipLevel);
	}

	private void UpdateColor () {
		if (myOwnershipLevel >= myOwnershipLevelMax) {
			myOwnershipLevel = 10;
			mySpriteRenderer.color = myColorTeam1;
			myTransform.localScale = Vector3.one;
			CS_MessageBox.Instance.AddScore (1);

		} else if (myOwnershipLevel <= -myOwnershipLevelMax) {
			myOwnershipLevel = -10;
			mySpriteRenderer.color = myColorTeam2;
			myTransform.localScale = Vector3.one;
			CS_MessageBox.Instance.AddScore (2);

		} else if (myOwnershipLevel > 0) {
			mySpriteRenderer.color = myColorTempTeam1;
			myTransform.localScale = Vector3.one * Mathf.Abs (myOwnershipLevel) / myOwnershipLevelMax;

		} else if (myOwnershipLevel < 0) {
			mySpriteRenderer.color = myColorTempTeam2;
			myTransform.localScale = Vector3.one * Mathf.Abs (myOwnershipLevel) / myOwnershipLevelMax;

		} else {
			myTransform.localScale = Vector3.one * Mathf.Abs (myOwnershipLevel) / myOwnershipLevelMax;
		}
			
	}
}
