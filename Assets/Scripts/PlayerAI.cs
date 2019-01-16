using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(MoveSet))]
[RequireComponent(typeof(PlayerController2D))]
[RequireComponent(typeof(Inventory))]
public class PlayerAI : AI {

    bool debugOnly = true;
    public PlayerController2D cc;

    public bool jump = false;
    public bool isDashing, recentlyAttacked;
    public bool isFacingRight;
    public bool canDoubleJump, doubleJumpUsed;
    public bool lockMovement;

    public float recoveryTimer, recoveryTime;

    public float castTime = 0;
    //public float finishedCast = 1;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public LayerMask EnemiesOnly, PlayersOnly, PlatformsOnly, ExcludeEnemies, ExcludePlayers, ExcludePlatforms;

    public BoxCollider2D emulatedCamera;

    public float runSpeed;

    bool isMoving;

    public float horizontalMove = 0f, verticalMove = 0f;

    public enum PlayerAIStates
    {
        Idle,
        Walking,
        Detecting,          // Might Scrap
        Attacking,          // Activates when nearby enemies or player
        Resting,            // Not Sure
        Attacked            // 
    }
    public enum PlayerAIBehaviour
    {
        Bloodthirsty,
        Coward,
        Chaotic,

    }
    public enum PlayerAIBehaviourModifier
    {

    }


    [Header("Player AI Settings")]
    public PlayerAIStates AIStates;
    public PlayerAIBehaviour AIBehaviour;
    public PlayerAIBehaviourModifier AIBehaviourModifier;

    public override void Start () {
        base.Start();

        if (debugOnly)
        {
            if (GetComponent<Fighter>())
            {
                Debug.LogError("Fighter Script found on PlayerAI");
            }

            if (GetComponent<EnemyAI>())
            {
                Debug.LogError("EnemyAI Script found on PlayerAI");
            }

            if (GetComponent<InputHandler>())
            {
                Debug.LogError("InputHandler Script found on PlayerAI");
            }
        }

        cc = GetComponent<PlayerController2D>();
        AIStates = new PlayerAIStates();
        
	}
	
	// Update is called once per frame
	void Update () {
        UpdateEdgeDetection();

        if (recentlyAttacked && lockMovement != true)
        {
            print("recovery time set: " + recoveryTimer);
            recoveryTime = 0;
            lockMovement = true;
        }
        else if (recentlyAttacked)
        {
            print("recovering");
            recoveryTime += Time.deltaTime;

            if (recoveryTime > recoveryTimer)
            {
                print("recovered");
                lockMovement = recentlyAttacked = false;
            }
        }


        switch (AIStates)
        {
            case PlayerAIStates.Idle:
                // Recieves no inputs
                // insert timer
                AIStates = PlayerAIStates.Walking;
                break;

            case PlayerAIStates.Walking:
                Move();
                if (wallDetected)
                {
                    if (cc.m_Grounded)
                    {
                        jump = true;
                    }
                        
                }
                break;

        }



        if (!lockMovement)
        {
            Move();
        }
    }

    private void Move()
    {
        // Moving left
        if (AIStates == PlayerAIStates.Walking)
        {
            if (!isDashing)
            {
                if(isFacingRight)
                {
                    horizontalMove = -(speed);
                }
                else
                {
                    horizontalMove = (speed);
                }
                
            }
            else if (isDashing)
            {
                if (isFacingRight)
                {
                    horizontalMove = -(runSpeed);
                }
                else
                {
                    horizontalMove = (runSpeed);
                }
            }
        }
        else
        {
            horizontalMove = 0;
        }

        if (horizontalMove < 0 || horizontalMove > 0)
        {
            anim.SetFloat("speed", 1);
        }
        else
        {
            anim.SetFloat("speed", 0);
        }
    }


    private void FixedUpdate()
    {
        
        if (rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;

        }
        else if (rb2d.velocity.y > 0)
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }

        //move character
        if (!lockMovement)
            cc.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;

    }

    public GameObject[] GetVisibleEnemies()
    {
        GameObject[] objects = null;

        Collider2D[] overlappingColliders = Physics2D.OverlapBoxAll(emulatedCamera.bounds.center, emulatedCamera.bounds.size, 0, EnemiesOnly);

        Array.Resize(ref objects, overlappingColliders.Length);

        int omit = 0;

        for (int i = 0; i < overlappingColliders.Length; i++)
        {
            if (overlappingColliders[i].tag == "Enemy")
            {
                objects[i] = overlappingColliders[i].gameObject;
            }
            else
            {
                omit++;
            }
        }

        print("Ommitted: " + omit);

        return objects;
    }

    public GameObject[] GetVisiblePlayers()
    {
        GameObject[] objects = null;

        Collider2D[] overlappingColliders = Physics2D.OverlapBoxAll(emulatedCamera.bounds.center, emulatedCamera.bounds.size, 0, PlayersOnly);

        Array.Resize(ref objects, overlappingColliders.Length);

        int omit = 0;

        for (int i = 0; i < overlappingColliders.Length; i++)
        {
            if(overlappingColliders[i].tag == "Player")
            {
                objects[i] = overlappingColliders[i].gameObject;
            }
            else
            {
                omit++;
            }
        }

        print("Ommitted: " + omit);

        return objects;
    }

    public GameObject[] GetVisiblePlatforms()
    {
        GameObject[] objects = null;

        Collider2D[] overlappingColliders = Physics2D.OverlapBoxAll(emulatedCamera.bounds.center, emulatedCamera.bounds.size, 0, PlatformsOnly);

        Array.Resize(ref objects, overlappingColliders.Length);

        int omit = 0;

        for (int i = 0; i < overlappingColliders.Length; i++)
        {
            if (overlappingColliders[i].tag == "Ground" || overlappingColliders[i].tag == "Platform")
            {
                objects[i] = overlappingColliders[i].gameObject;
            }
            else
            {
                omit++;
            }
        }

        print("Ommitted: " + omit);

        return objects;
    }

    private void Turn()
    {
        direction *= -1;
        isFacingRight = !isFacingRight;
    }

    void Respawn()
    {
        transform.position = rm.activeSpawnPoint.position;
        health.currentHealth = health.maxHealth;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.2f, 0.5f, 0f, 0.4f);
        Gizmos.DrawCube(emulatedCamera.bounds.center, emulatedCamera.bounds.size);

        Gizmos.color = new Color(0, 1, 1);
        Gizmos.DrawSphere(edgeDetectionBack.position, 0.1f);

        Gizmos.color = new Color(1, 0, 1);
        Gizmos.DrawSphere(edgeDetectionFront.position, 0.1f);
    }
}
