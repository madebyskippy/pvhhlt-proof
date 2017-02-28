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
	[SerializeField] Color[] myButtColors;
	[SerializeField] Color[] myMiddleColors; //needs 4 colors, 12 forwardcolor, 34 backgroundcolor

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
			t_butt.GetComponent<PP_Butt> ().Init (i, myButtColors, myButtsSpawnPoint [i], myMiddleColors);
			myButts.Add (t_butt);
		}
	}

	public void SetScore (float[] g_scores) {
		myScores = g_scores;
	}
}
