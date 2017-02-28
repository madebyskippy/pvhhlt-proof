using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_BeanManager : MonoBehaviour {

	[SerializeField] GameObject myBeanPrefeb;

	[SerializeField] float mySpawnRadius = 1;
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
				GameObject t_bean = 
					Instantiate (myBeanPrefeb, this.transform.position + (Vector3)Random.insideUnitCircle * mySpawnRadius, Quaternion.identity) as GameObject;
				myTimer += mySpawnTime;
			}
		}
	}

	public void StartRespawnTimer () {
		myTimer = mySpawnTime;
	}
}
