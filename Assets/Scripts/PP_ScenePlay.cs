﻿using System.Collections;
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
	[SerializeField] Transform[] myScoresDisplay;
	private float[] myScores = { 0, 0 };
	private bool isGameEnd = false;

	// Use this for initialization
	void Start () {
		PP_MessageBox.Instance.InitPlay ();

		myScoresDisplay [0].localScale = new Vector2 (1, 0);
		myScoresDisplay [1].localScale = new Vector2 (1, 0);
	}
	
	// Update is called once per frame
	void Update () {
 
	}
		
	public void AddScore (int g_team, float g_score) {
		if (isGameEnd)
			return;
		
		myScores [g_team] += g_score;
		PP_UIPlay.Instance.ShowScore (g_team, myScores [g_team]);
		myScoresDisplay [g_team].localScale = new Vector2 (1, myScores [g_team] / myWinnerScore);

		CheckWinner (g_team);
	}

//	public void AddScore (int g_team, int g_type, float g_score) {
//		if (isGameEnd)
//			return;
//
//		myScores [g_team * 3 + g_type] += g_score;
//		PP_UIPlay.Instance.ShowScore (g_team, g_type, myScores [g_team * 3 + g_type]);
////		myScoresDisplay [g_team].localScale = new Vector2 (1, myScores [g_team] / myWinnerScore);
//
//		CheckWinner (g_team, g_type);
//	}

	private void CheckWinner (int g_team) {
		if (isGameEnd)
			return;

		if (myScores [g_team] >= myWinnerScore) {
			PP_UIPlay.Instance.ShowWinner (g_team);
			isGameEnd = true;
		}
	}

//	private void CheckWinner (int g_team, int g_type) {
//		if (isGameEnd)
//			return;
//		
//		if (myScores [g_team * 3 + g_type] > 300) {
//			PP_UIPlay.Instance.ShowWinner (g_team);
//			isGameEnd = true;
//		}
//	}
}
