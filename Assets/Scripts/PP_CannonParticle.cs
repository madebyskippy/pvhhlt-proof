using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_CannonParticle : MonoBehaviour {
	[SerializeField] float myLifeTime = 5;
	private float myTimer;
	// Use this for initialization
	void Awake () {
		this.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (myTimer > 0) {
			myTimer -= Time.deltaTime;
			if (myTimer <= 0) {
				this.gameObject.SetActive (false);
			}
		}
	}

	public void Init (Vector3 t_position) {
		myTimer = myLifeTime;
		this.gameObject.transform.position = t_position;
		this.gameObject.SetActive (true);
	}
}
