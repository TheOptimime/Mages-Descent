﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Fighter {

     



    public void UseAttack(Attack attack)
    {
        castTime = 0;
        if (!recentlyAttacked && attack != null)
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


        attackInQueue = null;
    }

    public void CastProjectile(Attack projectile)
    {
        //print("Cast projectile");
        GameObject projectileObject = new GameObject("Projectile");
        AttackScript spell = projectileObject.AddComponent<AttackScript>();
        spell.flipped = !cc.m_FacingRight;
        spell.attack = projectile;
        spell.origin = spellCastPoint.position;
        spell.direction = cc.m_FacingRight? 1 : -1;
        spell.user = gameObject.name;
    }

    IEnumerator MultiCast(Attack projectile)
    {

        for (int i = 0; i < projectile.multiFireCount; i++)
        {
            CastProjectile(projectile);
          
            yield return new WaitForSeconds(projectile.multiFireRate);
        }

    }

    public void SetAttackQueue(Attack attack)
    {
        if(attack != null)
        {
            castTime = 0;

            if (attack.instantCast)
            {
                UseAttack(attack);
                return;
            }

            attackIsInQueue = true;
            attackInQueue = attack;
            castTime = 0;
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

    public void RelayButtonInput()
    {
        print("holding attack: " + attackInQueue);
        if (attackIsInQueue)
        {
            if (attackInQueue != null && !recentlyAttacked)
            {
                castTime += Time.deltaTime;

                if (castTime > attackInQueue.chargeTime)
                {

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

    public void OnAttackButtonRelease()
    {
        //print("releasing attack: " + attackInQueue);
        if (attackIsInQueue)
        {
            if (attackInQueue != null && !recentlyAttacked)
            {
                if (castTime >= attackInQueue.chargeTime)
                {
                    UseAttack(attackInQueue);
                }
            }
            else
            {
                print("Error on releasing attack");
            }
        }
        

    }
}
