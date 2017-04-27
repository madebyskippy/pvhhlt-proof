using System.Collections;
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

	[SerializeField] GameObject burpObject;
	[SerializeField] GameObject dashObject;
	[SerializeField] GameObject freezeObject;

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
			UpdateSelection (false);
		}

		for (int i = 0; i < 3; i++) {
			string name1 = "Ready" + (i * 2 + 1);
			if (Input.GetButtonDown (name1) && Time.timeScale != 0) {
				teamAReady [i] = !teamAReady[i];
				teamA [i].GetComponent<PP_Player> ().ToggleReady ();
				checkTeamAReady = CheckReadys (teamAReady, selectA);
				checkTeamBReady = CheckReadys (teamBReady, selectB);
			}

			string name2 = "Ready" + (i * 2 + 2);
			if (Input.GetButtonDown (name2) && Time.timeScale != 0) {
				teamBReady [i] = !teamBReady[i];
				teamA [i].GetComponent<PP_Player> ().ToggleReady ();
				checkTeamAReady = CheckReadys (teamAReady, selectA);
				checkTeamBReady = CheckReadys (teamBReady, selectB);
			}
		}



		if (checkTeamAReady && checkTeamBReady) {
//			UnityEngine.SceneManagement.SceneManager.LoadScene ("Play");
			PP_MessageBox.Instance.LoadScenePlay ();
			//only call load scene once
			isLoading = true;
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
//			PP_Global.Abilities currentType = currentPlayer.GetComponent<PP_Player> ().GetMyAbility();
			if (currentTeamNum == 0) {
				teamA [teamACounter++] = currentPlayer;
			} else {
//				Debug.Log (teamB.Length);
//				Debug.Log (teamBCounter);
				teamB [teamBCounter++] = currentPlayer;
			}
		}
	}

	public void UpdateSelection(bool changed){
		if (changed) {
			ClearSelections ();
		}
		GenerateSelections (selectA, teamA, teamAReady, false);
		GenerateSelections (selectB, teamB, teamBReady, false);
		GenerateSelections (pauseA, teamA, teamAReady, true);
		GenerateSelections (pauseB, teamB, teamBReady, true);
	}

	void GenerateSelections(GameObject[] positions, GameObject[] team, bool[] teamReady, bool isPause){
		for (int i = 0; i < 3; i++) {
			PP_Global.Abilities currentAAbility = team [i].GetComponent<PP_Player> ().GetMyAbility();
			GameObject currentType = burpObject;
			switch (currentAAbility) {
			case PP_Global.Abilities.Dash:
				currentType = dashObject;
				break;
			case PP_Global.Abilities.Freeze:
				currentType = freezeObject;
				break;
			}

			GameObject currentSelect = Instantiate(currentType, positions[i].transform);
			currentSelect.transform.localPosition = new Vector3 (0f, 1.6f, 0f);
			currentSelect.transform.localScale = new Vector3 (0.3f, 0.3f, 0f);
			currentSelect.GetComponent<SpriteRenderer> ().color = team [i].GetComponent<PP_Player> ().GetMyColor();
//			positions[i].GetComponent<SpriteRenderer>().color = team [i].GetComponent<PP_Player> ().GetMyColor();
			positions[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = team [i].GetComponent<PP_Player> ().GetMyColor();
			currentSelect.transform.GetChild (0).GetComponent<SpriteRenderer> ().color = team [i].GetComponent<PP_Player> ().GetMyColorDetail ();

			if (isPause) {
				SpriteRenderer selfSprites = currentSelect.GetComponent<SpriteRenderer> ();
				selfSprites.sortingOrder = 3;
				selfSprites.sortingLayerName = "UI";
				for (int j = 0; j < currentSelect.transform.childCount; j++) {
					SpriteRenderer childSprites = currentSelect.transform.GetChild(j).GetComponent<SpriteRenderer> ();
					childSprites.sortingOrder = 3 + j;
					childSprites.sortingLayerName = "UI";
				}
			}
		}
	}

	void ClearSelections() {
		for (int i = 0; i < selectA.Length; i++) {
			GameObject typeObj = selectA [i].transform.GetChild (2).gameObject;
			Destroy (typeObj);
			GameObject uiObj = pauseA [i].transform.GetChild (1).gameObject;
			Destroy (uiObj);
		}

		for (int i = 0; i < selectB.Length; i++) {
			GameObject typeObj = selectB [i].transform.GetChild (2).gameObject;
			Destroy (typeObj);
			GameObject uiObj = pauseB [i].transform.GetChild (1).gameObject;
			Destroy (uiObj);
		}
	}

	bool CheckReadys(bool[] teamReady, GameObject[] select) {
		bool ready = true;
		for (int i = 0; i < teamReady.Length; i++) {
//			GameObject prompText = select [i].transform.FindChild ("ready_promp_text").gameObject;
			GameObject readyText = select [i].transform.FindChild ("ready_text").gameObject;
//			Color tmpPrompText = prompText.GetComponent<SpriteRenderer>().color;
			Color tmpReadyText = readyText.GetComponent<SpriteRenderer> ().color;
			if (teamReady [i]) {
//				tmpPrompText.a = 0f;
				tmpReadyText.a = 1f;
//				prompText.GetComponent<SpriteRenderer> ().color = tmpPrompText;
				readyText.GetComponent<SpriteRenderer> ().color = tmpReadyText;
			} else {
//				tmpPrompText.a = 1f;
				tmpReadyText.a = 0f;
//				prompText.GetComponent<SpriteRenderer> ().color = tmpPrompText;
				readyText.GetComponent<SpriteRenderer> ().color = tmpReadyText;
				ready = false;
			}
		}

		return ready;
	}
}
