using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_ScenePlay : MonoBehaviour {
	
	private static PP_ScenePlay instance = null;

	//========================================================================
	public static PP_ScenePlay Instance {
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

	[SerializeField] float myWinnerScore = 500;
	private float[] myScores = { 0, 0 };
	private bool isGameEnd = false;

	// Use this for initialization
	void Start () {
		PP_MessageBox.Instance.InitPlay ();
	}
	
	// Update is called once per frame
	void Update () {
 
	}
		
	public void AddScore (int g_team, float g_score) {
		if (isGameEnd)
			return;
		
		myScores [g_team] += g_score;
		PP_UIPlay.Instance.ShowScore (g_team, myScores [g_team]);

		CheckWinner (g_team);
	}

	private void CheckWinner (int g_team) {
		if (isGameEnd)
			return;
		
		if (myScores [g_team] > myWinnerScore) {
			PP_UIPlay.Instance.ShowWinner (g_team);
			isGameEnd = true;
		}
	}
}
