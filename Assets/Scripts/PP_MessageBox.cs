using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_MessageBox : MonoBehaviour {

	private static PP_MessageBox instance = null;

	//========================================================================
	public static PP_MessageBox Instance {
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

	[SerializeField] GameObject myButtPrefab;
	private List<GameObject> myButts = new List<GameObject> ();
	[SerializeField] Vector2[] myButtsSpawnPoint = { new Vector2 (-1, 0), new Vector2 (1, 0) };
//	[Tooltip("6 colors, 0-2 Team1PlayerColors, 3-5 Team2PlayerColors")]
	public PP_ColorSet[] myColors; 

	private float[] myScores = { 0, 0 };
	private PP_Global.Abilities[] myPlayerAbilities = {
		PP_Global.Abilities.Burp, PP_Global.Abilities.Dash, PP_Global.Abilities.Freeze,
		PP_Global.Abilities.Burp, PP_Global.Abilities.Dash, PP_Global.Abilities.Freeze
	};

	private bool isPaused;

	private string mySceneSelect = "Select";
	private string myScenePlay = "Play";

//	// Use this for initialization
//	void Start () {
//		
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}

	public void InitPlay () {
		for (int i = 0; i < 2; i++) {
			GameObject t_butt = Instantiate (myButtPrefab, myButtsSpawnPoint [0], Quaternion.identity) as GameObject;
//			Debug.Log (t_butt.transform.position);
			t_butt.GetComponent<PP_Butt> ().Init (i, myButtsSpawnPoint [i], myColors [i]);
			myButts.Add (t_butt);
		}
	}

	public void SetScores (float[] g_scores) {
		myScores = g_scores;
	}

	public float[] GetScores () {
		return myScores;
	}

	public void SavePlayerAbility (string g_control, PP_Global.Abilities g_ability) {
		myPlayerAbilities [int.Parse (g_control) - 1] = g_ability;
	}

	public PP_Global.Abilities GetPlayerAbility (string g_control) {
		return myPlayerAbilities [int.Parse (g_control) - 1];
	}

	public void Pause (bool g_status) {
		isPaused = g_status;
		if (isPaused) {
			Time.timeScale = 0;
		} else {
			Time.timeScale = 1;
		}
	}

	public bool GetIsPaused () {
		return isPaused;
	}

	public void SetScenePlay (string g_name) {
		myScenePlay = g_name;
	}

	public void SetSceneSelect (string g_name) {
		mySceneSelect = g_name;
	}

	public void LoadScenePlay () {
		UnityEngine.SceneManagement.SceneManager.LoadScene (myScenePlay);
	}

	public void LoadSceneSelect () {
		UnityEngine.SceneManagement.SceneManager.LoadScene (mySceneSelect);
	}
}
