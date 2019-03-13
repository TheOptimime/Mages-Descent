using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformMove : MonoBehaviour
{
	public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnCollisionEnter2D (Collision2D other){
		if (other.gameObject.tag == "Player") {
			player.transform.parent = this.transform;
		} 
	}

	void OnCollisionExit2D (Collision2D other){
		if (other.gameObject.tag == "Player") {
			player.transform.parent = null;
		
		}
	
	}
}
