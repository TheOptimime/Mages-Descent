﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {
    // This script is attatched to attacks like projectiles

    //public Vector2 speed;

    public Attack attack;
    [HideInInspector] public string user;

    Rigidbody2D rb;

    [HideInInspector] public float delay;
    public bool startDelayPassed;
    public bool activatedByPlayer;

    Collider2D col;
    BoxCollider2D boxCol;

    int interactionCount;

    SpriteRenderer sr;
    Sprite[] sprites;

    float frequency = 20, magnitude = 0.5f;

    [HideInInspector] public Vector2 origin;
    [HideInInspector] public int direction;
    [HideInInspector] public bool flipped;

    [HideInInspector] public float time;

    public bool StartNextAttackOnDestroy;

    int castingPlayer;
    float boxColliderSize = 0.01f;

    public GameObject followUpAttack;

    public Fighter usingFighter;


    void Start()
    {
        transform.position = origin;
        gameObject.tag = "Attack";

        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.gravityScale = 0;
        
        if(boxCol = GetComponentInChildren<BoxCollider2D>())
        {
            print("Box Collider Present");
        }

        if (attack.spriteAnimation != null)
        {
            GameObject sprite = Instantiate(attack.spriteAnimation, transform.position, Quaternion.identity,this.gameObject.transform);
            if(direction < 0)
            {
                // facing left
                sprite.transform.localScale = MathC.MultiplyVector(sprite.transform.localScale, MathC.NegaVectorX);
            }

            if (attack.attackBase != null)
            {
                // Checks for a 2D Collider in the attack base prefab
                if (sprite.GetComponent<Collider2D>())
                {
                    // Adds the attack collision script and links the attack script to it
                    AttackCollision _ac = sprite.AddComponent<AttackCollision>();
                    _ac._as = this;
                }
            }

            if (attack.attackPath == Attack.AttackPath.Meteor)
            {
                sprite.transform.Rotate(new Vector3(0, 0, -45));
            }
            else if(attack.attackPath == Attack.AttackPath.CrashDown)
            {
                sprite.transform.Rotate(new Vector3(0, 0, -90));
            }
        }
        
        

        if (col != null)
        {
            col.isTrigger = true;
        }

        if(attack.attackType == Attack.AttackType.Beam)
        {
            if (boxCol != null)
            {
                boxCol.isTrigger = true;
            }
        }
    }
    
    void Update()
    {
        time += Time.deltaTime;

        if(time > delay && startDelayPassed == false  && attack.hasSpecialChargeFunction != true || activatedByPlayer && startDelayPassed == false)
        {
            startDelayPassed = true;
            time = 0;
        }

        if (startDelayPassed || activatedByPlayer)
        {

            if (time > attack.lifetime)
            {
                // destroy this object
                if (attack.lifetime <= 0)
                {
                    if (StartNextAttackOnDestroy)
                        StartNextAttack(followUpAttack);

                    Destroy(gameObject);
                }
                else if (time > attack.lifetime)
                {
                    if (StartNextAttackOnDestroy)
                        StartNextAttack(followUpAttack);

                    Destroy(gameObject);
                }

            }

            if (attack.attackType == Attack.AttackType.Beam)
            {
                boxColliderSize += Time.deltaTime / 4;
                boxCol.size = new Vector2(boxColliderSize, boxCol.size.y);
            }
        }
            
    }

    private void FixedUpdate()
    {
        if (startDelayPassed || activatedByPlayer)
        {
            if (attack != null)
            {
                if (attack.attackType == Attack.AttackType.Blast || attack.attackType == Attack.AttackType.MultipleBlast)
                {
                    if (attack.attackPath == Attack.AttackPath.Straight)
                    {
                        rb.velocity = new Vector2(attack.speed * direction, 0);
                    }
                    else if (attack.attackPath == Attack.AttackPath.Meteor)
                    {
                        rb.velocity = new Vector2(attack.speed * direction, -attack.speed);
                    }
                    else if (attack.attackPath == Attack.AttackPath.CrashDown)
                    {
                        rb.velocity = new Vector2(0, -attack.speed);
                    }
                    else if (attack.attackPath == Attack.AttackPath.SineWave)
                    {
                        rb.velocity = new Vector2(attack.speed * direction, Mathf.Sin(Time.time * frequency) * magnitude);
                    }
                    else if(attack.attackPath == Attack.AttackPath.Curved)
                    {
                        rb.velocity = new Vector2(Mathf.Abs(-0.7f * (7.6f + transform.position.y + 4.3f) * attack.speed * Time.fixedDeltaTime) * direction, (0.7f * ((transform.position.x * transform.position.x) + 7.6f * transform.position.x + 4.3f) * attack.speed)/10 * Time.fixedDeltaTime);
                    }
                    else if(attack.attackPath == Attack.AttackPath.Homing)
                    {
                        if(usingFighter != null)
                        {
                            rb.velocity = (Vector2.MoveTowards(transform.position, usingFighter.transform.position, attack.speed * Time.fixedDeltaTime));
                            print(rb.velocity);

                            // rb.velocity = new Vector2(transform.position.x - usingFighter.position.x, transform.position.y - usingFighter.position.y);
                        }

                    }
                }


            }
        }

    }

    

    public void StartNextAttack(GameObject _attack)
    {

        if (attack.followUpAttack != null)
        {
            // So the followUpAttack is just the base prefab
            AttackScript _as;

            if (followUpAttack == null)
            {
                followUpAttack = new GameObject("Follow-Up Attack");
            }
            

            if (_as = followUpAttack.GetComponent<AttackScript>())
            {
                _as.origin = transform.position;
                _as.attack = attack.followUpAttack;

                _as.flipped = flipped;
                _as.direction = direction;
                _as.usingFighter = usingFighter;
            }
            else
            {
                _as = followUpAttack.AddComponent<AttackScript>();
                _as.origin = transform.position;
                _as.attack = attack.followUpAttack;

                _as.flipped = flipped;
                _as.direction = direction;
                _as.usingFighter = usingFighter;
            }

            
        }
    }
}
