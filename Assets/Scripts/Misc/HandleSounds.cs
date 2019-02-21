using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleSounds : MonoBehaviour
{

	public AudioClip footstep;
	 AudioSource audioS;

    // Start is called before the first frame update
    void Start()
    {
		audioS = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    public void PlayFootstep()
    {
		audioS.PlayOneShot (footstep, 0.1f);
    }
}
