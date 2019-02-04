using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {

	
	public Transform spawnpoint;
    RespawnManager rm;
	

	void Start () {
        rm = FindObjectOfType<RespawnManager>();

        rm.activeSpawnPoint = spawnpoint;
	}
	
	
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){

        if (other.gameObject.GetComponent<Fighter>())
        {
            other.gameObject.GetComponent<Fighter>().cc.FreezeVelocity();
            other.gameObject.transform.position = rm.activeSpawnPoint.position;
        }
		
	
	}
}
