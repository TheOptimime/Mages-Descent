﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour {

    public Fighter player;
    public Collider2D playerCol;
    Attack properties;
    
    public MeleeHitbox(Attack baseAttack)
    {
        properties.attackType = baseAttack.attackType;
        properties.element = baseAttack.element;
        properties.attackLength = baseAttack.attackLength;
    }

    public MeleeHitbox(Attack.AttackType attackType, Attack.Element element, float length)
    {
        properties.attackType = attackType;
        properties.element = element;
        properties.attackLength = length;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other != playerCol)
        {
            if(properties != null)
            {
                if (other.transform.tag == "Enemy" || other.transform.tag == "Player" && other.gameObject != player)
                {

                    other.gameObject.GetComponent<Health>().Damage(properties.damage);

                    KnockbackListener knockbackScript = other.gameObject.GetComponent<KnockbackListener>();
                    knockbackScript.SetHitstun(properties.hitStun);
                    Vector2 finalKnockback = properties.knockback;
                    finalKnockback.x *= player.cc.m_FacingRight ? 1 : -1;
                    knockbackScript.SetKnockback(finalKnockback);

                    if (other.transform.tag == "Enemy")
                    {
                        EnemyAI _enemy = other.gameObject.GetComponent<EnemyAI>();

                        _enemy.murderMeter += Mathf.Round(properties.damage / 4);

                        if (_enemy.murderMeter >= _enemy.murderMeterLimit)
                        {
                            if (properties.element == Attack.Element.Fire)
                            {
                                _enemy.ailmentHandler.ailment = AilmentHandler.Ailments.Burned;
                            }
                            else if (properties.element == Attack.Element.Ice)
                            {
                                _enemy.ailmentHandler.ailment = AilmentHandler.Ailments.Frozen;
                            }
                            else if (properties.element == Attack.Element.Blood)
                            {
                                _enemy.ailmentHandler.ailment = AilmentHandler.Ailments.Bleeding;
                                player.health.currentHealth += Mathf.Round(_enemy.murderMeterLimit / 2);
                            }
                            else if (properties.element == Attack.Element.Thunder)
                            {
                                _enemy.ailmentHandler.ailment = AilmentHandler.Ailments.Stunned;
                            }
                            else if (properties.element == Attack.Element.Arcane)
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

                            if (player != _player)
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

                    if (transform.position.x > other.transform.position.x)
                    {
                        if (other.transform.tag == "Player")
                        {
                            Fighter _player = other.gameObject.GetComponent<Fighter>();

                            if (player != _player)
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
                        }
                    }

                    player.IncrementComboChain(properties);
                    player.dabometer += Mathf.Abs(((properties.damage) / 2) * player.comboCount);
                    player.ShakeCamera(0.5f, Mathf.Abs(Random.Range(0, properties.damage / 2)));
                    player.SetVibration(Mathf.Abs(Random.Range(0, properties.damage / 2)), Mathf.Abs(Random.Range(0, properties.damage / 3)));
                }
            }
            
        }
        
    }

}
