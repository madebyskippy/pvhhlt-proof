using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_PauseController : MonoBehaviour {
	private static PP_PauseController instance = null;

	//========================================================================
	public static PP_PauseController Instance {
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

	[SerializeField] Sprite[] sprites;

	private bool resumeChoose;
	private bool exitChoose;
	private GameObject resumeBtn;
	private GameObject exitBtn;

	private bool isStickActive = false;

	// Use this for initialization
	void Start () {
		resumeBtn = this.transform.GetChild (0).GetChild (2).gameObject;
		exitBtn = this.transform.GetChild (0).GetChild (3).gameObject;
		resumeChoose = true;
		exitChoose = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Menu")) {
			Debug.Log ("change the pause key");
			toggleMenuShowHide ();
		}

		if (Input.GetAxisRaw ("Vertical") == 0) {
			isStickActive = false;
		}

		if (Time.timeScale == 0 && Input.GetAxisRaw("Vertical") > 0 && !isStickActive) {
			Debug.Log ("change the menu select key");
			isStickActive = true;
			toggleMenuSelect ();
		}

		if (Time.timeScale == 0 && Input.GetAxisRaw("Vertical") < 0 && !isStickActive) {
			Debug.Log ("change the menu select key");
			isStickActive = true;
			toggleMenuSelect ();
		}

		if (Time.timeScale == 0 && Input.GetButtonDown("Submit")) {
			Debug.Log ("change the menu Confirm key");
			if (exitChoose) {
//				Debug.Log ("do the return to menu function");
				PP_MessageBox.Instance.LoadSceneSelect ();
			}

			toggleMenuShowHide ();
		}
	}

	void toggleMenuSelect() {
		resumeChoose = !resumeChoose;
		exitChoose = !exitChoose;

		if (resumeChoose) {
			resumeBtn.GetComponent<SpriteRenderer> ().sprite = sprites [0];
		} else {
			resumeBtn.GetComponent<SpriteRenderer> ().sprite = sprites [1];
		}

		if (exitChoose) {
			exitBtn.GetComponent<SpriteRenderer> ().sprite = sprites [2];
		} else {
			exitBtn.GetComponent<SpriteRenderer> ().sprite = sprites [3];
		}
	}

	void toggleMenuShowHide() {
		GameObject messageBox = GameObject.Find ("MessageBox");
		bool isPaused = messageBox.GetComponent<PP_MessageBox> ().GetIsPaused();
		messageBox.GetComponent<PP_MessageBox> ().Pause(!isPaused);
		this.transform.GetChild(0).gameObject.SetActive (!isPaused);
	}
}
