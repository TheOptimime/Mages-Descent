using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSwitch : MonoBehaviour {

    public GameObject backgroundOverride;

	// Use this for initialization
	void Start () {
        //spr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            backgroundOverride.SetActive(!backgroundOverride.activeSelf);
    }
}
