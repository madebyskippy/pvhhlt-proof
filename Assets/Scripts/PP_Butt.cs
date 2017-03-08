﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PP_Butt : MonoBehaviour {
	[SerializeField] GameObject myPlayerPrefab;
	[SerializeField] GameObject myBodyPrefab;
	private List<GameObject> myPlayers = new List<GameObject> ();
	private List<GameObject> myBodies = new List<GameObject> ();
	private Vector2 mySpawnPoint;
	[SerializeField] float mySpawnRadius = 1;
	[SerializeField] float mySpeed = 5;

	[SerializeField] Transform mySpriteTransform;
	[SerializeField] SpriteRenderer mySpriteRenderer;
	[SerializeField] SpriteRenderer mySpriteRendererBack;
	[SerializeField] SpriteRenderer mySpriteRendererBorder;
	[SerializeField] GameObject myButthole;

	private int myTeamNumber;

	[SerializeField] int myBeansMax = 5;
	private int myBeansCurrent = 0;
	[SerializeField] GameObject myParticleBeans;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
//		UpdatePosition ();
		UpdateBodies ();
//		UpdatePlayers ();
		UpdateBeans ();
	}

	public void Init (int g_teamNumber, Vector2 g_spawnPoint, Color[] g_playerColors, Color[] g_buttColors, Color[] g_borderColors) {
		myTeamNumber = g_teamNumber;
		mySpawnPoint = g_spawnPoint;

		mySpriteRenderer.color = g_buttColors [g_teamNumber];
		mySpriteRendererBack.color = g_buttColors [g_teamNumber + 2];
		mySpriteRendererBorder.color = g_borderColors [g_teamNumber];

		this.transform.position = mySpawnPoint;

		for (int i = 0; i < 3; i++) {
			GameObject t_player = Instantiate (myPlayerPrefab, mySpawnPoint + Random.insideUnitCircle * mySpawnRadius, Quaternion.identity) as GameObject;
			string t_control = (i + 3 * myTeamNumber + 1).ToString ();
			t_player.GetComponent<PP_Player> ().Init (myTeamNumber, g_playerColors [i + 3 * myTeamNumber], g_borderColors [g_teamNumber], t_control);
			t_player.GetComponent<SpringJoint2D> ().connectedBody = this.GetComponent<Rigidbody2D> ();
			myPlayers.Add (t_player);

			GameObject t_body = Instantiate (myBodyPrefab, mySpawnPoint, Quaternion.identity) as GameObject;
			t_body.GetComponent<SpriteRenderer> ().color = g_borderColors [g_teamNumber];

			myBodies.Add (t_body);
		}
	}

	public void UpdatePosition () {
		Vector3 t_position = myPlayers[0].transform.position;
		for (int i = 1; i < myPlayers.Count; i++) {
			t_position += myPlayers [i].transform.position;
		}

		t_position /= myPlayers.Count;

//		this.transform.position = t_position;
		this.transform.position = Vector3.Lerp (this.transform.position, t_position, Time.deltaTime * mySpeed);
	}

	private void UpdateBodies () {
		for (int i = 0; i < myBodies.Count; i++) {
			Vector2 t_direction = (this.transform.position - myPlayers[i].transform.position) * -1;
			Vector2 t_position = (this.transform.position + myPlayers[i].transform.position) / 2;

			Quaternion t_quaternion = Quaternion.Euler (0, 0, 
				Vector2.Angle (Vector2.up, t_direction) * Vector3.Cross (Vector3.up, (Vector3)t_direction).normalized.z);

			myBodies[i].transform.position = t_position;
			myBodies[i].transform.rotation = t_quaternion;
			myBodies[i].transform.localScale = new Vector3 (myBodies[i].transform.localScale.x, t_direction.magnitude, 1);
		}
	}

	private void UpdatePlayers(){
		for (int i = 0; i < myPlayers.Count; i++) {
			Vector2 t_direction = (this.transform.position - myPlayers[i].transform.position) * -1;

			Quaternion t_quaternion = Quaternion.Euler (0, 0, 
				Vector2.Angle (Vector2.up, t_direction) * Vector3.Cross (Vector3.up, (Vector3)t_direction).normalized.z);
			
			myPlayers [i].transform.rotation = t_quaternion;
		}
	}

	private void UpdateBeans () {
		
		mySpriteTransform.localScale = 0.5f*Vector3.one+0.5f*(float)myBeansCurrent / myBeansMax * Vector3.one;
		transform.localScale = Vector3.one + Vector3.one * 0.5f * (float)myBeansCurrent / myBeansMax;
//		myButthole.transform.localScale = Vector3.one * 0.75f - Vector3.one * 0.75f * (float)myBeansCurrent / myBeansMax;
//		Debug.Log (myBeansCurrent + ":" + mySpriteTransform.localScale);
	}

	public int GetMyTeamNumber () {
		return myTeamNumber;
	}

	void OnCollisionEnter2D (Collision2D g_collision2D) {
//		Debug.Log ("Butt-OnCollisionEnter");
		if (g_collision2D.gameObject.tag == PP_Global.TAG_BEAN && myBeansCurrent < myBeansMax) {
			g_collision2D.gameObject.GetComponent<PP_Bean> ().Kill ();
			myBeansCurrent++;
			//tweenzzzz
			Sequence sq = DOTween.Sequence ();
			sq.Append (myButthole.transform.DOScale (myButthole.transform.localScale + Vector3.one * 0.5f, 0.1f));
			sq.Append (myButthole.transform.DOScale (Vector3.one * 0.75f - Vector3.one * 0.75f * (float)myBeansCurrent / myBeansMax, 0.15f));
		}
	}

	public float Pop () {
		float t_bean = (float)myBeansCurrent;
		myButthole.transform.localScale = Vector3.one * 0.75f; //the starting scale
		myBeansCurrent = 0;
		if (t_bean > 0) {
			Instantiate (myParticleBeans, this.transform.position, Quaternion.identity);
		}
		return t_bean;
	}
}
