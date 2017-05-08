using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PP_Global {

	public enum Abilities {
		Burp,
		Dash,  
		Freeze
	};

	public enum BeanStatus {
		Idle,
		Running,
		Frozen
	};

	public enum ScoreMethod : int {
		Grape = 0,
		Bean,
		Cannon
	};

	public const string TAG_BUTT = "Butt";
	public const string TAG_GRAPE = "Grape";
	public const string TAG_BEAN = "Bean";
	public const string TAG_PLAYER = "Player";

	public const string SCENE_PLAY = "Play";
	public const string SCENE_SELECT = "Select";
	public const string SCENE_TUTORIAL = "Tutorial";
}
