using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PP_ColorSet {
	public Color myColorBorder;
	public Color myColorButt;

	public PP_ColorSetPlayer[] myPlayers;

}

[System.Serializable]
public class PP_ColorSetPlayer {
	public Color[] myColors;
}
