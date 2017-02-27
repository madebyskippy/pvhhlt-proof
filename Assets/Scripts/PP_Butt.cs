﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Butt : MonoBehaviour {
	[SerializeField] GameObject myPlayerPrefab;
	[SerializeField] GameObject myBodyPrefab;
	private List<GameObject> myPlayers = new List<GameObject> ();
	private List<GameObject> myBodies = new List<GameObject> ();
	private Vector2 mySpawnPoint;
	[SerializeField] float mySpawnRadius = 1;
	[SerializeField] float mySpeed = 5;

	[SerializeField] SpriteRenderer mySpriteRenderer;

	private int myTeamNumber;
	private Color[] myColors;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
//		UpdatePosition ();
		UpdateBodies ();
	}

	public void Init (int g_teamNumber, Color[] g_colors, Vector2 g_spawnPoint, Color[] g_midColors) {
		myTeamNumber = g_teamNumber;
		myColors = g_colors;
		mySpawnPoint = g_spawnPoint;

		mySpriteRenderer.color = g_midColors [g_teamNumber];

		this.transform.position = mySpawnPoint;

		for (int i = 0; i < 3; i++) {
			GameObject t_player = Instantiate (myPlayerPrefab, mySpawnPoint + Random.insideUnitCircle * mySpawnRadius, Quaternion.identity) as GameObject;
			string t_control = (i + 3 * myTeamNumber + 1).ToString ();
			t_player.GetComponent<PP_Player> ().Init (myTeamNumber, myColors [i + 3 * myTeamNumber], t_control);
			t_player.GetComponent<SpringJoint2D> ().connectedBody = this.GetComponent<Rigidbody2D> ();
			myPlayers.Add (t_player);

			GameObject t_body = Instantiate (myBodyPrefab, mySpawnPoint, Quaternion.identity) as GameObject;
			t_body.GetComponent<SpriteRenderer> ().color = myColors [i + 3 * myTeamNumber];
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
			Vector2 t_direction = this.transform.position - myPlayers[i].transform.position;
			Vector2 t_position = (this.transform.position + myPlayers[i].transform.position) / 2;

			Quaternion t_quaternion = Quaternion.Euler (0, 0, 
				Vector2.Angle (Vector2.up, t_direction) * Vector3.Cross (Vector3.up, (Vector3)t_direction).normalized.z);

			myBodies[i].transform.position = t_position;
			myBodies[i].transform.rotation = t_quaternion;
			myBodies[i].transform.localScale = new Vector3 (myBodies[i].transform.localScale.x, t_direction.magnitude, 1);
		}
	}

	public int GetMyTeamNumber () {
		return myTeamNumber;
	}

}
