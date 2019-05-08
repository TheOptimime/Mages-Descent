using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEnder : MonoBehaviour
{

    public GameObject collisionParticle; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Enemy" || other.transform.tag == "Ground" || other.transform.tag == "Platform") {
            Instantiate(collisionParticle, transform.position, transform.rotation);
        }
    }
}
