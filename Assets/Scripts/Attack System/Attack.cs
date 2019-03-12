﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Attack : ScriptableObject{

    public new string name;

    [TextArea] public string description; 

    public int damage, multiFireCount;

    public float attackLength;
    [HideInInspector] public float animationLength, animationCancelLength, destroyTime;
    public float xDisplacement, multiFireRate, speed, lifetime, delay;
    public float maxAttackDistance;
    public float chargeTime = 0;

    [HideInInspector] public float xPositionalDisplacement;


    public float hitStun, attackCharge;
    public bool hasSpecialChargeFunction, burstOnDestroy;

    public int spellPoints;

    public Vector2 knockback;

    [HideInInspector] public List<int> _joystickCommand;

    public Attack followUpAttack, simultaneousAttack;

    public GameObject spriteAnimation, attackBase;
    

    public enum AttackPath
    {
        None,
        Straight,
        CrashDown,
        Meteor,
        SineWave,
        Curved,
        Boomerang,
        Homing,
        Custom
    }

    public enum AttackType
    {
        Melee,              // Physical Attacks
        Blast,              // Projectile
        MultipleBlast,      // Multiple Projectiles
        Beam,               // beam beams
        Impale,             // Piercing attack
        Special             // Ultimate Attacks
    }

    public enum FollowUpType
    {
        None,
        Auto,
        Command
    }

    public enum Element
    {
        None,
        Fire,
        Ice,
        Blood,
        Thunder,
        Arcane
    }

    public enum ElementEffect
    {
        None,
        Burst,
        Burn,
        Freeze,
        Stun,
        Slow
    }

    public enum ChargeType
    {
        Instant,
        Charge
    }

    public AttackPath attackPath;
    public Vector3[] keyPoints;
    public AttackType attackType;
    public Element element;
    public ElementEffect elementEffect;
    public ChargeType chargeType;
    public FollowUpType followUpType;
    public JoystickCommands joystickCommand;

    

    
}