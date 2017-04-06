using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_BeanManager : MonoBehaviour {

	[SerializeField] GameObject myBeanPrefeb;
	[SerializeField] int myBeanMaxNumber = 5;
	private List<GameObject> myBeans = new List<GameObject> ();
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
		} else if (myBeans.Count < myBeanMaxNumber) {
			myTimer = mySpawnTime;

			GameObject t_bean = 
				Instantiate (myBeanPrefeb, this.transform.position + (Vector3)Random.insideUnitCircle * mySpawnRadius, Quaternion.identity) as GameObject;
			t_bean.GetComponent<PP_Bean> ().Init (this.transform.position,mySpawnRadius,false);
			t_bean.GetComponent<PP_Bean> ().SetMyManager (this);
			myBeans.Add (t_bean);
		}
	}

	public void RemoveBean (GameObject g_bean) {
		myBeans.Remove (g_bean);
//		Destroy (g_bean);
//		myBeans.Remove (g_bean);
	}
}
