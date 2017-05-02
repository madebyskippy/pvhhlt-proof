﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	private GameObject[] selectA;
	private GameObject[] selectB;
	private GameObject[] pauseA;
	private GameObject[] pauseB;
	private bool[] teamAReady;
	private bool[] teamBReady;
	private int teamACounter = 0;
	private int teamBCounter = 0;
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
		selectA = GameObject.FindGameObjectsWithTag ("SelectA");
		selectB = GameObject.FindGameObjectsWithTag ("SelectB");
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
			UpdateSelection ();
		}

		for (int i = 0; i < 3; i++) {
			string name1 = "Ready" + (i * 2 + 1);
			if (Input.GetButtonDown (name1) && Time.timeScale != 0) {
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

			string name2 = "Ready" + (i * 2 + 2);
			if (Input.GetButtonDown (name2) && Time.timeScale != 0) {
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
		pauseA [2] = pauseCore.transform.GetChild (0).GetChild (1).gameObject;
		pauseA [1] = pauseCore.transform.GetChild (0).GetChild (2).gameObject;
		pauseB [0] = pauseCore.transform.GetChild (1).GetChild (0).gameObject;
		pauseB [2] = pauseCore.transform.GetChild (1).GetChild (1).gameObject;
		pauseB [1] = pauseCore.transform.GetChild (1).GetChild (2).gameObject;
	}
		
	void GetPlayersStatus() {
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		for (int i = 0; i < players.Length; i++) {
			GameObject currentPlayer = players [i];
			int currentTeamNum = currentPlayer.GetComponent<PP_Player> ().GetMyTeamNumber();
			if (currentTeamNum == 0) {
				teamA [teamACounter++] = currentPlayer;
			} else {
				teamB [teamBCounter++] = currentPlayer;
			}
		}
	}

	public void UpdateSelection(){
		GenerateSelections (selectA, teamA);
		GenerateSelections (selectB, teamB);
		GenerateSelections (pauseA, teamA);
		GenerateSelections (pauseB, teamB);
	}

	void GenerateSelections(GameObject[] positions, GameObject[] team){
		for (int i = 0; i < 3; i++) {
			PP_Global.Abilities currentAAbility = team [i].GetComponent<PP_Player> ().GetMyAbility();
			Color color = team [i].GetComponent<PP_Player> ().GetMyColor ();
			Color colorDetail = team [i].GetComponent<PP_Player> ().GetMyColorDetail ();
			positions [i].transform.FindChild ("SelectCharacterContainer").GetComponent<PP_SelectCharacterManager> ().SwitchShow (currentAAbility);
			positions [i].transform.FindChild ("SelectCharacterContainer").GetComponent<PP_SelectCharacterManager> ().SetColors (color, colorDetail);
			positions [i].transform.GetChild (0).gameObject.GetComponent<SpriteRenderer> ().color = color;
		}
	}


	bool CheckReadys(bool[] teamReady, GameObject[] select) {
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
