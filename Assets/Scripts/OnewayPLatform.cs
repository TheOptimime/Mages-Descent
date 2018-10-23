using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnewayPLatform : MonoBehaviour {

    private PlatformEffector2D effector;
    public float waitTime;

	// Use this for initialization
	void Start () {
        effector = GetComponent<PlatformEffector2D>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyUp(KeyCode.S))
        {
            waitTime = 0.01f;
        }
        if ( Input.GetKey(KeyCode.S))
        {
            if (waitTime <= 0)
            {
                effector.rotationalOffset = 180f;
                waitTime = 0.01f;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
        
        if (Input.GetKey(KeyCode.Space))
        {
            effector.rotationalOffset = 0f;
        }

	}
}
