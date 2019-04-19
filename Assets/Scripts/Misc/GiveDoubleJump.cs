using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GiveDoubleJump : MonoBehaviour
{

	public Fighter fighter;
	public GameObject player;
	public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
		text.gameObject.SetActive (false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	void OnTriggerEnter2D(Collider2D other){
		if (other.transform.tag == "Player") {
			fighter.canDoubleJump = true;
			fighter.maxNumberOfJumps = 3;
			text.gameObject.SetActive (true);
			Destroy (this.gameObject);

		
		}
	
	}
}
