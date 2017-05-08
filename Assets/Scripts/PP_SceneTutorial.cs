using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JellyJoystick;

public class PP_SceneTutorial : MonoBehaviour {
	[SerializeField] SpriteRenderer myTutorialSpriteRenderer;
	[SerializeField] Sprite[] myTutorialSlides; 
	private int myCurrentSlide = 0;

	// Use this for initialization
	void Start () {
		PP_TransitionManager.Instance.EndTransition ();
		myCurrentSlide = 0;
		myTutorialSpriteRenderer.sprite = myTutorialSlides [myCurrentSlide];
	}
	
	// Update is called once per frame
	void Update () {
		if (JellyJoystickManager.Instance.GetButton (ButtonMethodName.Down, 0, JoystickButton.A)) {
			ShowNextSlide ();
		}
	}

	private void ShowNextSlide () {
		Debug.Log ("ShowNextSlide");
		myCurrentSlide++;
		myCurrentSlide %= myTutorialSlides.Length;
		myTutorialSpriteRenderer.sprite = myTutorialSlides [myCurrentSlide];
	}
}
