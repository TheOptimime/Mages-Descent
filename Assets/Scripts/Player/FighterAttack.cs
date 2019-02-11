using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Fighter {


    MeleeHitbox hitbox_High, hitbox_Middle, hitbox_Low;


    public void UseAttack(Attack attack)
    {
        print("use attack function");
        castTime = 0;
        if (!recentlyAttacked && attack != null)
        {
            print("first wall");
            if (attack.attackType == Attack.AttackType.Special)
            {
                // stop time for attack length
                if(dabometer >= 100)
                {
                    StartAttack(attack);
                }
            }
            else if (attack.attackType == Attack.AttackType.Blast)
            {
                print("casting attack");
                StartAttack(attack);
            }
            else if (attack.attackType == Attack.AttackType.MultipleBlast)
            {
                StartAttack(attack);
            }
            else if (attack.attackType == Attack.AttackType.Melee)
            {

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
        spell.flipped = !cc.m_FacingRight;
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
        }
        else if(attack.attackPath == Attack.AttackPath.None)
        {
            spell.origin = backSpellCastPoint.position;
        }
        else
        {
            spell.origin = spellCastPoint.position;
        }
        
        spell.direction = cc.m_FacingRight ? 1 : -1;
        spell.user = gameObject.name;

        if(attack.simultaneousAttack != null)
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
        movementFreezeLength = new DoubleTime(attack.animationCancelLength, attack.animationLength);

        print("xDisp: " + attack.xDisplacement);

        if (attack.xDisplacement != 0)
        {
            //rb.AddForce(new Vector2(rb.velocity.x + attack.xDisplacement * spell.direction, rb.velocity.y), ForceMode2D.Impulse);
            rb.velocity = new Vector2(rb.velocity.x + attack.xDisplacement * spell.direction, rb.velocity.y);
            print("force applied");
        }
    }

    public void StartMeleeAttack(Attack attack)
    {

    }

    public void SetAttackQueue(Attack attack)
    {
        if(attack != null)
        {
            castTime = 0;

            attackInQueue = attack;
            attackIsInQueue = true;

            if (attack.instantCast)
            {
                UseAttack(attack);
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
            movementFreezeLength = new DoubleTime(attack.animationCancelLength, attack.animationLength);
            yield return new WaitForSeconds(attack.multiFireRate);
        }
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
                    UseAttack(attackInQueue);
                }
            }
            else
            {
                print("Error on releasing attack");
            }
        }

        if (attackIsSpecialHeld)
        {
            specialHold.activatedByPlayer = true;
        }
    }
}
