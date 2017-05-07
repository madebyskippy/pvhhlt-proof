using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_SceneTutorial : MonoBehaviour {
	[SerializeField] SpriteRenderer myTutorialSpriteRenderer;
	[SerializeField] Sprite[] myTutorialSlides; 
	private int myCurrentSlide = 0;

	// Use this for initialization
	void Start () {
		myCurrentSlide = 0;
		myTutorialSpriteRenderer.sprite = myTutorialSlides [myCurrentSlide];
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Submit")) {
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
