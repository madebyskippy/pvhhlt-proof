using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_GrapeManager : MonoBehaviour {

	[SerializeField] GameObject myGrapePrefab;
	[SerializeField] float myFirstSpawnTime = 5;
	[SerializeField] float mySpawnTime = 5;
	private float myTimer;

	[SerializeField] AudioClip mySFX_Spawn;

	// Use this for initialization
	void Start () {
		myTimer = myFirstSpawnTime;
		myGrapePrefab = Instantiate (myGrapePrefab, this.transform) as GameObject;
		myGrapePrefab.GetComponent<PP_Grape> ().SetMyManager (this);
		myGrapePrefab.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (myTimer > 0) {
			myTimer -= Time.deltaTime;
			if (myTimer <= 0) {
				myGrapePrefab.transform.position = this.transform.position;
				myGrapePrefab.SetActive (true);
				myTimer = 0;

				CS_AudioManager.Instance.PlaySFX (mySFX_Spawn);
			}
		}
	}

	public void StartSpawnTimer () {
		myTimer = mySpawnTime;
	}
}
