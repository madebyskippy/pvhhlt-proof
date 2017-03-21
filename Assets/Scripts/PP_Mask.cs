using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Mask : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, 99);
	}
}
