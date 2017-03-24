using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Butt : MonoBehaviour {
	[SerializeField] Rigidbody2D myRigidbody2D;

	[SerializeField] GameObject myPlayerPrefab;
	[SerializeField] GameObject myBodyPrefab;
	private List<GameObject> myPlayers = new List<GameObject> ();
	private List<GameObject> myBodies = new List<GameObject> ();
	private Vector2 mySpawnPoint;
	[SerializeField] float mySpawnRadius = 1;
	[SerializeField] float mySpeed = 5;

	private PP_ColorSet myColorSet;
//	[SerializeField] Transform mySpriteTransform;
	[SerializeField] SpriteRenderer mySpriteRenderer;
//	[SerializeField] SpriteRenderer mySpriteRendererBack;
	[SerializeField] SpriteRenderer mySpriteRendererBorder;
	[SerializeField] GameObject myButthole;
	[SerializeField] GameObject myButtSprite;
	[SerializeField] GameObject myButtSpriteBorder;
	[SerializeField] GameObject myButtSpriteMask;
	[SerializeField] float myButtSpriteDeltaScale = 0.25f;
	[SerializeField] float myButtMaskDeltaScale = 0.2f;

	private int myTeamNumber;

	[Header("Beans")]
	[SerializeField] GameObject myBeanPrefab;
	[SerializeField] int myBeansMax = 5;
	[Tooltip("x: min mass, without beans, y: max mass, full")]
	[SerializeField] Vector2 myMassRange = new Vector2 (1, 10);
	[Tooltip("x: min scale, without beans, y: max scale, full")]
	[SerializeField] Vector2 myScaleRange = new Vector2 (1, 1.5f);
	private int myBeansCurrent = 0;
	[SerializeField] GameObject myParticleBeans;

	[Header("Status")]
	[SerializeField] Color myStunColor = Color.gray;
	[SerializeField] float myStatus_StunTime = 1;
	private float myStatus_StunTimer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
//		UpdatePosition ();
		UpdateBodies ();
//		UpdatePlayers ();
		UpdateBeans ();
		UpdateStatus ();
	}

	public void Init (int g_teamNumber, Vector2 g_spawnPoint, PP_ColorSet g_colorSet) {
		myTeamNumber = g_teamNumber;
		mySpawnPoint = g_spawnPoint;
		myColorSet = g_colorSet;

		mySpriteRenderer.color = g_colorSet.myColorButt;
//		mySpriteRendererBack.color = g_buttColors [g_teamNumber + 2];
		mySpriteRendererBorder.color = g_colorSet.myColorBorder;

		this.transform.position = (Vector3)mySpawnPoint + Vector3.forward * g_teamNumber * 50;

		for (int i = 0; i < 3; i++) {
			GameObject t_player = Instantiate (myPlayerPrefab, mySpawnPoint + Random.insideUnitCircle * mySpawnRadius, Quaternion.identity) as GameObject;
			t_player.transform.position = t_player.transform.position + Vector3.forward * (i * 10 + this.transform.position.z);
			string t_control = (i + 3 * myTeamNumber + 1).ToString ();
			t_player.GetComponent<PP_Player> ().Init (myTeamNumber, this.gameObject, g_colorSet.myPlayers[i], g_colorSet.myColorBorder, t_control);
			t_player.GetComponent<SpringJoint2D> ().connectedBody = this.GetComponent<Rigidbody2D> ();
			myPlayers.Add (t_player);

			GameObject t_body = Instantiate (myBodyPrefab, mySpawnPoint, Quaternion.identity) as GameObject;
			t_body.transform.position = Vector3.forward * (i * 10 + this.transform.position.z);
			t_body.GetComponent<PP_Body> ().GetMySpriteRenderer ().color = g_colorSet.myColorBorder;

			myBodies.Add (t_body);
		}
	}

	private void UpdateStatus () {
		if (myStatus_StunTimer > 0) {
			myStatus_StunTimer -= Time.deltaTime;
			if (myStatus_StunTimer <= 0) {
				myStatus_StunTimer = 0;
				mySpriteRenderer.color = myColorSet.myColorButt;
			}
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
			float t_posZ = myBodies [i].transform.position.z;
			Vector2 t_direction = (this.transform.position - myPlayers[i].transform.position) * -1;
			Vector2 t_position = (this.transform.position + myPlayers[i].transform.position) / 2;

			Quaternion t_quaternion = Quaternion.Euler (0, 0, 
				Vector2.Angle (Vector2.up, t_direction) * Mathf.Sign (t_direction.x * -1));

			myBodies [i].transform.position = (Vector3)t_position + Vector3.forward * t_posZ;
			myBodies [i].transform.rotation = t_quaternion;
			myBodies [i].transform.localScale = new Vector3 (myBodies [i].transform.localScale.x, t_direction.magnitude, 1);
		}
	}

	private void UpdateBeans () {
		myButtSpriteBorder.transform.localScale = ((float)myBeansCurrent / myBeansMax * (myScaleRange.y - myScaleRange.x) + myScaleRange.x) * Vector2.one;
		myButtSprite.transform.localScale = (myButtSpriteBorder.transform.localScale.x - myButtSpriteDeltaScale) * Vector2.one;
		myButtSpriteMask.transform.localScale = (myButtSpriteBorder.transform.localScale.x - myButtMaskDeltaScale) * Vector2.one;
		myRigidbody2D.mass = (float)myBeansCurrent / myBeansMax * (myMassRange.y - myMassRange.x) + myMassRange.x;
	}

	public int GetMyTeamNumber () {
		return myTeamNumber;
	}

	void OnCollisionEnter2D (Collision2D g_collision2D) {
//		Debug.Log ("Butt-OnCollisionEnter");
		if (myStatus_StunTimer > 0)
			return;

		if (g_collision2D.gameObject.tag == PP_Global.TAG_BEAN && myBeansCurrent < myBeansMax) {
			g_collision2D.gameObject.GetComponent<PP_Bean> ().Kill ();
			myBeansCurrent++;

			myButthole.GetComponent<PP_Hole> ().Eat (1 - (float)myBeansCurrent / myBeansMax);

//			//tweenzzzz
//			Sequence sq = DOTween.Sequence ();
//			sq.Append (myButthole.transform.DOScale (myButthole.transform.localScale + Vector3.one * 0.5f, 0.1f));
//			sq.Append (myButthole.transform.DOScale (Vector3.one * 0.75f - Vector3.one * 0.75f * (float)myBeansCurrent / myBeansMax, 0.15f));
		}
	}

	void OnTriggerEnter2D (Collider2D g_Collider2D) {
		//		Debug.Log ("Butt-OnCollisionEnter");
		if (myStatus_StunTimer > 0)
			return;

		if (g_Collider2D.gameObject.tag == PP_Global.TAG_BEAN && myBeansCurrent < myBeansMax) {
			g_Collider2D.gameObject.GetComponent<PP_Bean> ().Kill ();
			myBeansCurrent++;

			myButthole.GetComponent<PP_Hole> ().Eat (1 - (float)myBeansCurrent / myBeansMax);
		}
	}

	public float Pop () {
		float t_bean = (float)myBeansCurrent;
//		myButthole.transform.localScale = Vector3.one * 0.75f; //the starting scale

		myButthole.GetComponent<PP_Hole> ().Pop (1); //the starting scale

		myBeansCurrent = 0;
		if (t_bean > 0) {
			Instantiate (myParticleBeans, this.transform.position, Quaternion.identity);
		}
		return t_bean;
	}

	public void SetBodySprite (int g_number, Sprite g_sprite) {
		myBodies [g_number].GetComponent<PP_Body> ().GetMySpriteRendererPattern ().sprite = g_sprite;
	}

	public void Stun () {
		myStatus_StunTimer = myStatus_StunTime;
		mySpriteRenderer.color = myStunColor;
		for (int i = 0; i < myBeansCurrent; i++) {
			Instantiate (myBeanPrefab, this.transform.position + (Vector3)(Random.insideUnitCircle * 0.1f), Quaternion.identity);
		}
		myBeansCurrent = 0;
		myButthole.GetComponent<PP_Hole> ().Pop (1);
	}
}
