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
	private int teamACounter = 0;
	private int teamBCounter = 0;
	private bool firstGenerate = false;

	[SerializeField] GameObject burpObject;
	[SerializeField] GameObject dashObject;
	[SerializeField] GameObject freezeObject;

	// Use this for initialization
	void Start () {
		teamA = new GameObject [3];
		teamB = new GameObject [3];
		selectA = GameObject.FindGameObjectsWithTag ("SelectA");
		selectB = GameObject.FindGameObjectsWithTag ("SelectB");
		PP_MessageBox.Instance.InitPlay ();
		GetPlayersStatus ();
	}
	
	// Update is called once per frame
	void Update () {
		//use timer to make sure the abilities updated
		if (Time.time > 0.005 && !firstGenerate) {
			firstGenerate = true;
			UpdateSelection (false);
		}
	}
		
	void GetPlayersStatus() {
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		for (int i = 0; i < players.Length; i++) {
			GameObject currentPlayer = players [i];
			int currentTeamNum = currentPlayer.GetComponent<PP_Player> ().GetMyTeamNumber();
			PP_Global.Abilities currentType = currentPlayer.GetComponent<PP_Player> ().GetMyAbility();
			if (currentTeamNum == 0) {
				teamA [teamACounter++] = currentPlayer;
			} else {
				teamB [teamBCounter++] = currentPlayer;
			}
		}
	}

	public void UpdateSelection(bool changed){
		if (changed) {
			ClearSelections ();
		}
		GenerateSelections (selectA, teamA);
		GenerateSelections (selectB, teamB);
	}

	void GenerateSelections(GameObject[] positions, GameObject[] team){
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
			currentSelect.transform.localPosition = new Vector3 (0f, -0.5f, 0f);
			currentSelect.transform.localScale = new Vector3 (1.5f, 1.5f, 0f);
			currentSelect.GetComponent<SpriteRenderer> ().color = team [i].GetComponent<PP_Player> ().GetMyColor();
			currentSelect.transform.GetChild (0).GetComponent<SpriteRenderer> ().color = team [i].GetComponent<PP_Player> ().GetMyColorDetail ();

		}
	}

	void ClearSelections() {
		for (int i = 0; i < selectA.Length; i++) {
			GameObject typeObj = selectA [i].transform.GetChild (selectA.Length).gameObject;
			Destroy (typeObj);
		}

		for (int i = 0; i < selectB.Length; i++) {
			GameObject typeObj = selectB [i].transform.GetChild (selectA.Length).gameObject;
			Destroy (typeObj);
		}
	}
}
