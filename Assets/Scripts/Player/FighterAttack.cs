using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Fighter {


    MeleeHitbox hitbox;


    public void UseAttack(Attack attack)
    {
        castTime = 0;
        if (!recentlyAttacked && attack != null)
        {
            if (attack.attackType == Attack.AttackType.Special)
            {
                // stop time for attack length
                if(dabometer >= 100)
                {
                    StartAttack(attack);
                    dabometer = 0;
                    health.invulnerabilityTimer = attack.lifetime;
                }
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
            else if (attack.attackType == Attack.AttackType.Melee)
            {
                StartMeleeAttack(attack);
            }
            else if (attack.attackType == Attack.AttackType.Beam)
            {
                print("casting attack");
                StartAttack(attack);
            }
        }


        attackInQueue = null;
    }

    public void StartAttack(Attack attack)
    {
        print("Cast projectile + " + attack.name);
        GameObject attackObject = new GameObject("Projectile");
        AttackScript spell = attackObject.AddComponent<AttackScript>();

        if (attack.attackType == Attack.AttackType.Special && attack.attackPath == Attack.AttackPath.Homing)
        {

        }

        anim.SetInteger("element", (int)attack.element);
        anim.SetTrigger("cast");

        cc.FreezeVelocity();
        // Cast animation goes here... probably
        anim.SetFloat("speed", 0);

        if (attack.attackPath != Attack.AttackPath.CrashDown || attack.attackPath != Attack.AttackPath.Meteor)
        {
            spell.flipped = !cc.m_FacingRight;
        }
        
        spell.attack = attack;
        spell.usingFighter = this;

        if(attack.chargeType == Attack.ChargeType.Instant)
        {
            if (attack.hasSpecialChargeFunction)
            {
                if (attack.attackPath == Attack.AttackPath.Homing)
                {
                    attackIsSpecialHeld = true;
                    specialHold = spell;
                }
            }
        }
        
        

        if(attack.attackPath == Attack.AttackPath.Meteor || attack.attackPath == Attack.AttackPath.CrashDown)
        {
            spell.origin = meteorSpellCastPoint.position;
            
            if(attack.attackType == Attack.AttackType.MultipleBlast)
            {
                if (attack.attackPath == Attack.AttackPath.Meteor)
                {
                    spell.origin.x = meteorSpellCastPoint.position.x + Random.Range(0, attack.xPositionalDisplacement) + (spell.attack.offset.x * spell.direction);
                }
                else if(attack.attackPath == Attack.AttackPath.CrashDown)
                {
                    spell.origin.x = meteorSpellCastPoint.position.x + Random.Range(0, attack.xPositionalDisplacement);
                }
            }
        }
        else if(attack.attackPath == Attack.AttackPath.Homing)
        {
            spell.origin = backSpellCastPoint.position;
        }
        else
        {
            spell.origin = spellCastPoint.position;
        }
        
        spell.direction = cc.m_FacingRight ? 1 : -1;
        spell.user = gameObject.name;
        spell.origin += new Vector2(attack.offset.x * spell.direction, attack.offset.y);

        spell.transform.position = spell.origin;

        if (attack.simultaneousAttack != null)
        {
            if(attack.attackType == Attack.AttackType.Melee)
            {
                StartMeleeAttack(attack.simultaneousAttack);
            }
            else
            {
                
                
                StartAttack(attack.simultaneousAttack);
            }
        }

        if (attack.coolDown.defaultTime == 0 && attack.coolDown.cancelTime == 0)
        {
            movementFreezeLength = new DoubleTime(attack.animationCancelLength, attack.animationLength);
        }
        else
        {
            //movementFreezeLength = new DoubleTime();
            movementFreezeLength = attack.coolDown;
        }
        //movementFreezeLength = new DoubleTime(attack.attackLength, attack.attackLength);

        //print("xDisp: " + attack.xDisplacement + comboCount);

        if (attack.xDisplacement + comboCount != 0)
        {
            //rb.AddForce(new Vector2(rb.velocity.x + attack.xDisplacement * spell.direction, rb.velocity.y), ForceMode2D.Impulse);
            if(attack.attackType != Attack.AttackType.MultipleBlast)
            {
                rb.velocity = new Vector2( Mathf.Abs(rb.velocity.x + attack.xDisplacement + comboCount) * -spell.direction, rb.velocity.y);
            }
            else
            {
                if(attack.attackPath == Attack.AttackPath.Straight)
                {
                    //rb.velocity = new Vector2(rb.velocity.x)
                }
            }
            
            //print("force applied");
        }
    }

    public void StartMeleeAttack(Attack attack)
    {
        cc.FreezeVelocity();
        

        if(attack.name == "Jab 1")
        {
            // light
            anim.SetInteger("jab", 1);
            hitbox = new MeleeHitbox(attack);
        }
        else if(attack.name == "Jab 2")
        {
            //mid
            anim.SetInteger("jab", 1);
        }


        
        
        //hitbox_Middle.Activate();

        if (attack.xDisplacement + comboCount != 0)
        {
            //rb.AddForce(new Vector2(rb.velocity.x + attack.xDisplacement * spell.direction, rb.velocity.y), ForceMode2D.Impulse);
            //rb.velocity = new Vector2((rb.velocity.x + attack.xDisplacement + comboCount) * (cc.m_FacingRight? 1 : -1), rb.velocity.y);
            rb.velocity = new Vector2((rb.velocity.x + attack.xDisplacement + comboCount) * (cc.m_FacingRight ? 1 : -1), rb.velocity.y);
            print("force applied");
        }
    }

    public void SetAttackQueue(Attack attack)
    {
        if(attack != null)
        {
            castTime = 0;

            attackInQueue = attack;
            attackIsInQueue = true;

            if (attack.chargeType == Attack.ChargeType.Instant)
            {
                if(movementFreezeLength.cancelTime <= 0)
                {
                    //print(movementFreezeLength.cancelTime + " " + movementFreezeLength.defaultTime);
                    UseAttack(attack);
                }
                return;
            }

            
            if (recentlyAttacked)
            {
                attackInQueue = null;
            }
        }
        else
        {
            attackInQueue = null;
            attackIsInQueue = false;
        }
        
    }

    IEnumerator MultiCast(Attack attack)
    {
        for(int i = 0; i < attack.multiFireCount; i++)
        {
            if (!recentlyAttacked)
            {
                if (attack.coolDown.defaultTime == 0 && attack.coolDown.cancelTime == 0)
                {
                    movementFreezeLength = new DoubleTime(attack.animationCancelLength, attack.animationLength);
                }
                else
                {
                    //movementFreezeLength = new DoubleTime();
                    movementFreezeLength = attack.coolDown;
                }
                
                

                if(attack.attackType == Attack.AttackType.Special)
                {
                    if (attack.attackPath == Attack.AttackPath.Homing)
                    {
                        //attack.offset.x = FindObjectOfType<Vortex>().transform.position.x;
                    }
                }
                
                StartAttack(attack);

                yield return new WaitForSeconds(attack.multiFireRate);
            }
            else
            {
                StopCoroutine(multicastCoroutine);
            }
        }
        StopCoroutine(multicastCoroutine);
    }

    public void RelayButtonInput()
    {
        if (attackIsInQueue)
        {
            if (attackInQueue != null && !recentlyAttacked)
            {
                castTime += Time.deltaTime;

                if (castTime > attackInQueue.chargeTime)
                {
                    attackInQueue.attackCharge = Mathf.Round(castTime);
                }
            }
        }
        else
        {
            attackIsInQueue = false;
            attackInQueue = null;
        }

        

    }

    public void RelayJumpButtonInput()
    {

    }

    public void IncreaseSpellbookExp(Attack.Element element, int value)
    {
        // Find all elements that match and raise exp 
    }

    public void OnAttackButtonRelease()
    {
        //print("releasing attack: " + attackInQueue);
        if (attackIsInQueue)
        {
            if (attackInQueue != null && !recentlyAttacked)
            {
                if (castTime >= attackInQueue.chargeTime)
                {
                    print("attack cast + " +attackInQueue.name);
                    if(movementFreezeLength.cancelTime <= 0)
                    {
                        print(movementFreezeLength.cancelTime + " " + movementFreezeLength.defaultTime);
                        UseAttack(attackInQueue);
                    }
                    
                }
            }
            else
            {
                //print("Error on releasing attack");
            }
        }

        if (attackIsSpecialHeld)
        {
            specialHold.activatedByPlayer = true;
            attackIsSpecialHeld = false;
            specialHold = null;
        }
    }
}
