using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerdie : MonoBehaviour {

	public Animator anim;
	public Animator fadeAnim;
	public string sceneName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.transform.tag == "Player") {
			StartCoroutine (DieNLoad ());
		}


	}

	IEnumerator DieNLoad(){
	
		anim.SetBool ("death", true);
		yield return new WaitForSeconds (4f);
		fadeAnim.SetTrigger ("end");
		yield return new WaitForSeconds (1.5f);

		SceneManager.LoadScene ("Title");

	
	}
}
