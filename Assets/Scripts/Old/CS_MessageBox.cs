using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CS_MessageBox : MonoBehaviour {

	private static CS_MessageBox instance = null;

	//========================================================================
	public static CS_MessageBox Instance {
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


	[SerializeField] GameObject myPlayerPrefab;
	[SerializeField] int myTeamSize;
	[SerializeField] Vector2 myTeam1SpawnPoint;
	[SerializeField] Vector2 myTeam2SpawnPoint;
	[SerializeField] float mySpawnRadius = 1;
	[SerializeField] Color[] myTeam1Colors;
	[SerializeField] Color[] myTeam2Colors;
	private List<GameObject> myTeam1 = new List<GameObject> ();
	private List<GameObject> myTeam2 = new List<GameObject> ();

	private int[] myCurrentTeamsMaps = { 0, 0, 0, 0 }; //0:T1L, 1:T1R, 2:T2L, 3:T2R
	private int[,] myControllerMaps = {
		{ 1, 2, 3 }, { 1, 3, 2 }, 
		{ 2, 1, 3 }, { 2, 3, 1 },
		{ 3, 1, 2 }, { 3, 2, 1 }
	};

	[SerializeField] GameObject myTimer;
	[SerializeField] float myReMapTime = 10;
	private float myReMapTimer = 0;
	[SerializeField] GameObject myReMapHint;

	[SerializeField] float myScoreScale = 0.5f;
	[SerializeField] TextMesh myTextTeam1;
	[SerializeField] TextMesh myTextTeam2;
	private float myScoreTeam1 = 0;
	private float myScoreTeam2 = 0;

	void Start () {
//		Debug.Log (myControllerMaps.GetLength (0));
//		Debug.Log ("Start MessageBox");
		for (int i = 0; i < myTeamSize; i++) {
			GameObject t_player = 
				Instantiate (myPlayerPrefab, myTeam1SpawnPoint + Random.insideUnitCircle * mySpawnRadius, Quaternion.identity) as GameObject;
			myTeam1.Add (t_player);
			int t_numberInt = i + 1;
			string t_number = t_numberInt.ToString ();
			t_player.name = "Player" + t_number;
			t_player.GetComponent<CS_Player> ().Init (1, myTeam1Colors[i], t_number, t_number);
		}
		for (int i = 0; i < myTeamSize; i++) {
			GameObject t_player = 
				Instantiate (myPlayerPrefab, myTeam2SpawnPoint + Random.insideUnitCircle * mySpawnRadius, Quaternion.identity) as GameObject;
			myTeam2.Add (t_player);
			int t_numberInt = i + myTeamSize + 1;
			string t_number = t_numberInt.ToString ();
			t_player.name = "Player" + t_number;
			t_player.GetComponent<CS_Player> ().Init (2, myTeam2Colors[i], t_number, t_number);
		}

//		ReMap ();
	}

	void Update () {
		if (Input.GetButtonDown ("Restart")) {
			SceneManager.LoadScene ("Play");
		}

		if (myReMapTimer <= 0) {
			ReMap ();
			Instantiate (myReMapHint);
			myReMapTimer += myReMapTime;
		}

		myReMapTimer -= Time.deltaTime;
		myTimer.transform.localScale = Vector3.right * myReMapTimer / myReMapTime + Vector3.up;
	}

	private void ReMap () {
		int t_num;
		for (int j = 0; j < myCurrentTeamsMaps.Length; j++) {
			for (int i = 0; i < 100; i++) {
				t_num = Random.Range (0, myControllerMaps.GetLength (0));
				if (t_num != myCurrentTeamsMaps [j]) {
					myCurrentTeamsMaps [j] = t_num;
					break;
				}
			}
		}

		for (int z = 0; z < myTeam1.Count; z++) {
			myTeam1 [z].GetComponent<CS_Player> ().SetMyControllerLeft (myControllerMaps [myCurrentTeamsMaps [0], z].ToString ());
		}

		for (int z = 0; z < myTeam1.Count; z++) {
			myTeam1 [z].GetComponent<CS_Player> ().SetMyControllerRight (myControllerMaps [myCurrentTeamsMaps [1], z].ToString ());
		}

		for (int z = 0; z < myTeam2.Count; z++) {
			myTeam2 [z].GetComponent<CS_Player> ().SetMyControllerLeft ((myControllerMaps [myCurrentTeamsMaps [2], z] + 3).ToString ());
		}

		for (int z = 0; z < myTeam2.Count; z++) {
			myTeam2 [z].GetComponent<CS_Player> ().SetMyControllerRight ((myControllerMaps [myCurrentTeamsMaps [3], z] + 3).ToString ());
		}

	}

	public void AddScore (int g_teamNumber) {
		if (g_teamNumber == 1) {
			myScoreTeam1 += Time.deltaTime * myScoreScale;
			myTextTeam1.text = myScoreTeam1.ToString ("0.0");
		} else if (g_teamNumber == 2) {
			myScoreTeam2 += Time.deltaTime * myScoreScale;
			myTextTeam2.text = myScoreTeam2.ToString ("0.0");
		}
	}
}
