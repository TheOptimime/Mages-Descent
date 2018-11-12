using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public Transform spawnPoint;
    //public bool isActive;

    RespawnManager rm;

	// Use this for initialization
	void Start () {
        rm = FindObjectOfType<RespawnManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        rm.activeSpawnPoint = spawnPoint;
    }
}
