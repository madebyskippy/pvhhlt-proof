using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_KillMyself : MonoBehaviour {
	[SerializeField] bool isTimeTrigger = false;
	[SerializeField] float myLifeTime = 5;
	[SerializeField] GameObject myLeftover;
	private float myTimer;

	// Use this for initialization
	void Start () {
		if (isTimeTrigger) {
			myTimer = myLifeTime;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isTimeTrigger) {
			myTimer -= Time.deltaTime;
			if (myTimer <= 0) {
				KillMyself ();
			}
		}
	}

	public void KillMyself () {
		Destroy (this.gameObject);
		if (myLeftover != null)
			Instantiate (myLeftover, this.transform.position, Quaternion.identity);
	}
}
