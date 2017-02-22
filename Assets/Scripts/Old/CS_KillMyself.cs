using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_KillMyself : MonoBehaviour {
	[SerializeField] bool isOnTime = false;
	[SerializeField] float myDeathTime = 5;
	// Use this for initialization
	void Start () {
		if (isOnTime)
			Destroy (this.gameObject, myDeathTime);
	}

	public void Suicide () {
		Destroy (this.gameObject);
	}
}
