using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SkillPush : CS_Skill {
	[SerializeField] float myPushDistance;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (Collider2D g_collider2D) {
		if (g_collider2D.tag == "Player") {
			CS_Player t_player = g_collider2D.gameObject.GetComponent<CS_Player> ();

			Vector2 t_direction = (g_collider2D.transform.position - this.transform.position).normalized;
			t_player.Pushed (myCasterTeamNumber, t_direction);
		}
		if (g_collider2D.tag == "Head") {
			CS_Head t_playerHead = g_collider2D.gameObject.GetComponent<CS_Head> ();

			Vector2 t_direction = (g_collider2D.transform.position - this.transform.position).normalized;
			t_playerHead.Pushed (myCasterTeamNumber, t_direction);
		}
	}

}
