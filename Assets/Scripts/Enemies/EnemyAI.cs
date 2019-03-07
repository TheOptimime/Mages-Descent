using System.Collections;
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


    public bool respawnEnabled;

    public bool stunned;


    public float attackRange, hitTimer;
    bool timerSet, idleTimerSet, hitTimerSet, restTimerSet;
    public bool hit;
    float timeLimit;

    bool isDead, respawnCalled, resting;
    bool hasRangedAttack;




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


    void Update() {

        UpdateEdgeDetection();

		if (enemyState == EnemyState.Walking) {
			anim.SetBool ("isWalking", true);
		} else  {
			anim.SetBool ("isWalking", false);
		}
			
        switch (enemyState)
        {
            case (EnemyState.Idle):
                print("in idle");
                if (idleTimerSet != true)
                {
                    idleTime = 0;
                    idleTimer = Random.Range(2.5f, 7f);
                    idleTimerSet = true;
                }


                if (edgeDetected)
                {
                    // wait
                    if (idleTime > idleTimer)
                    {
                        print("Turn Around");
                        direction *= -1;
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
			print ("in walking");
                

			if (!edgeDetected || ignoreEdgeDetection) {
//                    print("should be walking" + direction * walkSpeed * Time.deltaTime);
				ec.Move (direction * speed * Time.deltaTime, false, false);
				ignoreEdgeDetection = false;
					
				}
                else
                {
                    print("edge detected");
                    enemyState = EnemyState.Idle;
                }
                break;
            case (EnemyState.Detecting):
                print("in detecting");
                break;
            case (EnemyState.Attacking):
                print("in attacking");
                float distanceFromPlayer = Mathf.Abs((transform.position - player.transform.position).magnitude);
                float directionPlayerIsIn = -Mathf.Sign((transform.position - player.transform.position).magnitude);

                //print("distance: " + distanceFromPlayer);
                //print("attack range: " + attackRange);

                print(directionPlayerIsIn);

                if (distanceFromPlayer < attackRange)
                {
                    // can attack

                    if(directionPlayerIsIn == 1)
                    {
                        // player is right
                        if (ec.m_FacingRight)
                        {
                            //UseAttack(spellIndex.tripleFire);
                            enemyState = EnemyState.Resting;
                        }
                    }
                    else if(directionPlayerIsIn == -1)
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
                    ec.Move(directionPlayerIsIn * walkToPlayerSpeed * Time.deltaTime, false, false);
                }

                break;
            case (EnemyState.Resting):
                print("in resting");

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
                        hitTimer = Random.Range(0.5f, 2f);
                    }

                    hitTimerSet = true;
                }

                if (hitTime > hitTimer)
                {
                    hit = false;
                    enemyState = EnemyState.Attacking;
                }

                hitTime += Time.deltaTime;

                break;
        }


        if(player != null)
        {
            if ((transform.position - player.transform.position).magnitude < attackRange)
            {
                if (enemyState == EnemyState.Idle || enemyState == EnemyState.Detecting || enemyState == EnemyState.Walking || enemyState != EnemyState.Resting && resting == false)
                {
                    enemyState = EnemyState.Attacking;
                }
            }
        }
        

        if (enemyState != EnemyState.Resting)
        {
            resting = false;
        }

        if (health.currentHealth <= 0) {
            isDead = true;


        }

        if (isDead) {
			
			print("enemy is dead");
			//Instantiate (deathParticle, transform.position, Quaternion.identity);


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

        //Gizmos.DrawCube(transform.position, new Vector2(attackRange, attackRange));
    }

   
    

    public void UseAttack(Attack attack)
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


}
