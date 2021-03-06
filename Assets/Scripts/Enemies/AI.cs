﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(AilmentHandler))]
public class AI : MonoBehaviour {

    public Health health;

    public Vector2 startingPoint;
    public Transform spellCastPoint;

    public SpellDatabase spellIndex;
    public Rigidbody2D rb2d;

    public bool edgeDetected, wallDetected, ignoreEdgeDetection;
    public int direction = 1;

    public Transform edgeDetectionFront, edgeDetectionBack;

    public float timer, idleTimer, idleTime, hitTime, restTime, restTimer, recoveryTime, recoveryTimer;
    
    public float speed;

    public LayerMask EdgeDetectIgnore;

    public RespawnManager rm;
    public Animator anim;
    public AilmentHandler ailmentHandler;

    IEnumerator hitstunCoroutine;
    public bool lockMovement;

    public enum TurnStyle
    {
        TurnByEdge,
        TurnByTrigger,
        TurnByDistance
    }

    public TurnStyle turnStyle;

    public virtual void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        spellIndex = FindObjectOfType<SpellDatabase>();
        startingPoint = transform.position;
        anim = GetComponent<Animator>();
        rm = FindObjectOfType<RespawnManager>();
        ailmentHandler = GetComponent<AilmentHandler>();
    }
	
	// Update is called once per frame
	void Update () {
        if (recoveryTimer > 0)
        {
            lockMovement = true;

            if (recoveryTime > recoveryTimer)
            {
                lockMovement = false;
            }
            recoveryTimer -= Time.deltaTime;
        }
    }

    public float DistanceBetween(GameObject GO)
    {
        return (Vector2.Distance(gameObject.transform.position, GO.transform.position));
    }

    public void UpdateEdgeDetection()
    {
        if(edgeDetectionBack && edgeDetectionFront)
        {
            RaycastHit2D floorHitInfo = Physics2D.Raycast(edgeDetectionFront.position, Vector2.down, 0.1f, EdgeDetectIgnore);
            RaycastHit2D wallHitInfo = Physics2D.Raycast(edgeDetectionFront.position, Vector2.right * direction, 0.01f, EdgeDetectIgnore);

            //print("floor: " + floorHitInfo.collider.gameObject.name);
            //print("wall: " + wallHitInfo.collider.gameObject.name);

            if (floorHitInfo.collider == null)
            {
                // edge found
                edgeDetected = true;
                print("edge detected");
            }
            else if (wallHitInfo.collider != null && wallHitInfo.collider.tag != "Player")
            {
                // test for wall
                wallDetected = true;
                //print("wall detected");
            }
            else
            {
                edgeDetected = false;
                wallDetected = false;
            }
        }
        
    }


    public void SetHitstunTimer(DoubleTime recovery)
    {
        recoveryTimer = recovery.defaultTime;
        recoveryTime = recovery.cancelTime;
    }


}
