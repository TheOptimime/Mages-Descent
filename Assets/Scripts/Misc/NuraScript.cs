using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NuraScript : MonoBehaviour
{
	public GameObject nura; 
	Animator anim;
	public Image diaglogue;

    // Start is called before the first frame update
    void Start()
    {
		anim = GetComponent<Animator> ();
		diaglogue.gameObject.SetActive (false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnTriggerEnter2D (Collider2D other){
		if (other.transform.tag == "Player") {
		
			diaglogue.gameObject.SetActive (true);
			anim.SetTrigger ("talk");
		}
	
	}

	void OnTriggerExit2D (Collider2D other){
		if (other.transform.tag == "Player") {
		
			diaglogue.gameObject.SetActive (false);
		
		}
	
	}
}
