using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_BeanManager : MonoBehaviour {

	private static PP_BeanManager instance = null;

	//========================================================================
	public static PP_BeanManager Instance {
		get { 
			return instance;
		}
	}

	void Awake () {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
		} else {
			instance = this;
		}

//		DontDestroyOnLoad(this.gameObject);
	}
	//========================================================================

//	[SerializeField] Transform[] mySpawnPoint;
	[SerializeField] GameObject myBeanPrefeb;
	private List<GameObject> myBeanPool = new List<GameObject> ();
	[SerializeField] int myBeanMaxNumber = 30;

	[SerializeField] GameObject myBoundsMarker;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < myBeanMaxNumber; i++) {
			myBeanPool.Add (Instantiate (myBeanPrefeb, this.transform));
		}
	}

//	// Update is called once per frame
//	void Update () {
//		
//	}

	public GameObject GetBean () {
		for (int i = 0; i < myBeanPool.Count; i++) {
			if (myBeanPool [i].activeSelf == false) {
				return myBeanPool [i];
			}
		}

		Debug.Log ("Run out of bean!");
		GameObject t_bean = Instantiate (myBeanPrefeb, this.transform) as GameObject;
		myBeanPool.Add (t_bean);
		return t_bean;
	}
		
	public Vector3 GetBounds() {
		return myBoundsMarker.transform.position;
	}
}
