﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveSet))]
[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(PlayerController2D))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AilmentHandler))]
public partial class Fighter : MonoBehaviour {

    #region Variables
    public float speed;
    public PlayerController2D cc;
    SpellDatabase spellList;

    public bool attackIsInQueue, attackInProgress;

    Attack attackInQueue;

    public Transform spellCastPoint;

    InputHandler input;

    Vector3 movePos;

    public float horizontalMove = 0f, verticalMove = 0f;

    float comboMeter, burstMeter;

    public float recoveryTimer, recoveryTime;

    public float runSpeed = 40f;

    public MoveSet moveset;

    public int jumpCount;

    public bool jump = false, specialJump = false;
    public bool isDashing, recentlyAttacked;
    public bool isFacingRight;
    public bool canDoubleJump, doubleJumpUsed;
    public bool lockMovement, lockInput, canRecover;

    public bool isLeaping, isBackLeaping, isBackStepping;

    public float forwardLeapSpeed, backStepSpeed, backwardLeapSpeed;

    public GameObject fireBullet;
    public GameObject lowHitbox, midHitbox, highHitbox;

    GameManager gm;

    public SpriteRenderer sr;

    Animator anim;

    AudioSource audioSource;

    IEnumerator hitstunCoroutine;

    //Transform firePos;
    Health health;
    //CharacterController2D cc;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public Rigidbody2D rb;

    public float castTime = 0;
    public float finishedCast = 1;

    KnockbackListener knockbackListener;
    AilmentHandler ailmentHandler;

    RespawnManager rm;
    #endregion

    void Start()
    {
        cc = GetComponent<PlayerController2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        moveset = GetComponent<MoveSet>();
        input = GetComponent<InputHandler>();
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        audioSource = GetComponent<AudioSource>();
        ailmentHandler = GetComponent<AilmentHandler>();
        knockbackListener = GetComponent<KnockbackListener>();

        spellList = FindObjectOfType<SpellDatabase>();


        input.player = this;
        input.spellDatabase = spellList;



                
        gm = FindObjectOfType<GameManager>();
        rm = FindObjectOfType<RespawnManager>();

        cc.m_doubleJumpEnabled = canDoubleJump;
    }

    void Update()
    {
        
        if (recentlyAttacked && lockMovement != true)
        {
            // Player was just attacked

            print("recovery time set: " + recoveryTimer);
            recoveryTime = 0;
            attackIsInQueue = false;
            attackInQueue = null;
            attackInProgress = false;
            lockMovement = true;
        }
        else if (recentlyAttacked)
        {
            print("recovering");
            recoveryTime += Time.deltaTime;

            if(recoveryTime > recoveryTimer)
            {
                print("recovered");

                if (cc.m_Grounded)
                {
                    // Insert some lag time and stuff
                    lockMovement = false;
                }

                if (jump)
                {
                    RecoveryJump();
                }
                lockMovement = recentlyAttacked = false;
            }
        }
        

        if (!lockMovement)
        {
            Move();
        }
        else
        {
            input.joystickPosition = 5;
            input.joystickRecord.Clear();
        }
        
        isFacingRight = cc.m_FacingRight;

        if(health.currentHealth <= 0)
        {
            Respawn();
        }


        if (cc.m_Grounded && IsGrounded())
        {
            cc.m_doubleJumpUsed = doubleJumpUsed = false;
            jumpCount = 0;
            specialJump = false;
            isLeaping = isBackLeaping = isBackStepping = false;
            cc.m_AirControl = true;
        }

        if (attackIsInQueue)
        {
            if (!attackInProgress)
            {

            }
        }

        print("Leaping: " + isLeaping + " BackStep: " + isBackStepping + "BackLeaping: " + isBackLeaping);
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
            anim.SetFloat("speed", 0);
        }

        if (horizontalMove < 0 || horizontalMove > 0) {
            anim.SetFloat("speed", 1);
        }
    }

    void RecoveryJump()
    {
        lockMovement = false;
        recentlyAttacked = false;
        jump = false;
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1);

        }
        else if (rb.velocity.y > 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 0.8f);
        }

        if(jump && jumpCount < 2 && rb.velocity.y < 0 && !recentlyAttacked)
        {
            rb.velocity = Vector2.zero;
        }

        //move character
        if (!lockMovement && !specialJump)
        {
            cc.Move(horizontalMove, jump && jumpCount < 2);

            if (jumpCount > 0 && jump)
            {
                doubleJumpUsed = true;
                jumpCount++;
            }

            if (cc.m_doubleJumpEnabled)
            {
                if (jumpCount < 1)
                {
                    
                    jumpCount++;
                }
            }

            if(jumpCount > 1)
            {
                cc.m_doubleJumpUsed = doubleJumpUsed = true;
            }
            
            
        }
        else if (!lockMovement && specialJump)
        {
            if (isLeaping)
            {
                print("is leaping");
                Leap();
            }
            else if (isBackLeaping)
            {
                BackLeap();
            }
            else if (isBackStepping)
            {
                BackStep();
            }
        }

        jump = false;
        

    }

    public bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, sr.sprite.bounds.extents.y);
    }

    public void Leap()
    {
        cc.m_AirControl = false;
        jumpCount += 2;
        specialJump = true;
        cc.Move(forwardLeapSpeed, jump, PlayerController2D.JumpType.Long);
        canDoubleJump = false;
    }

    public void BackLeap()
    {
        cc.m_AirControl = false;
        jumpCount += 2;
        specialJump = true;
        cc.Move(backwardLeapSpeed, jump, PlayerController2D.JumpType.Back);
        canDoubleJump = false;
    }

    public void BackStep()
    {
        cc.m_AirControl = false;
        jumpCount += 2;
        specialJump = true;
        cc.Move(backStepSpeed, jump, PlayerController2D.JumpType.Back);
        canDoubleJump = false;
    }

    void SetVibration(float vibration)
    {
        input.vibrateLeftMotor = input.vibrateRightMotor = vibration;
    }

    void SetVibration(float leftMotor, float rightMotor)
    {
        input.vibrateLeftMotor = leftMotor;
        input.vibrateRightMotor = rightMotor;
    }

    void Respawn()
    {
        transform.position = rm.activeSpawnPoint.position;
        health.currentHealth = health.maxHealth;
    }

    public void SetHitstunTimer(float time)
    {
        if(hitstunCoroutine != null)
        {
            StopCoroutine(hitstunCoroutine);
        }
        hitstunCoroutine = HitstunTimer(time);
        StartCoroutine(hitstunCoroutine);
    }

    IEnumerator HitstunTimer(float time)
    {
        lockInput = lockMovement = true;
        yield return new WaitForSeconds(time);
        lockInput = lockMovement = false;
        hitstunCoroutine = null;
    }
}
