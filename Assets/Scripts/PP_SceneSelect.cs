using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JellyJoystick;

public class PP_SceneSelect : MonoBehaviour {
	
	private static PP_SceneSelect instance = null;

	//========================================================================
	public static PP_SceneSelect Instance {
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

//		DontDestroyOnLoad(this.gameObject);
	}
	//========================================================================

	private GameObject[] teamA;
	private GameObject[] teamB;
	[SerializeField] private GameObject[] selectA;
	[SerializeField] private GameObject[] selectB;
	private GameObject[] pauseA;
	private GameObject[] pauseB;
	private bool[] teamAReady;
	private bool[] teamBReady;
	private bool firstGenerate = false;
	private bool checkTeamAReady;
	private bool checkTeamBReady;
	private GameObject pauseCore;



	//only call load scene once
	private bool isLoading = false;

	// Use this for initialization
	void Start () {
		teamA = new GameObject [3];
		teamB = new GameObject [3];
		pauseA = new GameObject [3];
		pauseB = new GameObject [3];
		teamAReady = new bool[3] { false, false, false };
		teamBReady = new bool[3] { false, false, false };
//		selectA = GameObject.FindGameObjectsWithTag ("SelectA");
//		selectB = GameObject.FindGameObjectsWithTag ("SelectB");
		GetPauseInfo ();
		PP_MessageBox.Instance.InitPlay ();
		GetPlayersStatus ();
		CheckReadys (teamAReady, selectA);
		CheckReadys (teamBReady, selectB);

		PP_TransitionManager.Instance.EndTransition ();
	}
	
	// Update is called once per frame
	void Update () {
		//only call load scene once
		if (isLoading)
			return;

		//use timer to make sure the abilities updated
		if (Time.time > 0.005 && !firstGenerate) {
			firstGenerate = true;
			UpdateSelection (true);
		}

		for (int i = 0; i < 3; i++) {
			int t_controller_1 = i * 2 + 1;
			if (JellyJoystickManager.Instance.GetButton(ButtonMethodName.Down, t_controller_1, JoystickButton.Y) && 
				Time.timeScale != 0) {
				teamAReady [i] = !teamAReady[i];
				teamA [i].GetComponent<PP_Player> ().ToggleReady ();
				checkTeamAReady = CheckReadys (teamAReady, selectA);
				checkTeamBReady = CheckReadys (teamBReady, selectB);

				if (checkTeamAReady && checkTeamBReady) {
					PP_MessageBox.Instance.LoadScenePlay ();
					//only call load scene once
					isLoading = true;
				}
			}

			int t_controller_2 = i * 2 + 2;
			if (JellyJoystickManager.Instance.GetButton(ButtonMethodName.Down, t_controller_2, JoystickButton.Y) && 
				Time.timeScale != 0) {
				teamBReady [i] = !teamBReady[i];
				teamB [i].GetComponent<PP_Player> ().ToggleReady ();
				checkTeamAReady = CheckReadys (teamAReady, selectA);
				checkTeamBReady = CheckReadys (teamBReady, selectB);

				if (checkTeamAReady && checkTeamBReady) {
					PP_MessageBox.Instance.LoadScenePlay ();
					//only call load scene once
					isLoading = true;
				}
			}
		}

	}

	void GetPauseInfo() {
		pauseCore = GameObject.Find ("PauseScreen").transform.GetChild (0).gameObject;
		pauseA [0] = pauseCore.transform.GetChild (0).GetChild (0).gameObject;
		pauseA [1] = pauseCore.transform.GetChild (0).GetChild (1).gameObject;
		pauseA [2] = pauseCore.transform.GetChild (0).GetChild (2).gameObject;
		pauseB [0] = pauseCore.transform.GetChild (1).GetChild (0).gameObject;
		pauseB [1] = pauseCore.transform.GetChild (1).GetChild (1).gameObject;
		pauseB [2] = pauseCore.transform.GetChild (1).GetChild (2).gameObject;
	}
		
	void GetPlayersStatus() {
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		for (int i = 0; i < players.Length; i++) {
			GameObject currentPlayer = players [i];
			int currentTeamNum = currentPlayer.GetComponent<PP_Player> ().GetMyTeamNumber();
			int currentIdx = currentPlayer.GetComponent<PP_Player> ().GetMyControl ();
			if (currentTeamNum == 0) {
				teamA [(currentIdx + 1)/2 - 1] = currentPlayer;
			} else {
				teamB [currentIdx/2 - 1] = currentPlayer;
			}
		}
	}

	public void UpdateSelection(bool isFirstTime){
		GenerateSelections (selectA, teamA, isFirstTime);
		GenerateSelections (selectB, teamB, isFirstTime);
		GenerateSelections (pauseA, teamA, isFirstTime);
		GenerateSelections (pauseB, teamB, isFirstTime);
	}

	void GenerateSelections(GameObject[] positions, GameObject[] team, bool isFirstTime){
		for (int i = 0; i < 3; i++) {
			PP_Global.Abilities currentAAbility = team [i].GetComponent<PP_Player> ().GetMyAbility();
			positions [i].transform.FindChild ("SelectCharacterContainer").GetComponent<PP_SelectCharacterManager> ().SwitchShow (currentAAbility);

			if (isFirstTime) {
				Color color = team [i].GetComponent<PP_Player> ().GetMyColor ();
				Color colorDetail = team [i].GetComponent<PP_Player> ().GetMyColorDetail ();
				positions [i].transform.FindChild ("SelectCharacterContainer").GetComponent<PP_SelectCharacterManager> ().SetColors (color, colorDetail);
				positions [i].transform.GetChild (0).gameObject.GetComponent<SpriteRenderer> ().color = color;
				if (positions [i].transform.GetChild (1).gameObject.GetComponent<SpriteRenderer> ()) {
					//bug fixed? 0513
					Color oldColor = positions [i].transform.GetChild (1).gameObject.GetComponent<SpriteRenderer> ().color;
					Color nowColor = color;
					nowColor.a = oldColor.a;
					positions [i].transform.GetChild (1).gameObject.GetComponent<SpriteRenderer> ().color = nowColor;
				}
			}
		}
	}


	bool CheckReadys(bool[] teamReady, GameObject[] select) {
		Debug.Log ("CheckReadys");
		bool ready = true;
		for (int i = 0; i < teamReady.Length; i++) {
			GameObject readyText = select [i].transform.FindChild ("ready_text").gameObject;
			Color tmpReadyText = readyText.GetComponent<SpriteRenderer> ().color;
			if (teamReady [i]) {
				tmpReadyText.a = 1f;
				readyText.GetComponent<SpriteRenderer> ().color = tmpReadyText;
			} else {
				tmpReadyText.a = 0f;
				readyText.GetComponent<SpriteRenderer> ().color = tmpReadyText;
				ready = false;
			}
		}

		return ready;
	}
}
