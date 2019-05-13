using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChandelierSwing : MonoBehaviour
{
    Animator anim;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            anim.SetTrigger("collide");
        }
    }
}
