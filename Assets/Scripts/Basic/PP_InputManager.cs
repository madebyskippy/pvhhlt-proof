using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_InputManager : MonoBehaviour {
	private enum Platform {
		OSX,
		WIN
	}

	private enum MapType {
		NONE,
		OSX_XBOX,
		WIN_XBOX,
		PS4,
		PS3
	}

	private Platform myPlatform;
	private MapType[] myControllersType = new MapType[8];

	void Start () {
		if (Application.platform == RuntimePlatform.OSXEditor ||
		    Application.platform == RuntimePlatform.OSXPlayer) {
			myPlatform = Platform.OSX;
		} else if (Application.platform == RuntimePlatform.WindowsEditor ||
		           Application.platform == RuntimePlatform.WindowsPlayer) {
			myPlatform = Platform.WIN;
		}
		UpdateMyControllersType ();
	}

	public void UpdateMyControllersType () {
		string[] t_names = Input.GetJoystickNames();

		int number = Mathf.Clamp (t_names.Length, 0, 8);

		for (int i = 0; i < number; i++) {
			myControllersType [i] = GetControllerType (t_names [i]);
		}
	}

	private MapType GetControllerType (string g_name){
		Debug.Log (g_name);

		if (g_name.Contains ("Microsoft") ||
		    g_name.Contains ("Xbox") ||
		    g_name.Contains ("XBOX") ||
		    g_name.Contains ("360") ||
		    g_name.Contains ("one") ||
		    g_name.Contains ("ONE")) {
			Debug.Log ("XBOX!");
			if (myPlatform == Platform.OSX)
				return MapType.OSX_XBOX;
			else if (myPlatform == Platform.WIN)
				return MapType.WIN_XBOX;
		}

		if (g_name.Contains ("3")) {
			Debug.Log ("PS3");
			return MapType.PS3;
		}

		if (g_name.Contains ("Wireless Controller")) {
			Debug.Log ("PS3");
			return MapType.PS3;
		}

		return MapType.NONE;
	}

//	public bool GetButtonUp (string g_buttonName, int g_joystickNumber) {
//		
//	}
//
//	public bool GetButton () {
//		
//	}
//
//	public bool GetButtonDown () {
//		
//	}
}
