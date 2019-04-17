using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBeam : MonoBehaviour
{

	public Transform spawner; 
	public GameObject particleToSpawn;

	public float speed; 
	float timer; 
    // Start is called before the first frame update
    void Start()
    {
		timer = .01f;
    }

    // Update is called once per frame
    void Update()
    {
		timer -= Time.deltaTime;
		if (timer <= 0) {
			Invoke ("SpawnParticle", 0);
			timer = .01f;
		
		}


	
    }

	public void SpawnParticle() {
	
	
		Instantiate (particleToSpawn, spawner.position, spawner.rotation);

	}
}
