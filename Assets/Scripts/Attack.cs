using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack {

    public string name, description, spritePath;
    public int damage, multiFireCount;
    public float attackLength, animationLength, xDisplacement, multiFireRate, speed, lifetime, chargeTime = 0, delay;
    public float hitStun;
    public bool hasSpecialChargeFunction, instantCast;
    public List<int> joystickCommand;

    public enum AttackType
    {
        Melee,              // Physical Attacks
        Blast,              // Projectile
        MultipleBlast,      // Multiple Projectiles
        Beam,               // beam beams
        Impale,             // Piercing attack
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
        Stun,
        Slow
    }


    public AttackType attackType;
    public Element element;
    public ElementEffect elementEffect;

    /// <summary>
    /// Most Basic Attack
    /// </summary>
	public Attack(string _name, string _description, int _damage, Element _element, ElementEffect effect, AttackType AT, float knockback, float AttackLength,float AnimationLength)
    {
        name = _name;
        description = _description;
        damage = _damage;
        element = _element;
        damage = _damage;
        elementEffect = effect;
        attackType = AT;
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
        attackType = AT;
        animationLength = AnimationLength;
        xDisplacement = _XDisplacement;
    }

    /// <summary>
    /// For Multi-Hitting Attacks
    /// </summary>
    public Attack(string _name, string _description, int _damage, Element _element, ElementEffect effect,AttackType AT,int numberOfAttacks, float knockback, float AttackLength, float AnimationLength)
    {
        //attackType = AttackType.MultipleBlast;
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
    /// Contains X-Displacement to move the player while attacking + projectile
    /// </summary>
    public Attack(string _name, string _description, int _damage, float _speed, Element _element, ElementEffect effect, AttackType AT, float knockback, float _lifetime, float AttackLength, float AnimationLength, float _XDisplacement, string _spritePath)
    {
        attackType = AttackType.Blast;
        name = _name;
        description = _description;
        damage = _damage;
        element = _element;
        damage = _damage;
        speed = _speed;
        elementEffect = effect;
        attackLength = AttackLength;
        animationLength = AnimationLength;
        xDisplacement = _XDisplacement;
        spritePath = _spritePath;
        lifetime = _lifetime;
    }

    /// <summary>
    /// For Multi-Hitting Projectiles
    /// </summary>
    public Attack(string _name, string _description, int _damage, float _speed, Element _element, ElementEffect effect, AttackType AT, int numberOfAttacks, float attackRate, float knockback, float _lifetime, float AttackLength, float AnimationLength, string _spritePath)
    {
        attackType = AttackType.MultipleBlast;
        name = _name;
        description = _description;
        damage = _damage;
        speed = _speed;
        element = _element;
        damage = _damage;
        elementEffect = effect;
        attackLength = AttackLength;
        animationLength = AnimationLength;
        spritePath = _spritePath;
        lifetime = _lifetime;
        multiFireCount = numberOfAttacks;
        multiFireRate = attackRate;
    }

    public Attack(string _name, string _description, int _damage, float _speed, Element _element, ElementEffect effect, AttackType AT, int numberOfAttacks, float attackRate, float knockback, float _lifetime, float AttackLength, float AnimationLength, string _spritePath, List<int> joystickInput)
    {
        attackType = AttackType.MultipleBlast;
        name = _name;
        description = _description;
        damage = _damage;
        speed = _speed;
        element = _element;
        damage = _damage;
        elementEffect = effect;
        attackLength = AttackLength;
        animationLength = AnimationLength;
        spritePath = _spritePath;
        lifetime = _lifetime;
        multiFireCount = numberOfAttacks;
        multiFireRate = attackRate;
        joystickCommand = joystickInput;
    }

    public Attack(string _name, string _description, int _damage, float _speed, Element _element, ElementEffect effect, AttackType AT, int numberOfAttacks, float attackRate, float knockback, float _lifetime, float AttackLength, float _delay, float AnimationLength, string _spritePath)
    {
        attackType = AttackType.MultipleBlast;
        name = _name;
        description = _description;
        damage = _damage;
        speed = _speed;
        element = _element;
        damage = _damage;
        elementEffect = effect;
        attackLength = AttackLength;
        delay = _delay;
        animationLength = AnimationLength;
        spritePath = _spritePath;
        lifetime = _lifetime;
        multiFireCount = numberOfAttacks;
        multiFireRate = attackRate;
    }
}
