using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Bean : MonoBehaviour {

	[SerializeField] GameObject myEffect;
	private PP_BeanManager myManager;


	public void SetMyManager (PP_BeanManager g_manager) {
		myManager = g_manager;
	}

	public void Kill () {
		if (myEffect != null) {
			Instantiate (myEffect, this.transform.position, Quaternion.identity);
		}
		if (myManager != null) {
			myManager.RemoveBean (this.gameObject);
		}
		Destroy (this.gameObject);
	}
}
