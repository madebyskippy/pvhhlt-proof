using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_CannonBall : MonoBehaviour {

	private int myTeamNumber = -1;
	private Vector3 myTargetPosition;
	private Vector3 myInitialPosition;
	private float myAngle;

	private float myPosition;

	//using the equation y = a(x-h)^2 + k were (h,k is the top of the parabola)
	private float a;
	private float h;
	private float k;

	[SerializeField] float mySpeed = 1;

	// Use this for initialization
	void Start () {
		
	}

	public void Init (int g_teamNumber, Vector3 g_InitialPosition, Vector3 g_TargetPosition, float g_angle) {
		myTeamNumber = g_teamNumber;

		myAngle = g_angle;
		myInitialPosition = g_InitialPosition;
		myTargetPosition = g_TargetPosition;

		float x0 = myInitialPosition.x;
		float y0 = myInitialPosition.y;
		float xb = myTargetPosition.x;
		float yb = myTargetPosition.y;

		a = Mathf.Abs ((0.5f) * Mathf.Tan (Mathf.Deg2Rad * (myAngle - 90)));
		h = (x0 + xb) / 2f - (y0 - yb) / (2f * a * (x0 - xb));
		k = yb - a * (xb - h) * (xb - h);
		Debug.Log (myAngle + ", a:" + a + ", tan:" + Mathf.Tan (Mathf.Deg2Rad * myAngle) + ", h:" + h + ", k:" + k);
		Debug.Log ("x0: " + x0 + " y0: " + y0 + ",   xb: " + xb + " yb: " + yb);
	}
	
	// Update is called once per frame
	void Update () {
		float t_x = transform.position.x + Time.deltaTime * mySpeed * (myTeamNumber * 2 - 1);
		float t_y = a * (t_x - h) * (t_x - h) + k;
//		float t_y = 2 * a * t_x;

		//TEMPORARY FIX
		t_y = (myTargetPosition.y - myInitialPosition.y) / (myTargetPosition.x - myInitialPosition.x ) * t_x;

		transform.position = new Vector3 (t_x, t_y, transform.position.z);
		if (Mathf.Abs (this.transform.position.x) - Mathf.Abs (myTargetPosition.x) > 0) {
			PP_ScenePlay.Instance.AddScore (myTeamNumber, 1);
			Destroy (this.gameObject);
		}
	}
}
