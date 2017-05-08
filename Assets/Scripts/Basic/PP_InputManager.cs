using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_InputManager : MonoBehaviour {

	private static PP_InputManager instance = null;

	//========================================================================
	public static PP_InputManager Instance {
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
		DontDestroyOnLoad(this.gameObject);
	}
	//========================================================================
	private const int NUMBER_MAX_JOYSTICK = 8;

	public enum ButtonMethodName {
		Down,
		Hold,
		Up
	}

	public enum JoyStickButton {
		A,
		B,
		X,
		Y,
		LB,
		RB,
		BACK,
		START,
		LS,
		RS
	}

	public enum AxisMethodName {
		Normal,
		Raw
	}

	public enum JoyStickAxis {
		LS_X,
		LS_Y,
		RS_X,
		RS_Y,
		LT,
		RT,
	}

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

	private delegate bool ButtonMethod (KeyCode t_keyCode);
	private delegate float AxisMethod (string t_axis);

	private Platform myPlatform;
	private MapType[] myMapTypes = new MapType[NUMBER_MAX_JOYSTICK];

	void Start () {
		InitPlatform ();
		UpdateMyControllersType ();
	}

	private void InitPlatform () {
		if (Application.platform == RuntimePlatform.OSXEditor ||
			Application.platform == RuntimePlatform.OSXPlayer) {
			myPlatform = Platform.OSX;
		} else if (Application.platform == RuntimePlatform.WindowsEditor ||
			Application.platform == RuntimePlatform.WindowsPlayer) {
			myPlatform = Platform.WIN;
		}
	}

	void Update () {
//		if (GetButton(ButtonMethodName.Down, 1, JoyStickButton.A)){
//			Debug.Log ("A1!");
//		}
//
//		if (GetButton(ButtonMethodName.Up, 1, JoyStickButton.A)){
//			Debug.Log ("A1up!");
//		}
//
//		if (GetButton(ButtonMethodName.Down, 2, JoyStickButton.A)){
//			Debug.Log ("A2!");
//		}
//
//		if (GetButton(ButtonMethodName.Down, 0, JoyStickButton.A)){
//			Debug.Log ("A!!!!");
//		}
//
//		Debug.Log ("1LX" + GetAxis (AxisMethodName.Normal, 1, JoyStickAxis.RS_X));
//		Debug.Log ("1LY" + GetAxis (AxisMethodName.Normal, 1, JoyStickAxis.RS_Y));
//		Debug.Log ("2LX" + GetAxis (AxisMethodName.Normal, 2, JoyStickAxis.RS_X));
//		Debug.Log ("2LY" + GetAxis (AxisMethodName.Normal, 2, JoyStickAxis.RS_Y));
//		Debug.Log ("LX" + GetAxis (AxisMethodName.Normal, 2, JoyStickAxis.RS_X));
//		Debug.Log ("LY" + GetAxis (AxisMethodName.Normal, 2, JoyStickAxis.RS_Y));
	}

	public void UpdateMyControllersType () {
		string[] t_names = Input.GetJoystickNames();

		int number = Mathf.Clamp (t_names.Length, 0, NUMBER_MAX_JOYSTICK);

		for (int i = 0; i < number; i++) {
			myMapTypes [i] = GetControllerType (t_names [i]);
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
			Debug.Log ("PS4");
			return MapType.PS4;
		}

		return MapType.NONE;
	}

	public float GetAxis (AxisMethodName g_input, int g_joystickNumber, JoyStickAxis g_axis) {
		//get the input function
		AxisMethod t_InputFunction;
		if (g_input == AxisMethodName.Normal)
			t_InputFunction = Input.GetAxis;
		else
			t_InputFunction = Input.GetAxisRaw;

		//0 -> all; 1-8 -> joystick1-8 
		g_joystickNumber = Mathf.Clamp (g_joystickNumber, 0, NUMBER_MAX_JOYSTICK);

		if (g_joystickNumber != 0) {
			//get the map type
			MapType t_mapType = myMapTypes [g_joystickNumber - 1];

			if (t_mapType == MapType.PS4) {
				
				if (g_axis == JoyStickAxis.LS_X)
					return t_InputFunction ("Joystick" + g_joystickNumber + "Axis1");
				if (g_axis == JoyStickAxis.LS_Y)
					return t_InputFunction ("Joystick" + g_joystickNumber + "Axis2") * -1;
				
				if (g_axis == JoyStickAxis.RS_X)
					return t_InputFunction ("Joystick" + g_joystickNumber + "Axis3");
				if (g_axis == JoyStickAxis.RS_Y)
					return t_InputFunction ("Joystick" + g_joystickNumber + "Axis4") * -1;
				
				if (g_axis == JoyStickAxis.LT)
					return t_InputFunction ("Joystick" + g_joystickNumber + "Axis5");
				if (g_axis == JoyStickAxis.RT)
					return t_InputFunction ("Joystick" + g_joystickNumber + "Axis6");
				
			} else if (t_mapType == MapType.OSX_XBOX) {

				if (g_axis == JoyStickAxis.LS_X)
					return t_InputFunction ("Joystick" + g_joystickNumber + "Axis1");
				if (g_axis == JoyStickAxis.LS_Y)
					return t_InputFunction ("Joystick" + g_joystickNumber + "Axis2") * -1;

				if (g_axis == JoyStickAxis.RS_X)
					return t_InputFunction ("Joystick" + g_joystickNumber + "Axis3");
				if (g_axis == JoyStickAxis.RS_Y)
					return t_InputFunction ("Joystick" + g_joystickNumber + "Axis4") * -1;

				if (g_axis == JoyStickAxis.LT)
					return t_InputFunction ("Joystick" + g_joystickNumber + "Axis5");
				if (g_axis == JoyStickAxis.RT)
					return t_InputFunction ("Joystick" + g_joystickNumber + "Axis6");
				
			} else if (t_mapType == MapType.WIN_XBOX) {

				if (g_axis == JoyStickAxis.LS_X)
					return t_InputFunction ("Joystick" + g_joystickNumber + "Axis1");
				if (g_axis == JoyStickAxis.LS_Y)
					return t_InputFunction ("Joystick" + g_joystickNumber + "Axis2") * -1;

				if (g_axis == JoyStickAxis.RS_X)
					return t_InputFunction ("Joystick" + g_joystickNumber + "Axis4");
				if (g_axis == JoyStickAxis.RS_Y)
					return t_InputFunction ("Joystick" + g_joystickNumber + "Axis5") * -1;

				if (g_axis == JoyStickAxis.LT)
					return t_InputFunction ("Joystick" + g_joystickNumber + "Axis9");
				if (g_axis == JoyStickAxis.RT)
					return t_InputFunction ("Joystick" + g_joystickNumber + "Axis10");

			}
		} else {
			for (int i = 1; i <= NUMBER_MAX_JOYSTICK; i++) {
				float t_value = GetAxis (g_input, i, g_axis);
				if (t_value != 0)
					return t_value;
			}
		}
		return 0;
	}

	public bool GetButton (ButtonMethodName g_input, int g_joystickNumber, JoyStickButton g_button) {
		//get the input function
		ButtonMethod t_InputFunction;
		if (g_input == ButtonMethodName.Up)
			t_InputFunction = Input.GetKeyUp;
		else if (g_input == ButtonMethodName.Hold)
			t_InputFunction = Input.GetKey;
		else
			t_InputFunction = Input.GetKeyDown;

		//0 -> all; 1-8 -> joystick1-8 
		g_joystickNumber = Mathf.Clamp (g_joystickNumber, 0, NUMBER_MAX_JOYSTICK);

		if (g_joystickNumber != 0) {
			//get the map type
			MapType t_mapType = myMapTypes [g_joystickNumber - 1];

			if (t_mapType == MapType.PS4) {
				
				if (g_button == JoyStickButton.A)
					return t_InputFunction (GetKeyCode (1, g_joystickNumber));
				if (g_button == JoyStickButton.B)
					return t_InputFunction (GetKeyCode (2, g_joystickNumber));
				if (g_button == JoyStickButton.X)
					return t_InputFunction (GetKeyCode (0, g_joystickNumber));
				if (g_button == JoyStickButton.Y)
					return t_InputFunction (GetKeyCode (3, g_joystickNumber));
				
				if (g_button == JoyStickButton.LB)
					return t_InputFunction (GetKeyCode (4, g_joystickNumber));
				if (g_button == JoyStickButton.RB)
					return t_InputFunction (GetKeyCode (5, g_joystickNumber));
				
				if (g_button == JoyStickButton.BACK)
					return t_InputFunction (GetKeyCode (8, g_joystickNumber));
				if (g_button == JoyStickButton.START)
					return t_InputFunction (GetKeyCode (9, g_joystickNumber));
				
				if (g_button == JoyStickButton.LS)
					return t_InputFunction (GetKeyCode (10, g_joystickNumber));
				if (g_button == JoyStickButton.RS)
					return t_InputFunction (GetKeyCode (11, g_joystickNumber));
				
			} else if (t_mapType == MapType.OSX_XBOX) {
				
				if (g_button == JoyStickButton.A)
					return t_InputFunction (GetKeyCode (16, g_joystickNumber));
				if (g_button == JoyStickButton.B)
					return t_InputFunction (GetKeyCode (17, g_joystickNumber));
				if (g_button == JoyStickButton.X)
					return t_InputFunction (GetKeyCode (18, g_joystickNumber));
				if (g_button == JoyStickButton.Y)
					return t_InputFunction (GetKeyCode (19, g_joystickNumber));

				if (g_button == JoyStickButton.LB)
					return t_InputFunction (GetKeyCode (13, g_joystickNumber));
				if (g_button == JoyStickButton.RB)
					return t_InputFunction (GetKeyCode (14, g_joystickNumber));

				if (g_button == JoyStickButton.BACK)
					return t_InputFunction (GetKeyCode (10, g_joystickNumber));
				if (g_button == JoyStickButton.START)
					return t_InputFunction (GetKeyCode (9, g_joystickNumber));

				if (g_button == JoyStickButton.LS)
					return t_InputFunction (GetKeyCode (11, g_joystickNumber));
				if (g_button == JoyStickButton.RS)
					return t_InputFunction (GetKeyCode (12, g_joystickNumber));
				
			} else if (t_mapType == MapType.WIN_XBOX) {

				if (g_button == JoyStickButton.A)
					return t_InputFunction (GetKeyCode (0, g_joystickNumber));
				if (g_button == JoyStickButton.B)
					return t_InputFunction (GetKeyCode (1, g_joystickNumber));
				if (g_button == JoyStickButton.X)
					return t_InputFunction (GetKeyCode (2, g_joystickNumber));
				if (g_button == JoyStickButton.Y)
					return t_InputFunction (GetKeyCode (3, g_joystickNumber));

				if (g_button == JoyStickButton.LB)
					return t_InputFunction (GetKeyCode (4, g_joystickNumber));
				if (g_button == JoyStickButton.RB)
					return t_InputFunction (GetKeyCode (5, g_joystickNumber));

				if (g_button == JoyStickButton.BACK)
					return t_InputFunction (GetKeyCode (6, g_joystickNumber));
				if (g_button == JoyStickButton.START)
					return t_InputFunction (GetKeyCode (7, g_joystickNumber));

				if (g_button == JoyStickButton.LS)
					return t_InputFunction (GetKeyCode (8, g_joystickNumber));
				if (g_button == JoyStickButton.RS)
					return t_InputFunction (GetKeyCode (9, g_joystickNumber));
			}
		} else {
			for (int i = 1; i <= NUMBER_MAX_JOYSTICK; i++) {
				if (GetButton (g_input, i, g_button))
					return true;
			}
		}
		return false;
	}

	private KeyCode GetKeyCode (int g_buttonNumber, int g_joystickNumber) {
		return KeyCode.JoystickButton0 + g_buttonNumber + g_joystickNumber * 20;
	}
}
