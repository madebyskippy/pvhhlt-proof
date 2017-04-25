using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PP_TransitionManager : MonoBehaviour {

	private static PP_TransitionManager instance = null;

	//========================================================================
	public static PP_TransitionManager Instance {
		get { 
			return instance;
		}
	}

	void Awake () {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
		} else {
			instance = this;
		}

		DontDestroyOnLoad(this.gameObject);
	}
	//========================================================================

	[SerializeField] Animator myAnimator;
//	[SerializeField] float myLoadingWaitTime = 5;
	private string myNextScene;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartTransition (string g_scene) {
		myNextScene = g_scene;
		TransitionOut ();
	}

	public void EndTransition () {
		TransitionIn ();
	}

	public void TransitionIn () {
		myAnimator.SetBool ("isTransitioning", false);
	}

	public void TransitionOut () {
		myAnimator.SetBool ("isTransitioning", true);
	}

	public void StartLoading () {
		SceneManager.LoadSceneAsync (myNextScene);
	}
}
