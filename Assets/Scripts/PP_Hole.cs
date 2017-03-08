using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Hole : MonoBehaviour {
	enum Status : byte {
		Idle,
		Open,
		Close,
		Pop
	};

	[SerializeField] float myOpenTime = 0.1f;
	[SerializeField] float myCloseTime = 0.1f;
	[SerializeField] float myPopTime = 0.5f;
	private float myTimer = 0;
	[SerializeField] float myOpenSizeDelta;
	private float myCloseSize = 1;
	private Status myStatus = Status.Idle;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (myStatus == Status.Open) {
			this.transform.localScale = Vector3.Lerp (Vector3.one * myCloseSize, Vector3.one * (myOpenSizeDelta + myCloseSize), 1 - myTimer / myOpenTime);			

			myTimer -= Time.deltaTime;
			if (myTimer <= 0) {
				myTimer = myCloseTime;
				myStatus = Status.Close;
			}
		} else if (myStatus == Status.Close) {
			myTimer -= Time.deltaTime;
			if (myTimer <= 0) {
				myStatus = Status.Idle;
			}

			this.transform.localScale = Vector3.Lerp (Vector3.one * (myOpenSizeDelta + myCloseSize), Vector3.one * myCloseSize, 1 - myTimer / myOpenTime);
		} else if (myStatus == Status.Pop) {
			myTimer -= Time.deltaTime;
			if (myTimer <= 0) {
				myStatus = Status.Idle;
			}

			this.transform.localScale = Vector3.Lerp (this.transform.localScale, Vector3.one * myCloseSize, 1 - myTimer / myPopTime);
		}
	}

	public void Eat (float g_closeSize) {
		myCloseSize = g_closeSize;
		myTimer = myOpenTime;
		myStatus = Status.Open;
	}

	public void Pop (float g_closeSize) {
		myCloseSize = g_closeSize;
		myTimer = myPopTime;
		myStatus = Status.Pop;
	}

}
