using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timedPlatform : MonoBehaviour
{

	public GameObject player;
	Animator anim; 
	public float speed;
	public bool isColliding; 
	public float startWaitTime;
	public float waitTime;


    // Start is called before the first frame update
    void Start()
    {
		anim = GetComponent<Animator> ();
		transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
		
    }

	public void OnCollisionEnter2D (Collision2D other){
		if (other.gameObject.tag == "Player") {
			anim.SetTrigger ("wiggle");

		}

	
	}

	public void OnCollisionExit2D (Collision2D other) {
		if (other.gameObject.tag == "Player") {
			
		
		}

	}


}
