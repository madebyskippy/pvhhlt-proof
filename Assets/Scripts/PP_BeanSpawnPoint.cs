using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_BeanSpawnPoint : MonoBehaviour {

	[SerializeField] int mySpawnMaxNumber = 5;
	private int mySpawnCount = 0;
	[SerializeField] float mySpawnRadius = 1;
	[SerializeField] float mySpawnTime = 5;
	private float myTimer;

	[SerializeField] GameObject myBoundsMarker;

	// Use this for initialization
	void Start () {
		myTimer = mySpawnTime;
	}
	
	// Update is called once per frame
	void Update () {
		if (myTimer > 0) {
			myTimer -= Time.deltaTime;
		} else if (mySpawnCount < mySpawnMaxNumber) {
			mySpawnCount++;
			myTimer = mySpawnTime;

			GameObject t_bean = PP_BeanManager.Instance.GetBean ();
			t_bean.GetComponent<PP_Bean> ().Init (this.transform.position + (Vector3)Random.insideUnitCircle * mySpawnRadius, this.transform.position, mySpawnRadius, this);
		}
	}

	public Vector3 GetBounds() {
		return myBoundsMarker.transform.position;
	}

	public void RemoveBean () {
		mySpawnCount--;
	}
}
