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
	// Use this for initialization
	void Start () {
		for (int i = 0; i < 2; i++) {
			GameObject t_butt = Instantiate (myButtPrefab, myButtsSpawnPoint [0], Quaternion.identity) as GameObject;
			t_butt.GetComponent<PP_Butt> ().Init (i, myButtColors, myButtsSpawnPoint [i]);
			myButts.Add (t_butt);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
