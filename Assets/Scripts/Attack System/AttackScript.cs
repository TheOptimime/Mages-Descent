using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {
    // This script is attatched to attacks like projectiles

    //public Vector2 speed;

    public Attack attack;
    [HideInInspector] public string user;

    Rigidbody2D rb;

    [HideInInspector] public float delay;

    Collider2D col;

    SpriteRenderer sr;
    Sprite[] sprites;

    [HideInInspector] public Vector2 origin;
    [HideInInspector] public int direction;
    [HideInInspector] public bool flipped;

    [HideInInspector] public float time;

    int castingPlayer;

    public GameObject followUpAttack;


    void Start()
    {
        transform.position = origin;
        gameObject.tag = "Attack";

        rb = gameObject.AddComponent<Rigidbody2D>();
        sr = gameObject.AddComponent<SpriteRenderer>();
        
        //sr.sprite = testSprite;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        sr.transform.localScale += new Vector3(3,3,3);
        

        sr.sprite = Resources.Load<Sprite>(attack.spritePath);
        sr.sortingOrder = 8;

        rb.gravityScale = 0;

        if(col != null)
        {
            col.isTrigger = true;
        }
        else
        {

        }
        

        print("attack init complete");

        
        
    }
    
    void Update()
    {
        time += Time.deltaTime;
        
            sr.flipX = flipped;
        

        
        if(sr.sprite == null)
        {
            //sr.sprite = testSprite;
        }

        
        //print(gameObject.name);

        if (time > attack.lifetime)
        {
            // destroy this object
            if(attack.lifetime <= 0)
            {
                StartNextAttack(followUpAttack);
                Destroy(gameObject);
            }
            else if (time > attack.lifetime)
            {
                StartNextAttack(followUpAttack);
                Destroy(gameObject);
            }

        }

        
    }

    private void FixedUpdate()
    {
        if (attack != null)
        {
            if(attack.attackType == Attack.AttackType.Blast || attack.attackType == Attack.AttackType.MultipleBlast)
            {
                rb.velocity = new Vector2(attack.speed * direction, 0);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Most likely going to make a tag for interactable ojbects

        if (other.transform.tag == "Enemy" && other.gameObject.name != user || other.transform.tag == "Player" && other.gameObject.name != user)
        {
            
            other.gameObject.GetComponent<Health>().Damage(attack.damage);
            
                KnockbackListener knockbackScript = other.gameObject.GetComponent<KnockbackListener>();
                knockbackScript.SetHitstun(attack.hitStun);
                Vector2 finalKnockback = attack.knockback;
                finalKnockback.x *= direction;
                knockbackScript.SetKnockback(finalKnockback);

            // Get direction of the impact relative to the player/ai and flip accordingly 
            if(transform.position.x < other.transform.position.x)
            {
                if(other.transform.tag == "Player")
                {
                    Fighter _player = other.gameObject.GetComponent<Fighter>();

                    if(_player == null)
                    {
                        PlayerAI _ai = other.gameObject.GetComponent<PlayerAI>();
                        if (!_ai.cc.m_FacingRight) _ai.cc.Flip();
                    }
                    else
                    {
                        if (!_player.cc.m_FacingRight) _player.cc.Flip();
                    }
                }
                else if(other.transform.tag == "Enemy")
                {
                    EnemyAI _enemy = other.gameObject.GetComponent<EnemyAI>();
                    if (!_enemy.ec.m_FacingRight) _enemy.ec.Flip();
                }
            }
            else if(transform.position.x > other.transform.position.x)
            {
                if (other.transform.tag == "Player")
                {
                    Fighter _player = other.gameObject.GetComponent<Fighter>();

                    if (_player == null)
                    {
                        PlayerAI _ai = other.gameObject.GetComponent<PlayerAI>();
                        if (_ai.cc.m_FacingRight) _ai.cc.Flip();
                    }
                    else
                    {
                        if (_player.cc.m_FacingRight) _player.cc.Flip();
                    }
                }
                else if (other.transform.tag == "Enemy")
                {
                    EnemyAI _enemy = other.gameObject.GetComponent<EnemyAI>();
                    if (_enemy.ec.m_FacingRight) _enemy.ec.Flip();
                }
            }

            if (attack.attackType == Attack.AttackType.Blast)
            {
                if (attack.followUpAttack != null)
                {
                    // check if attack is burst/aoe
                    if(attack.elementEffect == Attack.ElementEffect.Burst)
                    {
                        StartNextAttack(followUpAttack);
                    }
                }
            }

            Destroy(gameObject, attack.destroyTime);

        }
        else if(other.transform.tag == "Ground" || other.transform.tag == "Wall")
        {
            if(attack.attackType == Attack.AttackType.Blast)
            {
                if(attack.followUpAttack != null)
                {
                    if(attack.elementEffect == Attack.ElementEffect.Burst)
                    {
                        StartNextAttack(followUpAttack);
                    }
                    else if(attack.followUpType == Attack.FollowUpType.Auto)
                    {
                        // Automatically Starts Next Attack
                        StartNextAttack(followUpAttack);
                    }
                    else if(attack.followUpType == Attack.FollowUpType.Command)
                    {
                        // Sends a flag back to the caster to add the follow up attack in the attack queue
                    }
                }
            }
            Destroy(gameObject, attack.destroyTime);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.tag == "Enemy" && other.gameObject.name != user || other.transform.tag == "Player" && other.gameObject.name != user)
        {
            // deal damage at a rate [if(time % rate)]
            other.gameObject.GetComponent<Health>().Damage(attack.damage);

            KnockbackListener knockbackScript = other.gameObject.GetComponent<KnockbackListener>();
            knockbackScript.SetHitstun(attack.hitStun);
            Vector2 finalKnockback = attack.knockback;
            finalKnockback.x *= direction;
            knockbackScript.SetKnockback(finalKnockback);

            // Get direction of the impact relative to the player/ai


            if (attack.attackType == Attack.AttackType.Beam)
            {
                if (attack.followUpAttack != null)
                {
                    // check if attack is burst/aoe
                    
                }
            }

            Destroy(gameObject, attack.destroyTime);

        }
        else if (other.transform.tag == "Ground" || other.transform.tag == "Wall")
        {
            if (attack.attackType == Attack.AttackType.Blast)
            {
                if (attack.followUpAttack != null)
                {
                    if (attack.elementEffect == Attack.ElementEffect.Burst)
                    {
                        StartNextAttack(followUpAttack);
                    }
                    else if (attack.followUpType == Attack.FollowUpType.Auto)
                    {
                        // Automatically Starts Next Attack
                    }
                    else if (attack.followUpType == Attack.FollowUpType.Command)
                    {
                        // Sends a flag back to the caster to add the follow up attack in the attack queue
                    }
                }
            }
            Destroy(gameObject, attack.destroyTime);
        }
    }

    void StartNextAttack(GameObject _attack)
    {

        if (attack.followUpAttack != null)
        {
            // So the followUpAttack is just the base prefab
            AttackScript _as;

            if (followUpAttack == null)
            {
                followUpAttack = new GameObject();
            }

            if (_as = followUpAttack.GetComponent<AttackScript>())
            {
                _as.attack = attack.followUpAttack;
            }
            else
            {
                _as = followUpAttack.AddComponent<AttackScript>();
                _as.attack = attack.followUpAttack;
            }
        }
    }
}
