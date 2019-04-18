using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class HellhoundHead : MonoBehaviour
{

    Health health;
    public bool isDead, deathReported;

    public float idleMinTime, idleMaxTime, coolDownTimeMax;

    float time, idleTime, coolDownTime;
    public int coolDownInc;

    public int spellPoints;
    IEnumerator multicastCoroutine;

    EnemyController2D ec;

    public Transform spellCastPoint;

    public Attack[] weakAttacks;
    public Attack[] strongAttacks;

    Fighter[] players;

    public bool stunCounterEnabled;

    int direction;
    float stunLevel;
    int stunCounter;

    public enum BossState
    {
        idle,
        attacking,
        attacked,
        stunned
    }

    BossState state;
    BossState oldState;


    void Start()
    {
        health = GetComponent<Health>();
        state = new BossState();
    }
    
    void Update()
    {
        time += Time.deltaTime;

        if(stunLevel > 100)
        {
            stunLevel--;
            stunLevel -= stunCounter;

            state = BossState.stunned;
        }

        if(health.currentHealth <= 0)
        {
            isDead = true;
        }

        if(isDead != true)
        {
            switch (state)
            {
                case BossState.idle:
                    if(oldState != state)
                    {
                        time = 0;
                        oldState = state;
                    }

                    if(time > idleTime)
                    {
                        state = BossState.attacking;
                    }
                    break;

                case BossState.attacking:
                    if (oldState != state)
                    {
                        time = 0;
                        oldState = state;
                    }

                    if(spellPoints < 10)
                    {
                        Attack attack = weakAttacks[Random.Range(0, weakAttacks.Length - 1)];
                        spellPoints += attack.spellPoints;
                        UseAttack(attack);
                    }
                    else
                    {
                        Attack attack = strongAttacks[Random.Range(0, strongAttacks.Length - 1)];
                        spellPoints -= attack.spellPoints;
                        UseAttack(attack);
                    }
                    
                    break;

                case BossState.attacked:
                    if (oldState != state)
                    {
                        time = 0;
                        oldState = state;
                    }

                    if (time > coolDownTime * coolDownInc)
                    {
                        if(Random.Range(0,1) == 0)
                        {
                            coolDownInc = 1;
                            state = BossState.idle;
                        }
                        else
                        {
                            coolDownInc++;
                            state = BossState.attacking;

                        }
                    }

                    break;

                case BossState.stunned:
                    if(stunCounter <= 0)
                    {
                        spellPoints = stunCounter * 2;
                        coolDownInc = 1;
                        state = BossState.idle;
                    }
                    break;
            }
        }
        else
        {
            if (!deathReported)
            {

                deathReported = true;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

    }

    public void UseAttack(Attack attack)
    {
        
            if (attack.attackType == Attack.AttackType.Special)
            {
                // stop time for attack length
                
            }
            else if (attack.attackType == Attack.AttackType.Blast)
            {
                //print("casting attack");
                StartAttack(attack);
            }
            else if (attack.attackType == Attack.AttackType.MultipleBlast)
            {
                multicastCoroutine = MultiCast(attack);
                StartCoroutine(multicastCoroutine);
            }
            else if (attack.attackType == Attack.AttackType.Beam)
            {
                
                StartAttack(attack);
            }
        
    }

    IEnumerator MultiCast(Attack attack)
    {
        for (int i = 0; i < attack.multiFireCount; i++)
        {

            if (attack.attackPath == Attack.AttackPath.Meteor)
            {
                attack.offset.x = Random.Range(0, attack.xPositionalDisplacement);
            }

            if (attack.attackType == Attack.AttackType.Special)
            {
                if (attack.attackPath == Attack.AttackPath.Homing)
                {
                    //attack.offset.x = FindObjectOfType<Vortex>().transform.position.x;
                }
            }

            StartAttack(attack);
            
            yield return new WaitForSeconds(attack.multiFireRate);
        }

        StopCoroutine(multicastCoroutine);
    }
            
        
        
    

    public void StartAttack(Attack attack)
    {
        print("Cast projectile + " + attack.name);
        GameObject attackObject = new GameObject("Projectile");
        AttackScript spell = attackObject.AddComponent<AttackScript>();

        if(attack.attackPath != Attack.AttackPath.Meteor ||
            attack.attackPath != Attack.AttackPath.CrashDown ||
            attack.attackPath != Attack.AttackPath.Homing)
        {
            spell.attackPath = Attack.AttackPath.None;

            spell.transform.Translate(Vector2.MoveTowards(spell.transform.position, players[Random.Range(0, players.Length - 1)].transform.position, spell.xSpeed) * spell.xSpeed);

        }


        if (attack.attackType == Attack.AttackType.Special && attack.attackPath == Attack.AttackPath.Homing)
        {

        }
        

        if (attack.attackPath != Attack.AttackPath.CrashDown || attack.attackPath != Attack.AttackPath.Meteor)
        {
            //direction
            spell.flipped = !ec.m_FacingRight;
        }

        spell.attack = attack;
        //spell.usingEnemy = this;
        
        if (attack.attackPath == Attack.AttackPath.Meteor || attack.attackPath == Attack.AttackPath.CrashDown)
        {
            spell.origin = spellCastPoint.position;
        }
        else if (attack.attackPath == Attack.AttackPath.Homing)
        {
            spell.origin = spellCastPoint.position;
        }
        else
        {
            spell.origin = spellCastPoint.position;
        }

        spell.direction = direction;
        spell.user = gameObject.name;

        if (attack.simultaneousAttack != null)
        {
            StartAttack(attack.simultaneousAttack);
        }

        state = BossState.attacked;
    }
}
