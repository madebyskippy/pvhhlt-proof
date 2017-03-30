using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PP_UIPlay : MonoBehaviour {
	
	private static PP_UIPlay instance = null;

	//========================================================================
	public static PP_UIPlay Instance {
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

	[SerializeField] Text[] myScoreDisplays;
	[SerializeField] GameObject myEndDisplay;
	[SerializeField] GameObject[] myWinDisplays;

	void Start () {
		foreach (Text t_score in myScoreDisplays) {
			t_score.text = "0";
		}
	}

	public void ShowScore (int g_team, float g_score) {
		myScoreDisplays [g_team].text = g_score.ToString ("0");
	}

//	public void ShowScore (int g_team, int g_type, float g_score) {
//		Debug.Log ("ShowScore");
//		myScoreDisplays [g_team * 3 + g_type].text = g_score.ToString ("0");
//	}

	public void ShowWinner (int g_team) {
		myEndDisplay.SetActive (true);
		myWinDisplays [g_team].SetActive (true);
	}
}
