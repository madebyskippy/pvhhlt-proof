using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_GrapeManager : MonoBehaviour {

	[SerializeField] GameObject myGrapePrefab;

	[SerializeField] float mySpawnTime = 5;
	private float myTimer;

	// Use this for initialization
	void Start () {
		myTimer = mySpawnTime;
	}
	
	// Update is called once per frame
	void Update () {
		if (myTimer > 0) {
			myTimer -= Time.deltaTime;
			if (myTimer <= 0) {
				GameObject t_grape = Instantiate (myGrapePrefab, this.transform.position, Quaternion.identity) as GameObject;
				t_grape.GetComponent<PP_Grape> ().SetMyManager (this);
				myTimer = 0;
			}
		}
	}

	public void StartSpawnTimer () {
		myTimer = mySpawnTime;
	}
}
