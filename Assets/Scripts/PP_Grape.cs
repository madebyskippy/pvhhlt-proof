using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Grape : MonoBehaviour {

	[SerializeField] GameObject myEffect;
	private PP_GrapeManager myManager;


	public void SetMyManager (PP_GrapeManager g_manager) {
		myManager = g_manager;
	}

	public void Kill () {
		if (myEffect != null) {
			Instantiate (myEffect, this.transform.position, Quaternion.identity);
		}
		myManager.StartSpawnTimer ();
		this.gameObject.SetActive (false);
	}
}
