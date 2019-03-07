using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class die : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		
    }

	void OnTrigger2D(Collider other){
		if (other.gameObject.tag == "hitbox") {
			Destroy (this.gameObject);
		
		}
	
	}
}
