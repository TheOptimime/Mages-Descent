using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textfade : MonoBehaviour {

	public Animator anim;

	public GameObject text;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


		
	}

	void OnTriggerEnter2D (Collider2D other){
	
		text.SetActive (true);


		}
	
	}

