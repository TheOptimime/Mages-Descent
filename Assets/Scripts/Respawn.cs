using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {

	[SerializeField]
	public GameObject spawnpoint;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){

        if (other.gameObject.GetComponent<EnemyAI>() || other.gameObject.GetComponent<Fighter>())
		other.gameObject.transform.position = spawnpoint.transform.position;
	
	}
}
