﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack {

    public enum AttackType
    {
        Melee,
        Blast,
        MultipleBlast
    }

    public enum Element
    {
        Fire,
        Ice,
        Blood,
        Thunder,
        Arcane
    }

    public enum ElementEffect
    {
        Burst,
        Burn,
        Freeze,
        Stun
    }


	public Attack(int damage, Element element,AttackType AT, float knockback)
    {

    }

    public Attack(int damage, Element element,AttackType AT,int numberOfHits, float knockback)
    {
        // if multiple blast is not selected 

    }

    public virtual void MultiFire()
    {

    }
}
