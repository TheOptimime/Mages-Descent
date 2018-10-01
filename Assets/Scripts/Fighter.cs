using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveSet))]
[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(CharacterController2D))]
public class Fighter : MonoBehaviour {

    public float speed;
    CharacterController2D controller;

    float horizontalMove = 0f;

    float runSpeed = 40f;

    bool jump = false;

    bool dab = false;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private Rigidbody2D rb;


    // Use this for initialization
    void Start()
    {

        controller = GetComponent<CharacterController2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {


        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        //animator.SetFloat("Speed", Mathf.Abs(horizontalMove));


        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
            //animator.SetBool("isJumping", true);    
        }



    }


    private void FixedUpdate()
    {
        //move character
        controller.Move(horizontalMove * Time.deltaTime, false, jump);
        jump = false;

    }
}
