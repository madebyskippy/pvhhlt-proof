using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Head : MonoBehaviour {
	private CS_Player myPlayer;
	private int myTeamNumber;

	public void SetMyPlayer (CS_Player g_myPlayer) {
		myPlayer = g_myPlayer;
		myTeamNumber = g_myPlayer.GetMyTeamNumber ();
	}

	public void Pushed (int g_teamNumber, Vector2 g_direction) {
		myPlayer.HeadPushed (g_teamNumber, g_direction);
	}

	public int GetMyTeamNumber () {
		return myTeamNumber;
	}
}
