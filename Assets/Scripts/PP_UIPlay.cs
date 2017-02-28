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

	void Start () {
		foreach (Text t_score in myScoreDisplays) {
			t_score.text = "0";
		}
	}

	public void ShowScore (int g_team, float g_score) {
		myScoreDisplays [g_team].text = g_score.ToString ("0");
	}
}
