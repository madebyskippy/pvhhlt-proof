//CheesyCamera ver.1.0
//made by Hang Ruan, Kevin Zeng
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheesyCamera : MonoBehaviour {
	private enum ResizeType{
		OrthographicSize,
		ViewportRect
	}
	[SerializeField] ResizeType myType = ResizeType.ViewportRect;
	private Camera myCamera;
	[SerializeField] Vector2 myDefaultRatio = new Vector2 (16, 9);
	private Vector2 myLastScreenSize = Vector2.zero;
	private float myOrthographicSize;

	// Use this for initialization
	void Start () {
		myCamera = this.GetComponent<Camera> ();
		myOrthographicSize = this.GetComponent<Camera> ().orthographicSize;
		UpdateResize ();
		myLastScreenSize = new Vector2 (Screen.width, Screen.height);
	}

	// Update is called once per frame
	void Update () {
		if (Screen.width != myLastScreenSize.x || Screen.height != myLastScreenSize.y) {
			UpdateResize ();
			myLastScreenSize = new Vector2 (Screen.width, Screen.height);
		}
	}

	private void UpdateResize () {
		switch (myType) {
		case ResizeType.OrthographicSize: 
			UpdateOrthographicSize ();
			break;
		case ResizeType.ViewportRect: 
			UpdateViewportRect ();
			break;
		}
	}

	private void UpdateViewportRect () {
		if ((float)Screen.height / (float)Screen.width == myDefaultRatio.y / myDefaultRatio.x) {
			myCamera.rect = new Rect (0, 0, 1, 1);
		} else if ((float)Screen.height / (float)Screen.width > myDefaultRatio.y / myDefaultRatio.x) {
			float t_height = myDefaultRatio.y / myDefaultRatio.x / (float)Screen.height * (float)Screen.width;
			myCamera.rect = new Rect (0, (1 - t_height) * 0.5f, 1, t_height);
		} else {
			float t_width = myDefaultRatio.x / myDefaultRatio.y / (float)Screen.width * (float)Screen.height;
			myCamera.rect = new Rect ((1 - t_width) * 0.5f, 0, t_width, 1);
		}


	}

	private void UpdateOrthographicSize () {
		if ((float)Screen.height / (float)Screen.width > myDefaultRatio.y / myDefaultRatio.x)
			myCamera.orthographicSize = myOrthographicSize * myDefaultRatio.x / myDefaultRatio.y / (float)Screen.width * (float)Screen.height;
		else
			myCamera.orthographicSize = myOrthographicSize;
//		myCamera.orthographicSize = myOrthographicSize * myDefaultRatio.y / myDefaultRatio.x / (float)Screen.height * (float)Screen.width;
	}
}
