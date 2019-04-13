using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startGame : MonoBehaviour {
	public Animator transtitionAnim;
	public string sceneName;
    

    void Update() {

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Submit")){
			StartCoroutine (LoadScene());

		}
		
	}

	IEnumerator LoadScene() {
		transtitionAnim.SetTrigger ("end");
		yield return new WaitForSeconds (1.5f);

        // Fail-safe
        if(SceneManager.GetSceneByName(sceneName).IsValid())
        {
            SceneManager.LoadScene (sceneName);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
        
		

	}
}
