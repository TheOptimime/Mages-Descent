using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallControl : MonoBehaviour {

    public Vector2 speed;

    Rigidbody2D rb;
    public float delay;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = speed;
        Destroy(this.gameObject, delay);
	}
	
	// Update is called once per frame
	void Update () {
        rb.velocity = speed;
	}

	void OnCollisionEnter2D (Collision2D other) {
		if (other.transform.tag == "Enemy") {
		
			Destroy (this.gameObject);
		
		}
	
	}
}
