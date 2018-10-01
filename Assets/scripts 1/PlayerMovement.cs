using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public CharacterController2D controller;

    float horizontalMove = 0f;

     float runSpeed = 40f;

     bool jump = false;

    bool dab = false; 

    //public Animator animator;

    

	// Use this for initialization
	void Start () {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        //animator.SetFloat("Speed", Mathf.Abs(horizontalMove));









        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
            //animator.SetBool("isJumping", true);    
        }


       
   }

    /*public void OnLanding()
    {
        animator.SetBool("isJumping", false);
    }*/

    private void FixedUpdate()
    {
        //move character
        controller.Move(horizontalMove * Time.deltaTime, false, jump);
        jump = false;
        
    }

    

}
