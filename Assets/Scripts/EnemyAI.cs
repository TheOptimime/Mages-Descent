using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
	public float health = 30f;

	Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();

		
	}
	
	// Update is called once per frame
	void Update () {

		if (health == 0) {
		
			Destroy (this.gameObject);
		}
		
	}

	void OnCollisionEnter2D (Collision2D other) {
	

		if (other.transform.tag == "FireBall") {
		
			health = health - 10f;
		}
	
	}


}
