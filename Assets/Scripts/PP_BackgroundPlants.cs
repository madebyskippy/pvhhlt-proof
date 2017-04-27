using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_BackgroundPlants : MonoBehaviour {

	[SerializeField] Animator myAnimator;

	[SerializeField] AnimationClip clip;

	// Use this for initialization
	void Start () {

		string clipName = clip.name;
		myAnimator.Play (clipName);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
