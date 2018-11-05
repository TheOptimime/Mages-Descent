using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveSet))]
[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(CharacterController2D))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Inventory))]
public partial class Fighter : MonoBehaviour {

    #region Variables
    public float speed;
    public CharacterController2D cc;
    SpellDatabase spellList;

    public bool attackIsInQueue;

    Attack attackInQueue;

    public Transform spellCastPoint;

    InputHandler input;

    Vector3 movePos;

    public float horizontalMove = 0f, verticalMove = 0f;

    public float runSpeed = 40f;

    public MoveSet moveset;    

    public bool jump = false;
    public bool isDashing, recentlyAttacked;
    public bool isFacingRight;
    public bool canDoubleJump, doubleJumpUsed;
    public bool lockMovement;

    public float forwardLeapSpeed, backStepSpeed, backwardLeapSpeed;

    public GameObject fireBullet;

    public SpriteRenderer sr;

    //Transform firePos;

    //CharacterController2D cc;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private Rigidbody2D rb;

    public float castTime = 0;
    public float finishedCast = 1;
    #endregion

    void Start()
    {
        //fightercc = GetComponent<Charactercc2D>();
        cc = GetComponent<CharacterController2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        moveset = GetComponent<MoveSet>();
        spellList = FindObjectOfType<SpellDatabase>();
        input = GetComponent<InputHandler>();
    }

    void Update()
    {

        if (!lockMovement)
        {
            Move();
        }
        
        isFacingRight = cc.m_FacingRight;
    }

    private void Move()
    {
        if (input.joystickPosition == 4 || input.state.ThumbSticks.Left.X < -input.deadzone)
        {
            if (!isDashing)
            {
                horizontalMove = -(speed);
            }
            else
            {
                horizontalMove = -(runSpeed);
            }
        }
        else if (input.joystickPosition == 6 || input.state.ThumbSticks.Left.X > input.deadzone)
        {
            if (!isDashing)
            {
                horizontalMove = speed;
            }
            else
            {
                horizontalMove = runSpeed;
            }
        }
        else
        {
            horizontalMove = 0;
        }
    }

    private void FixedUpdate()
    {
        

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;

        }
        else if (rb.velocity.y > 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }

        //move character
        cc.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;

    }

    public bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, sr.sprite.bounds.extents.y);
    }

    public void Leap()
    {

    }

    void SetVibration(float leftMotor, float rightMotor)
    {
        input.vibrateLeftMotor = leftMotor;
        input.vibrateRightMotor = rightMotor;
    }
}
