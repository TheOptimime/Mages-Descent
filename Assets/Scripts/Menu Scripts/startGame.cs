using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startGame : MonoBehaviour {
	public Animator transtitionAnim;
	public string sceneName;

	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetButtonDown("Submit")){
			StartCoroutine (LoadScene());

		}
		
	}

	IEnumerator LoadScene() {
		transtitionAnim.SetTrigger ("end");
		yield return new WaitForSeconds (1.5f);
		SceneManager.LoadScene (sceneName);

	}
}
