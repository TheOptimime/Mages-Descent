﻿using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyController2D))]
public class EnemyAI : AI {

    
    //float internalTimer;

    Fighter player;
    
    SpriteRenderer spr;
    
    public EnemyController2D ec;

    public float walkToPlayerSpeed;
    public float murderMeter, murderMeterLimit;
	public GameObject deathParticle; 

    public enum EnemyState
    {
        Idle,
        Walking,
        Detecting,
        Attacking,
        Resting,
        Attacked
    }

    public EnemyState enemyState;

    GameObject AttackHitbox;

    public bool respawnEnabled;

    public bool stunned;
    public GameObject sightRange;
    public BoxCollider2D sightBox;

    public float attackRange, hitTimer;
    bool timerSet, idleTimerSet, hitTimerSet, restTimerSet;
    public bool hit, hitTarget;
    float timeLimit;

    bool isDead, respawnCalled, resting, playerInRange;
    bool hasRangedAttack, forceTurn;

    public float turnTimer, turnTime, detectTime, detectTimer, coolDownTimer, coolDownTime;

    public LayerMask ThisIsPlayer;

    public override void Start() {
        base.Start();



        ec = GetComponent<EnemyController2D>();
        player = FindObjectOfType<Fighter>();
        health = GetComponent<Health>();
        spr = GetComponent<SpriteRenderer>();
        enemyState = new EnemyState();

        murderMeterLimit = Mathf.Round(health.maxHealth / Random.Range(2,6));

        if (speed == 0 && walkToPlayerSpeed == 0)
        {
            Debug.LogWarning("Speeds are not properly set");
        }

        if (edgeDetectionBack == null || edgeDetectionFront == null)
        {

            Debug.LogWarning("Edge Detection not set up for enemy");
        }
        else
        {
            //direction = (int)Mathf.Sign(spr.bounds.center.x + edgeDetectionFront.position.x);
            direction = ec.m_FacingRight ? 1 : -1;
        }


    }


    public virtual void Update() {

        UpdateEdgeDetection();

        if (enemyState == EnemyState.Walking)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
			
        switch (enemyState)
        {
            case (EnemyState.Idle):
                //print("in idle");
                anim.SetTrigger("Idle");

                if (idleTimerSet != true)
                {
                    idleTime = 0;
                    idleTimer = Random.Range(2.5f, 7f);
                    idleTimerSet = true;
                }

                turnTime = 0;

                if (edgeDetected)
                {
                    // wait
                    if (idleTime > idleTimer)
                    {
                        print("Turn Around");
                        edgeDetected = false;
                        idleTimerSet = false;
                        ignoreEdgeDetection = true;
                        //ec.Move(direction * walkSpeed * Time.deltaTime, false, false);
                        enemyState = EnemyState.Walking;
                        
                    }
                }
                else
                {
                    if (idleTime > idleTimer)
                    {
                        //direction *= -1;
                        edgeDetected = false;
                        idleTimerSet = false;
                        enemyState = EnemyState.Walking;
                    }
                }

                idleTime += Time.deltaTime;
                break;
		case (EnemyState.Walking):
			//print ("in walking");
                
                if(turnStyle == TurnStyle.TurnByEdge)
                {
                    if (edgeDetected)
                    {
                        forceTurn = true;
                        print("edge detected");
                    }
                }
                else if(turnStyle == TurnStyle.TurnByDistance)
                {
                    if(turnTime > turnTimer)
                    {
                        turnTime = 0;
                        forceTurn = true;
                    }
                    else
                    {
                        turnTime += Time.deltaTime;
                    }
                }
                else if(turnStyle == TurnStyle.TurnByTrigger)
                {

                }

                if (forceTurn)
                {
                    direction *= -1;
                    enemyState = EnemyState.Detecting;
                }

                ec.Move(direction * speed, false, false);

                break;

            case (EnemyState.Detecting):
                print("in detecting");

                detectTime += Time.deltaTime;
                if(detectTime > detectTimer)
                {
                    enemyState = EnemyState.Idle;
                }

                break;

            case (EnemyState.Attacking):
                //print("in attacking");
                float distanceFromPlayer = Mathf.Abs((transform.position - player.transform.position).magnitude);
                float directionPlayerIsIn = -Mathf.Sign((transform.position - player.transform.position).magnitude);

                if (hitTarget)
                {
                    hitTarget = false;
                    enemyState = EnemyState.Resting;
                }
                
                

                //print(directionPlayerIsIn);

                if (distanceFromPlayer < attackRange)
                {
                    // can attack
                    anim.SetTrigger("Attacking");

                    if (directionPlayerIsIn == direction)
                    {
                        // player is right
                        if (ec.m_FacingRight)
                        {
                            //UseAttack(spellIndex.tripleFire);
                            enemyState = EnemyState.Resting;
                        }
                    }
                    else if(directionPlayerIsIn != direction)
                    {
                        // player is left
                        if (!ec.m_FacingRight)
                        {
                            //UseAttack(spellIndex.tripleFire);
                            enemyState = EnemyState.Resting;
                        }
                    }
                    
                }
                else
                {
                    // moves towards player
                    enemyState = EnemyState.Walking;
                    ec.Move(directionPlayerIsIn * (walkToPlayerSpeed * 2) * Time.deltaTime, false, false);
                }

                break;
            case (EnemyState.Resting):
                print("in resting");
                anim.SetTrigger("Idle");
                if (restTimerSet != true)
                {
                    restTime = 0;
                    restTimer = Random.Range(0.5f, 2f);
                    restTimerSet = true;
                }

                if (restTime > restTimer)
                {
                    //hit = false;
                    restTimerSet = false;
                    enemyState = EnemyState.Attacking;
                }

                restTime += Time.deltaTime;

                break;
            case (EnemyState.Attacked):
                print("in attacked");
                if (hitTimerSet != true)
                {
                    hitTime = 0;

                    if (hitTimer == 0)
                    {
                        hitTimer = Random.Range(0.1f, 0.5f);
                    }

                    hitTimerSet = true;
                }

                if (hitTime > hitTimer)
                {
                    hit = false;
                    hitTimerSet = false;
                    enemyState = EnemyState.Attacking;
                }

                hitTime += Time.deltaTime;

                break;
        }


        if(player != null)
        {
            if (Vector2.Distance(transform.position, player.transform.position) < attackRange)
            {
                if (enemyState == EnemyState.Idle || enemyState == EnemyState.Detecting || enemyState == EnemyState.Walking || enemyState != EnemyState.Resting && resting == false)
                {
                    //enemyState = EnemyState.Attacking;
                    
                }
            }
        }

        if(sightRange != null)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, sightBox.bounds.size, 0, ThisIsPlayer);
            if (colliders.Length > 0)
            {
                print("ThisAttacking");
                playerInRange = true;
                direction = (int)Mathf.Sign(transform.position.x - colliders[0].gameObject.transform.position.x);
                enemyState = EnemyState.Attacking;
            }
        }
        
        

        if (enemyState != EnemyState.Resting)
        {
            resting = false;
        }

        if (health.currentHealth <= 0) {
            isDead = true;


        }

        if (isDead)
        {
			
			print("enemy is dead");

            if (health.enabled == true)
            {
                print(deathParticle);
                GameObject part = Instantiate(deathParticle, transform.position, Quaternion.identity);
                //GameObject DeathParticle = Instantiate(deathParticle) as GameObject;
                health.enabled = false;
            }

            transform.position = new Vector3(0, 3000, 0);

            if (respawnEnabled)
            {
                respawnCalled = true;
            }
        }
        if (respawnCalled) {
            Respawn();
           
        }

        if (hit)
        {
            hitTimerSet = false;
            enemyState = EnemyState.Attacked;
        }

        

    }

    void Respawn() {
        if (!timerSet) {
            timer = Time.deltaTime;
            timeLimit = Time.deltaTime + 5f;
            timerSet = true;
        }

        if (timer > timeLimit && !spr.isVisible) {
            print("respawn");
            health.enabled = true;
            rb2d.velocity = Vector2.zero;
            health.currentHealth = health.maxHealth;
            isDead = false;
            timerSet = false;
            respawnCalled = false;
            transform.position = startingPoint;
        }
        else {
            //print ("timer: " + timer);
            timer += Time.deltaTime;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 1);
        Gizmos.DrawSphere(edgeDetectionBack.position, 0.1f);

        Gizmos.color = new Color(1, 0, 1);
        Gizmos.DrawSphere(edgeDetectionFront.position, 0.1f);

        Gizmos.DrawCube(transform.position, sightBox.bounds.size);
        //Gizmos.DrawCube(transform.position, new Vector2(attackRange, attackRange));
    }

   
    

    public virtual void UseAttack(Attack attack)
    {
        

        if (enemyState == EnemyState.Attacking && attack != null)
        {
            if (attack.attackType == Attack.AttackType.Special)
            {
                // stop time for attack length
            }
            else if (attack.attackType == Attack.AttackType.Blast)
            {
                print("casting attack");
                CastProjectile(attack);
            }
            else if (attack.attackType == Attack.AttackType.MultipleBlast)
            {
                StartCoroutine(MultiCast(attack));
            }
            else if (attack.attackType == Attack.AttackType.Melee)
            {

            }
            else if (attack.attackType == Attack.AttackType.Beam)
            {

            }
        }
    }

    public void CastProjectile(Attack projectile)
    {
        print("cast projectile");
        GameObject projectileObject = new GameObject("projectile");
        AttackScript spell = projectileObject.AddComponent<AttackScript>();
        spell.flipped = !ec.m_FacingRight;
        spell.attack = projectile;
        spell.origin = spellCastPoint.position;
        spell.direction = ec.m_FacingRight ? 1 : -1;
        spell.user = gameObject.name;
    }

    IEnumerator MultiCast(Attack projectile)
    {

        for (int i = 0; i < projectile.multiFireCount; i++)
        {
            yield return new WaitForSeconds(projectile.multiFireRate);
            CastProjectile(projectile);
            yield return new WaitForSeconds(projectile.multiFireRate);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Attack")
        {
            hit = true;
        }
    }

    public void DisableAttackHitbox()
    {
        AttackHitbox.SetActive(false);
    }
}
