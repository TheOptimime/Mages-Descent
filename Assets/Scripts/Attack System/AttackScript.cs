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
    public bool startDelayPassed;
    public bool activatedByPlayer;

    Collider2D col;
    BoxCollider2D boxCol;

    int interactionCount;

    SpriteRenderer sr;
    Sprite[] sprites;

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
        sr = gameObject.AddComponent<SpriteRenderer>();
        
        //sr.sprite = testSprite;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        //sr.transform.localScale += new Vector3(3,3,3);
        

        //sr.sprite = Resources.Load<Sprite>(attack.spritePath);
        //sr.sortingOrder = 8;

        rb.gravityScale = 0;

        
        if(col != null)
        {
            col.isTrigger = true;
        }

        if(boxCol = GetComponent<BoxCollider2D>())
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
            
            if(attack.attackPath == Attack.AttackPath.Meteor)
            {
                sprite.transform.Rotate(new Vector3(0, 45, 0));
            }
            else if(attack.attackPath == Attack.AttackPath.CrashDown)
            {
                sprite.transform.Rotate(new Vector3(0, 90, 0));
            }
        }
        

        print("attack init complete");

        
        
    }
    
    void Update()
    {
        time += Time.deltaTime;

        if(time > delay && startDelayPassed == false || activatedByPlayer && startDelayPassed == false)
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
                        rb.velocity = new Vector2(attack.speed * direction, attack.speed * -direction);
                    }
                    else if (attack.attackPath == Attack.AttackPath.CrashDown)
                    {
                        rb.velocity = new Vector2(0, attack.speed);
                    }
                    else if (attack.attackPath == Attack.AttackPath.SineWave)
                    {
                        rb.velocity = new Vector2(attack.speed * direction, attack.speed * Mathf.Sin(Time.deltaTime));
                    }
                    else if(attack.attackPath == Attack.AttackPath.Curved)
                    {
                        rb.velocity = new Vector2(-0.7f * (7.6f + rb.velocity.y + 4.3f) * attack.speed, 0.7f * ((rb.velocity.x * rb.velocity.x) + 7.6f * rb.velocity.x + 4.3f) * -1 * attack.speed);
                    }
                    else if(attack.attackPath == Attack.AttackPath.Homing)
                    {
                        if(usingFighter != null)
                        {
                            rb.velocity = (Vector2.MoveTowards(transform.position, usingFighter.transform.position, Vector2.Distance(transform.position, usingFighter.transform.position))) * attack.speed;
                        }
                        
                    }
                }


            }
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Most likely going to make a tag for interactable ojbects
        if (startDelayPassed || activatedByPlayer)
        {
            if (other.transform.tag == "Enemy" && other.gameObject.name != user || other.transform.tag == "Player" && other.gameObject.name != user)
            {

                other.gameObject.GetComponent<Health>().Damage(attack.damage);

                KnockbackListener knockbackScript = other.gameObject.GetComponent<KnockbackListener>();
                knockbackScript.SetHitstun(attack.hitStun);
                Vector2 finalKnockback = attack.knockback;
                finalKnockback.x *= direction;
                knockbackScript.SetKnockback(finalKnockback);

                if (other.transform.tag == "Enemy")
                {
                    EnemyAI _enemy = other.gameObject.GetComponent<EnemyAI>();

                    _enemy.murderMeter += Mathf.Round(attack.damage / 4);

                    if (_enemy.murderMeter >= _enemy.murderMeterLimit)
                    {
                        if (attack.element == Attack.Element.Fire)
                        {
                            _enemy.ailmentHandler.ailment = AilmentHandler.Ailments.Burned;
                        }
                        else if (attack.element == Attack.Element.Ice)
                        {
                            _enemy.ailmentHandler.ailment = AilmentHandler.Ailments.Frozen;
                        }
                        else if (attack.element == Attack.Element.Blood)
                        {
                            _enemy.ailmentHandler.ailment = AilmentHandler.Ailments.Bleeding;
                            GameObject.Find(user).GetComponent<Fighter>().health.currentHealth += Mathf.Round(_enemy.murderMeterLimit / 2);
                        }
                        else if (attack.element == Attack.Element.Thunder)
                        {
                            _enemy.ailmentHandler.ailment = AilmentHandler.Ailments.Stunned;
                        }
                        else if (attack.element == Attack.Element.Arcane)
                        {
                            // Shut up, poisoned is a placeholder
                            _enemy.ailmentHandler.ailment = AilmentHandler.Ailments.Poisoned;
                        }

                        _enemy.murderMeter = 0;
                    }
                }

                // Get direction of the impact relative to the player/ai and flip accordingly 
                if (transform.position.x < other.transform.position.x)
                {
                    if (other.transform.tag == "Player")
                    {
                        Fighter _player = other.gameObject.GetComponent<Fighter>();

                        if(usingFighter != _player)
                        {
                            if (_player == null)
                            {
                                PlayerAI _ai = other.gameObject.GetComponent<PlayerAI>();
                                if (!_ai.cc.m_FacingRight) _ai.cc.Flip();
                            }
                            else
                            {
                                if (!_player.cc.m_FacingRight) _player.cc.Flip();
                            }
                        }
                        
                    }
                    else if (other.transform.tag == "Enemy")
                    {
                        EnemyAI _enemy = other.gameObject.GetComponent<EnemyAI>();
                        if (!_enemy.ec.m_FacingRight) _enemy.ec.Flip();
                    }
                }
                else if (transform.position.x > other.transform.position.x)
                {
                    if (other.transform.tag == "Player")
                    {
                        Fighter _player = other.gameObject.GetComponent<Fighter>();

                        if(usingFighter != _player)
                        {
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
                        else if(usingFighter == _player && attack.attackPath == Attack.AttackPath.Homing)
                        {
                            Destroy(this.gameObject);
                        }
                    }

                        
                }

                if (attack.attackType == Attack.AttackType.Blast)
                {
                    if (attack.followUpAttack != null)
                    {
                        // check if attack is burst/aoe
                        if (attack.elementEffect == Attack.ElementEffect.Burst)
                        {
                            StartNextAttack(followUpAttack);
                        }
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
                            StartNextAttack(followUpAttack);
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
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (startDelayPassed || activatedByPlayer)
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
