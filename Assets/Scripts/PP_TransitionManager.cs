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
	[SerializeField] Animator myGrapeAnimator;
	[SerializeField] SpriteRenderer myTutorialSpriteRenderer;
	[SerializeField] Sprite[] myTutorialSlides; 
	private int myCurrentSlide;
	[SerializeField] float myTutorialSwitchTime = 6;
	private float myTutorialTimer = -1;
//	[SerializeField] float myLoadingWaitTime = 5;
	private string myNextScene;
	private bool isStickActive = false;

	// Use this for initialization
	void Start () {
		myTutorialSpriteRenderer.sprite = myTutorialSlides [0];
		myCurrentSlide = 0;
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log (myTutorialTimer);
		if (myTutorialTimer < 0)
			return;
		
		myTutorialTimer += Time.unscaledDeltaTime;

		if (myTutorialTimer >= myTutorialSwitchTime) {
			myTutorialTimer = 0;
			ShowNextSlide ();
			Debug.Log ("Timer");
		}

		if (Input.GetAxisRaw ("Vertical") == 0) {
			isStickActive = false;
		}

		if (Input.GetAxisRaw("Vertical") > 0 && !isStickActive) {
			ShowNextSlide ();
			Debug.Log ("Vertical");
			myTutorialTimer = 0;
			isStickActive = true;
		}

		if (Input.GetAxisRaw("Vertical") < 0 && !isStickActive) {
			ShowNextSlide ();
			Debug.Log ("Vertical");
			myTutorialTimer = 0;
			isStickActive = true;
		}
	}

	public void StartTransition (string g_scene) {
		myNextScene = g_scene;
		myGrapeAnimator.SetBool ("isGrape", true);
		TransitionOut ();
	}

	public void EndTransition () {
		TransitionIn ();
	}

	public void TransitionIn () {
		myTutorialTimer = -1;
		myAnimator.SetBool ("isTransitioning", false);
	}

	public void TransitionOut () {
		myTutorialTimer = 0;
		ShowNextSlide ();
		Debug.Log ("TransitionOut");
		myAnimator.SetBool ("isTransitioning", true);
	}

	public void StartLoading () {
		SceneManager.LoadSceneAsync (myNextScene);
	}

	public void ShowPressToStart () {
		myGrapeAnimator.SetBool ("isGrape", false);
	}

	private void ShowNextSlide () {
		Debug.Log ("ShowNextSlide");
		myCurrentSlide++;
		myCurrentSlide %= myTutorialSlides.Length;
		myTutorialSpriteRenderer.sprite = myTutorialSlides [myCurrentSlide];
	}
}
