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

		//		DontDestroyOnLoad(this.gameObject);
	}
	//========================================================================

	[SerializeField] GameObject myButtPrefab;
	private List<GameObject> myButts = new List<GameObject> ();
	[SerializeField] Vector2[] myButtsSpawnPoint = { new Vector2 (-1, 0), new Vector2 (1, 0) };
	[Tooltip("6 colors, 0-2 Team1PlayerColors, 3-5 Team2PlayerColors")]
	[SerializeField] Color[] myPlayerColors;
	[Tooltip("4 colors, 0 Team1ForwardColor, 1 Team2ForwardColor, 2 Team1BackColor, 3 Team2BackColor")]
	[SerializeField] Color[] myButtColors; 
	[Tooltip("2 colors, 0 Team1BorderColor, 1 Team2BorderColor")]
	[SerializeField] Color[] myBorderColors; //needs 2 colors

	private float[] myScores = { 0, 0 };
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void InitPlay () {
		for (int i = 0; i < 2; i++) {
			GameObject t_butt = Instantiate (myButtPrefab, myButtsSpawnPoint [0], Quaternion.identity) as GameObject;
			t_butt.GetComponent<PP_Butt> ().Init (i, myButtsSpawnPoint [i], myPlayerColors, myButtColors, myBorderColors);
			myButts.Add (t_butt);
		}
	}

	public void SetScore (float[] g_scores) {
		myScores = g_scores;
	}
}
