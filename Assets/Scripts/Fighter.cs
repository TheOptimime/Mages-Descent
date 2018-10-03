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

    public GameObject leftBullet, rightBullet;

    Transform firePos;

    CharacterController2D fighterController;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private Rigidbody2D rb;

    public float castTime = 0;
    public float finishedCast = 1;


    


    // Use this for initialization
    void Start()
    {
        fighterController = GetComponent<CharacterController2D>();
        controller = GetComponent<CharacterController2D>();
        rb = GetComponent<Rigidbody2D>();

        firePos = transform.Find("SpellSpawner");
       
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
             
        }

        if (Input.GetKey(KeyCode.F))
        {
            castTime += Time.deltaTime;
            
        }

        if (Input.GetKeyUp(KeyCode.F) && (castTime > finishedCast))
        {
			StartCoroutine(FireballSequence());
            castTime = 0;

        }
        if (Input.GetKeyUp(KeyCode.F) && (castTime < finishedCast))
        {
            castTime = 0;

        }



     }


    private void FixedUpdate()
    {
        //move character
        controller.Move(horizontalMove * Time.deltaTime, false, jump);
        jump = false;

    }

    void Fire()
    {
        if (fighterController.m_FacingRight)
        {
            Instantiate(rightBullet, firePos.position, Quaternion.identity);
        }
        if (!fighterController.m_FacingRight)
        {
            Instantiate(leftBullet, firePos.position, Quaternion.identity);
        }

    }

	IEnumerator FireballSequence () {
	
		Fire ();
		yield return new WaitForSeconds(.2f); 
		Fire ();
		yield return new WaitForSeconds(.2f); 
		Fire ();

	}

}
