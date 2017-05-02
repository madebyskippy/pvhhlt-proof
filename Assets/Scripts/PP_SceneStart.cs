using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_SceneStart : MonoBehaviour {

	[SerializeField] Animator Elder_Eye_Red;
	[SerializeField] Animator Elder_Eye_Yellow;

	private float myBlinkProbability = 0.005f;

	// Use this for initialization
	void Start () {
		PP_TransitionManager.Instance.EndTransition ();
	}
	
	// Update is called once per frame
	void Update () {
		PP_PauseController.Instance.SetIsMenuActive (false);

		if (Input.GetButtonDown("Submit")) {
			PP_TransitionManager.Instance.StartTransition (PP_Global.SCENE_SELECT);
		}

		if (Random.Range (0f, 1f) < myBlinkProbability) {
			Debug.Log ("red blink");
			Elder_Eye_Red.SetTrigger ("blink");
		}
		if (Random.Range (0f, 1f) < myBlinkProbability) {
			Debug.Log ("yellow blink");
			Elder_Eye_Yellow.SetTrigger ("blink");
		}
	}
}
