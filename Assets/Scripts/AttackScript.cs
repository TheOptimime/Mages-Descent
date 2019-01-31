using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {
    // This script is attatched to attacks like projectiles

    //public Vector2 speed;

    public Attack attack;
    public string user;
    Rigidbody2D rb;
    public float delay;

    //public Sprite[] projectileSprites;
    public Sprite testSprite;
    CircleCollider2D col;

    SpriteRenderer sr;
    Sprite[] sprites;

    public Vector2 origin;
    public int direction;
    public bool flipped;

    public float time;

    int castingPlayer;
    



    void Start()
    {
        transform.position = origin;
        gameObject.tag = "Attack";

        rb = gameObject.AddComponent<Rigidbody2D>();
        col = gameObject.AddComponent<CircleCollider2D>();
        sr = gameObject.AddComponent<SpriteRenderer>();
        
        sr.sprite = testSprite;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        sr.transform.localScale += new Vector3(3,3,3);
        
        
        // Load Sprites 

        // sprites = Resources.LoadAll<Sprite>("fireball.png");
        // testSprite = Resources.Load<Sprite>(attack.spritePath);

        sr.sprite = Resources.Load<Sprite>(attack.spritePath);
        sr.sortingOrder = 8;

        rb.gravityScale = 0;

        col.isTrigger = true;

        print("attack init complete");
       
    }
    
    void Update()
    {
        time += Time.deltaTime;
        
            sr.flipX = flipped;
        

        
        if(sr.sprite == null)
        {
            sr.sprite = testSprite;
        }

        if(attack.name == "Triple Fire")
        {
            for(int i = 0; i < sprites.Length; i++)
            {
                sr.sprite = sprites[i];
            }
            
            
        }
        else if(attack.name == "Dark Fire")
        {
            sr.color = new Color(0.28f, 0f, 0.28f);
        }
        else if (attack.name == "Dab Ice")
        {
            sr.color = new Color(0.16f, 0.2f, 0.56f);
        }


        //print(gameObject.name);

        if (time > attack.lifetime)
        {
            // destroy this object
            if(attack.lifetime == 0)
            {
                Destroy(gameObject);
            }
            else if (time > attack.lifetime)
            {
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
                        StartNextAttack(attack.followUpAttack);
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
                        StartNextAttack(attack.followUpAttack);
                    }
                    else if(attack.followUpType == Attack.FollowUpType.Auto)
                    {
                        // Automatically Starts Next Attack
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
                        StartNextAttack(attack.followUpAttack);
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

    void StartNextAttack(Attack _attack)
    {

    }
}
