using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
	public float health = 30f;

	Rigidbody2D rb2d;
	public GameObject spawnpoint;

	float timer;
	bool timerSet;
	float timeLimit;

	bool isDead, respawnCalled;
	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();

		
	}
	
	// Update is called once per frame
	void Update () {

		if (health <= 0) {
			isDead = true;
		}

		if(isDead){
			print("enemy is dead");
			transform.position = new Vector3 (0, 3000, 0);
			respawnCalled = true;


		}
		if (respawnCalled) {
			Respawn ();
		}

		
	}

	void Respawn(){
		if (!timerSet) {
			timer = Time.deltaTime;
			timeLimit = Time.deltaTime + 5f;
			timerSet = true;
			print ("timer set");
		}

		if (timer > timeLimit) {
			print ("respawn");
			rb2d.velocity = Vector2.zero;
			health = 30f;
			isDead = false;
			timerSet = false;
			respawnCalled = false;
			transform.position = spawnpoint.transform.position;
		} 
		else {
			//print ("timer: " + timer);
			timer += Time.deltaTime;
		}

	}



	void OnCollisionEnter2D (Collision2D other) {
	

		if (other.transform.tag == "FireBall") {
		
			health = health - 10f;
		}
	
	}


}
