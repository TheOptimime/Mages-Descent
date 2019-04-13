using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    public AttackScript _as;
	public GameObject pillarParticle; 

    void OnTriggerEnter2D(Collider2D other)
    {
        // Most likely going to make a tag for interactable ojbects
        if (_as.startDelayPassed || _as.activatedByPlayer)
        {
            if (_as.attack.attackPath == Attack.AttackPath.Homing && _as.attack.hasSpecialChargeFunction)
            {
                Fighter temp;

                if (temp = other.gameObject.GetComponent<Fighter>())
                {
                    if (temp != null)
                    {
                        if (temp == _as.usingFighter)
                        {
                            Destroy(gameObject);
                        }
                    }
                }
            }

            if (other.transform.tag == "Enemy" && other.gameObject.name != _as.user || other.transform.tag == "Player" && other.gameObject.name != _as.user)
            {
                other.gameObject.GetComponent<Health>().Damage(_as.attack.damage);

                if (other.transform.tag == "Player")
                {
                    other.gameObject.GetComponent<Fighter>().recentlyAttacked = true;
                }

                KnockbackListener knockbackScript = other.gameObject.GetComponent<KnockbackListener>();
                Vector2 finalKnockback = _as.attack.knockback;

                knockbackScript.SetHitstun(_as.attack.hitStun);
                finalKnockback.x *= _as.direction;
                knockbackScript.SetKnockback(finalKnockback);

                if (other.transform.tag == "Enemy")
                {
                    EnemyAI _enemy = other.gameObject.GetComponent<EnemyAI>();
                    if (_as.usingFighter.vibrationSphere.bounds.Contains(_enemy.transform.position))
                    {
                        float distance = _as.usingFighter.vibrationSphere.transform.position.x - _enemy.transform.position.x;

                        if(distance > 0)
                        {
                            _as.usingFighter.ShakeCamera(0.5f, Mathf.Abs(Random.Range(0, _as.attack.damage / 2) - Mathf.Abs(distance/2)));
                            _as.usingFighter.SetVibration(Mathf.Abs(Random.Range(0, _as.attack.damage / 3) - Mathf.Abs(distance/2)), Mathf.Abs(Random.Range(0, _as.attack.damage / 2) - Mathf.Abs(distance)));
                        }
                        else if(distance < 0)
                        {
                            _as.usingFighter.ShakeCamera(0.5f, Mathf.Abs(Random.Range(0, _as.attack.damage / 2) - Mathf.Abs(distance/2)));
                            _as.usingFighter.SetVibration(Mathf.Abs(Random.Range(0, _as.attack.damage / 2) - Mathf.Abs(distance/2)), Mathf.Abs(Random.Range(0, _as.attack.damage / 3) - Mathf.Abs(distance)));
                        }
                        else
                        {
                            //_as.usingFighter.ShakeCamera(0, 0);
                            //_as.usingFighter.SetVibration(0, 0);
                        }
                    }

                    _enemy.murderMeter += Mathf.Round(_as.attack.damage / 4);

                    if (_enemy.murderMeter >= _enemy.murderMeterLimit)
                    {
                        if (_as.attack.element == Attack.Element.Fire)
                        {
                            _enemy.ailmentHandler.ailment = AilmentHandler.Ailments.Burned;
                        }
                        else if (_as.attack.element == Attack.Element.Ice)
                        {
                            _enemy.ailmentHandler.ailment = AilmentHandler.Ailments.Frozen;
                        }
                        else if (_as.attack.element == Attack.Element.Blood)
                        {
                            _enemy.ailmentHandler.ailment = AilmentHandler.Ailments.Bleeding;
                            _as.usingFighter.health.currentHealth += Mathf.Round(_enemy.murderMeterLimit / 2);
                        }
                        else if (_as.attack.element == Attack.Element.Thunder)
                        {
                            _enemy.ailmentHandler.ailment = AilmentHandler.Ailments.Stunned;
                        }
                        else if (_as.attack.element == Attack.Element.Arcane)
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

                        if (_as.usingFighter != _player)
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
                        if (!_enemy.ec.m_FacingRight) _enemy.direction *= -1;
                    }
                }
                else if (transform.position.x > other.transform.position.x)
                {
                    if (other.transform.tag == "Player")
                    {
                        Fighter _player = other.gameObject.GetComponent<Fighter>();

                        if (_as.usingFighter != _player)
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
                        else if (_as.usingFighter == _player && _as.attack.attackPath == Attack.AttackPath.Homing)
                        {
                            Destroy(this.gameObject);
                        }
                    }


                }

                if (_as.attack.attackType == Attack.AttackType.Blast)
                {
                    if (_as.attack.followUpAttack != null)
                    {
                        // check if attack is burst/aoe
                        if (_as.attack.elementEffect == Attack.ElementEffect.Burst)
                        {
                            _as.StartNextAttack(_as.followUpAttack);
                        }
                    }
                }

                _as.usingFighter.IncrementComboChain(_as.attack);
                _as.usingFighter.dabometer += Mathf.Abs(((_as.attack.damage + _as.attack.lifetime - _as.time)/2) + _as.usingFighter.comboCount);

                Destroy(gameObject, _as.attack.destroyTime);

            }
            else if (other.transform.tag == "Ground" || other.transform.tag == "Wall")
            {
                if (!_as.attack.bounces || _as.bounceCount > _as.attack.bounceCount)
                {
                    if (_as.followUpAttack != null)
                    {
                        if (_as.attack.attackPath == Attack.AttackPath.Meteor)
                        {
                            if (other.transform.tag == "Ground")
                            {
                                _as.StartNextAttack(_as.followUpAttack);

                            }
                            else if (_as.attack.followUpAttack.attackType != Attack.AttackType.Blast)
                            {
                                _as.StartNextAttack(_as.followUpAttack);
                            }
                            Destroy(this.gameObject);
                            print("destroy");

                            if (pillarParticle != null)
                                Instantiate(pillarParticle, transform.position, Quaternion.identity);

                        }
                    }


                    if (_as.attack.attackType == Attack.AttackType.Blast)
                    {
                        if (_as.attack.followUpAttack != null)
                        {
                            if (_as.attack.elementEffect == Attack.ElementEffect.Burst)
                            {
                                _as.StartNextAttack(_as.followUpAttack);
                            }
                            else if (_as.attack.followUpType == Attack.FollowUpType.Auto)
                            {
                                // Automatically Starts Next Attack
                                _as.StartNextAttack(_as.followUpAttack);
                            }
                            //Destroy(this.gameObject);
                        }
                    }
                }
                else if (_as.attack.bounces)
                {
                    if (_as.attack.attackPath == Attack.AttackPath.Meteor)
                    {
                        if (other.transform.tag == "Ground")
                        {
                            _as.ySpeed = -(_as.ySpeed / 3);
                            _as.xSpeed = Mathf.Abs(_as.xSpeed / 3);
                            _as.rb.gravityScale = _as.bounceCount;

                            _as.bounceCount++;
                            
                        }
                        else if (_as.attack.followUpAttack.attackType != Attack.AttackType.Blast)
                        {
                            _as.direction *= -1;
                            //_as.xSpeed = _as.attack.speed * _as.bounceCount;

                            _as.bounceCount++;
                        }


                        if (pillarParticle != null)
                        {
                            // Whatever this is
                            Instantiate(pillarParticle, transform.position, Quaternion.identity);
                        }
                    }
                    


                    if (_as.attack.attackType == Attack.AttackType.Blast)
                    {
                        if (_as.attack.followUpAttack != null)
                        {
                            if (_as.attack.elementEffect == Attack.ElementEffect.Burst)
                            {
                                _as.StartNextAttack(_as.followUpAttack);
                            }
                            else if (_as.attack.followUpType == Attack.FollowUpType.Auto)
                            {
                                // Automatically Starts Next Attack
                                _as.StartNextAttack(_as.followUpAttack);
                            }
                            //Destroy(this.gameObject);
                        }
                        else
                        {

                        }
                    }
                }
                

                if (_as.usingFighter.vibrationSphere.bounds.Contains(transform.position))
                {
                    float distance = _as.usingFighter.vibrationSphere.transform.position.x - transform.position.x;

                    if (distance > 0)
                    {
                        _as.usingFighter.ShakeCamera(0.5f, Mathf.Abs(Random.Range(0, (_as.attack.damage / 4) - (_as.attack.hitStun.cancelTime / 2)) - Mathf.Abs(distance / 2)));
                        _as.usingFighter.SetVibration((Mathf.Abs(Random.Range(0, (_as.attack.damage / 4) - (_as.attack.hitStun.cancelTime / 2)) - Mathf.Abs(distance / 2))) / 100, (Mathf.Abs(Random.Range(0, (_as.attack.damage / 4) - (_as.attack.hitStun.cancelTime / 2)) - Mathf.Abs(distance))) / 100);
                    }
                    else if (distance < 0)
                    {

                        _as.usingFighter.ShakeCamera(0.5f, Mathf.Abs(Random.Range(0, (_as.attack.damage / 4) - (_as.attack.hitStun.cancelTime / 2)) - Mathf.Abs(distance / 2)));
                        _as.usingFighter.SetVibration((Mathf.Abs(Random.Range(0, (_as.attack.damage / 4) - (_as.attack.hitStun.cancelTime / 2)) - Mathf.Abs(distance / 2))) / 100, (Mathf.Abs(Random.Range(0, (_as.attack.damage / 4) - (_as.attack.hitStun.cancelTime / 2)) - Mathf.Abs(distance))) / 100);
                    }
                }
                
                if (other.transform.tag == "Wall")
                Destroy(gameObject, _as.attack.destroyTime);
            }
            else if(other.transform.tag == "SpellGate")
            {
                Destroy(gameObject);
            }

        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_as.startDelayPassed || _as.activatedByPlayer)
        {
            if (other.transform.tag == "Enemy" && other.gameObject.name != _as.user || other.transform.tag == "Player" && other.gameObject.name != _as.user)
            {
                // deal damage at a rate [if(time % rate)]
                other.gameObject.GetComponent<Health>().Damage(_as.attack.damage);

                if(other.transform.tag == "Player")
                {
                    other.gameObject.GetComponent<Fighter>().recentlyAttacked = true;
                }

                KnockbackListener knockbackScript = other.gameObject.GetComponent<KnockbackListener>();
                knockbackScript.SetHitstun(_as.attack.hitStun);
                Vector2 finalKnockback = _as.attack.knockback;
                finalKnockback.x *= _as.direction;
                knockbackScript.SetKnockback(finalKnockback);

                // Get direction of the impact relative to the player/ai


                if (_as.attack.attackType == Attack.AttackType.Beam)
                {
                    if (_as.attack.followUpAttack != null)
                    {
                        // check if attack is burst/aoe

                    }
                }

                Destroy(gameObject, _as.attack.destroyTime);

            }
            else if (other.transform.tag == "Ground" || other.transform.tag == "Wall")
            {
                if (_as.attack.attackType == Attack.AttackType.Blast)
                {
                    if (_as.attack.followUpAttack != null)
                    {
                        if (_as.attack.elementEffect == Attack.ElementEffect.Burst)
                        {
                            _as.StartNextAttack(_as.followUpAttack);
                        }
                        else if (_as.attack.followUpType == Attack.FollowUpType.Auto)
                        {
                            // Automatically Starts Next Attack
                        }
                        else if (_as.attack.followUpType == Attack.FollowUpType.Command)
                        {
                            // Sends a flag back to the caster to add the follow up attack in the attack queue
                        }
                    }
                }
                Destroy(gameObject, _as.attack.destroyTime);
            }
        }

    }
}
