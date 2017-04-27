using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_StartController : MonoBehaviour {

	[SerializeField] Animator Elder_Eye_Red;
	[SerializeField] Animator Elder_Eye_Yellow;

	private float myBlinkProbability = 0.005f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Submit")) {
			Application.LoadLevel("Select");
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
