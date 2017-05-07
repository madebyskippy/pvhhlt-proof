using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_SceneStart : MonoBehaviour {

	[SerializeField] Animator Elder_Eye_Red;
	[SerializeField] Animator Elder_Eye_Yellow;
	[SerializeField] Sprite[] btnSprites;

	private float myBlinkProbability = 0.005f;
	private GameObject[] btns;
	private int selectIdx;
	private bool isStickActive = false;

	// Use this for initialization
	void Start () {
		PP_TransitionManager.Instance.EndTransition ();
		btns = new GameObject[3];
		btns [0] = transform.GetChild (0).gameObject;
		btns [1] = transform.GetChild (1).gameObject;
		btns [2] = transform.GetChild (2).gameObject;
		selectIdx = 0;
	}
	
	// Update is called once per frame
	void Update () {
		PP_PauseController.Instance.SetIsMenuActive (false);

		if (Input.GetAxisRaw ("Vertical") == 0) {
			isStickActive = false;
		}

		if (Input.GetAxisRaw ("Vertical") > 0 && !isStickActive) {
			if (selectIdx == 0) {
				selectIdx = 2;
			} else {
				selectIdx--;
			}
			isStickActive = true;
			ChangeBtnStatus ();
		}

		if (Input.GetAxisRaw ("Vertical") < 0 && !isStickActive) {
			if (selectIdx == 2) {
				selectIdx = 0;
			} else {
				selectIdx++;
			}
			isStickActive = true;
			ChangeBtnStatus ();
		}

		if (Input.GetButtonDown("Submit")) {
			if (selectIdx == 0) {
				PP_TransitionManager.Instance.StartTransition (PP_Global.SCENE_SELECT);
			} else if (selectIdx == 1) {
				Debug.Log ("change to tutorial");
//				PP_TransitionManager.Instance.StartTransition (PP_Global.SCENE_SELECT);
			} else {
				Debug.Log ("change to credits");
//				PP_TransitionManager.Instance.StartTransition (PP_Global.SCENE_SELECT);
			}
		}

		if (Random.Range (0f, 1f) < myBlinkProbability) {
//			Debug.Log ("red blink");
			Elder_Eye_Red.SetTrigger ("blink");
		}
		if (Random.Range (0f, 1f) < myBlinkProbability) {
//			Debug.Log ("yellow blink");
			Elder_Eye_Yellow.SetTrigger ("blink");
		}
	}

	void ChangeBtnStatus() {
		for (int i = 0; i < 3; i++) {
			if (selectIdx == i) {
				btns [i].GetComponent<SpriteRenderer> ().sprite = btnSprites [i * 2 + 1];
			} else {
				btns [i].GetComponent<SpriteRenderer> ().sprite = btnSprites [i * 2];
			}
		}
	}
}
