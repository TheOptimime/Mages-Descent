using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummy : MonoBehaviour
{

    Health health;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health.takeDmg)
        {
            anim.SetBool("hit", true);
        }
        else 
        {
            anim.SetBool("hit", false);
        }
    
       
    }
}
