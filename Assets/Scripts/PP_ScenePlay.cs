﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JellyJoystick;

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
	[SerializeField] PP_Base[] myBases;
	private float[] myScores = { 0, 0 };
	private float[] myScoreMethod = { 0, 0, 0, 0, 0, 0 };
	[SerializeField] AudioClip mySFX_Grape;
	[SerializeField] AudioClip mySFX_Bean;
	[SerializeField] AudioClip mySFX_Cannon;
	private bool isGameStart = false;
	private bool isGameEnd = false;

	[SerializeField] GameObject[] myTrophies;

	// Use this for initialization
	void Start () {
		myTrophies[0].SetActive(false);
		myTrophies[1].SetActive(false);

		PP_MessageBox.Instance.InitPlay ();

		myBases [0].ShowScore (myScores[0]);
		myBases [1].ShowScore (myScores[1]);

		//Transition
		PP_MessageBox.Instance.Pause (true);
		PP_TransitionManager.Instance.ShowPressToStart ();

	}
	
	// Update is called once per frame
	void Update () {
		if (!isGameStart) {
//			Debug.Log ("!isGameStart");
			if (JellyJoystickManager.Instance.GetButton(ButtonMethodName.Down, 0, JoystickButton.A)) {
//				Debug.Log ("Submit");
				PP_MessageBox.Instance.Pause (false);
				PP_TransitionManager.Instance.EndTransition ();
				isGameStart = true;
			}
		}

//		if (isGameEnd) {
//			if (JellyJoystickManager.Instance.GetButton(ButtonMethodName.Down, 0, JoystickButton.START)) {
//				PP_TransitionManager.Instance.StartTransition (PP_Global.SCENE_SELECT);
//			}
//		}
	}
		
	public void AddScore (int g_team, float g_score, PP_Global.ScoreMethod g_method) {
		if (isGameEnd)
			return;
		
		myScores [g_team] += g_score;
		myScoreMethod [g_team * 2 + (int)g_method] += g_score;
//		PP_UIPlay.Instance.ShowScore (g_team, myScores [g_team]);
		myBases [g_team].ShowScore (myScores [g_team] / myWinnerScore);

		//play sound
		if (g_method == PP_Global.ScoreMethod.Grape)
			CS_AudioManager.Instance.PlaySFX (mySFX_Grape);
		else if (g_method == PP_Global.ScoreMethod.Bean)
			CS_AudioManager.Instance.PlaySFX (mySFX_Bean);
		else if (g_method == PP_Global.ScoreMethod.Cannon)
			CS_AudioManager.Instance.PlaySFX (mySFX_Cannon);

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
//			PP_UIPlay.Instance.ShowWinner (g_team);
			myTrophies[g_team].SetActive(true);
			myBases [g_team].ShowWinParticle ();
			myBases [g_team].ShowWinner ();
			myBases [1 - g_team].ShowDead ();
			isGameEnd = true;
		}
	}

	public void ShowElderNibble (int g_team) {
		myBases [g_team].ShowNibble ();
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
