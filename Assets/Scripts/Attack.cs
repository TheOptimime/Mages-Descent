using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack {

    public string name, description;
    public int damage, multiFireCount;
    public float attackLength, animationLength, xDisplacement, multiFireRate;

    public enum AttackType
    {
        Melee,              // Physical Attacks
        Blast,              // Projectile
        MultipleBlast,      // Multiple Projectiles
        Special             // Ultimate Attacks
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
        None,
        Burst,
        Burn,
        Freeze,
        Stun
    }


    AttackType attackType;
    Element element;
    ElementEffect elementEffect;

	public Attack(string _name, string _description, int _damage, Element _element, ElementEffect effect, AttackType AT, float knockback, float AttackLength,float AnimationLength)
    {
        name = _name;
        description = _description;
        damage = _damage;
        element = _element;
        damage = _damage;
        elementEffect = effect;
        attackLength = AttackLength;
        animationLength = AnimationLength;
    }

    /// <summary>
    /// Contains X-Displacement to move the player while attacking
    /// </summary>
    public Attack(string _name, string _description, int _damage, Element _element, ElementEffect effect, AttackType AT, float knockback, float AttackLength, float AnimationLength, float _XDisplacement)
    {
        name = _name;
        description = _description;
        damage = _damage;
        element = _element;
        damage = _damage;
        elementEffect = effect;
        attackLength = AttackLength;
        animationLength = AnimationLength;
        xDisplacement = _XDisplacement;
    }

    /// <summary>
    /// For Multi-Hitting Attacks
    /// </summary>
    public Attack(string _name, string _description, int _damage, Element _element, ElementEffect effect,AttackType AT,int numberOfAttacks, float knockback, float AttackLength, float AnimationLength)
    {
        attackType = AttackType.MultipleBlast;
        name = _name;
        description = _description;
        damage = _damage;
        element = _element;
        damage = _damage;
        elementEffect = effect;
        attackLength = AttackLength;
        animationLength = AnimationLength;
    }

    public virtual void MultiFire()
    {

    }
}
