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

//	[SerializeField] Animator myAnimator;
	[SerializeField] Animator myGrapeAnimator;
	[SerializeField] SpriteRenderer myTutorialSpriteRenderer;
	[SerializeField] Sprite[] myTutorialSlides; 
	private int myCurrentSlide;
	[SerializeField] float myTutorialSwitchTime = 6;
	private float myTutorialTimer = -1;
//	[SerializeField] float myLoadingWaitTime = 5;
	private string myNextScene;
	private bool isStickActive = false;

	private enum Status {
		Idle,
		TransitionIn,
		TransitionOut
	}

	[SerializeField] Transform myTransition;
	[SerializeField] Vector3 myPositionStart;
	[SerializeField] Vector3 myPositionShow;
	[SerializeField] Vector3 myPositionEnd;
	[SerializeField] float myAnimationTime = 0.5f;
	private float myAnimationTimer;
	private Status myStatus = Status.Idle;

	// Use this for initialization
	void Start () {
		myTutorialSpriteRenderer.sprite = myTutorialSlides [0];
		myCurrentSlide = 0;
		myTutorialTimer = 0;
//		Debug.Log (myAnimationTimer);
	}
	
	// Update is called once per frame
	void Update () {
		UpdateTransitionIn ();
		UpdateTransitionOut ();
//		Debug.Log (myTutorialTimer);
		UpdateTutorial ();

//		Debug.Log (myAnimationTimer);
	}

	public void UpdateTutorial () {
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

	public void UpdateTransitionIn () {
		if (myStatus == Status.TransitionIn) {
			myAnimationTimer -= Time.unscaledDeltaTime;
			if (myAnimationTimer <= 0) {
				myAnimationTimer = 0;
				myStatus = Status.Idle;

				if (PP_PauseController.Instance != null) {
					PP_PauseController.Instance.SetIsMenuActive (true);
				}
			}
			//change the position
			myTransition.position = (myAnimationTimer / myAnimationTime) * (myPositionShow - myPositionEnd) + myPositionEnd;
		}
	}

	public void UpdateTransitionOut () {
//		Debug.Log (myAnimationTimer);
		if (myStatus == Status.TransitionOut) {
			myAnimationTimer -= Time.unscaledDeltaTime;
			if (myAnimationTimer <= 0) {
				myAnimationTimer = 0;
				myStatus = Status.Idle;
				StartLoading ();
			}
			//change the position
			myTransition.position = (myAnimationTimer / myAnimationTime) * (myPositionStart - myPositionShow) + myPositionShow;
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
		
		myAnimationTimer = myAnimationTime;
		myStatus = Status.TransitionIn;
		myTutorialTimer = -1;
//		myAnimator.SetBool ("isTransitioning", false);
	}

	public void TransitionOut () {
		Debug.Log ("TransitionOut");
		if (PP_PauseController.Instance != null) {
			PP_PauseController.Instance.SetIsMenuActive (false);
		}
		myAnimationTimer = myAnimationTime;
		myStatus = Status.TransitionOut;
		myTutorialTimer = 0;
		ShowNextSlide ();
//		myAnimator.SetBool ("isTransitioning", true);
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
