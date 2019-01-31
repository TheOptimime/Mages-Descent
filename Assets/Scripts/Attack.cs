using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Attack : ScriptableObject{

    public string name, description, spritePath;
    public int damage, multiFireCount;
    public float attackLength, animationLength, animationCancelLength, xDisplacement, multiFireRate, speed, lifetime, chargeTime = 0, delay;
    public float hitStun, destroyTime, attackCharge;
    public bool hasSpecialChargeFunction, instantCast, burstOnDestroy;
    public Vector2 knockback;
    public List<int> joystickCommand;
    public Attack followUpAttack;

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

    public AttackType attackType;
    public Element element;
    public ElementEffect elementEffect;
    public ChargeType chargeType;
    public FollowUpType followUpType;


    // Multi hitting Attacks
    /// <summary>
    /// Multi hitting Projectile Attack with xDisplacement
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_description"></param>
    /// <param name="joystickInput"></param>
    /// <param name="_damage"></param>
    /// <param name="_knockback"></param>
    /// <param name="_delay"></param>
    /// <param name="_projectileSpeed"></param>
    /// <param name="_element"></param>
    /// <param name="_elementalEffect"></param>
    /// <param name="_lifetime"></param>
    /// <param name="_animationLength"></param>
    /// <param name="_animationCancelLength"></param>
    /// <param name="_spritePath"></param>
    /// <param name="_numberOfAttacks"></param>
    /// <param name="_attackRate"></param>
    /// <param name="_xDisplacement"></param>
    public Attack(string _name, string _description, List<int> joystickInput, int _damage, Vector2 _knockback, float _delay, float _projectileSpeed, Element _element, ElementEffect _elementalEffect, float _lifetime, float _animationLength, float _animationCancelLength, string _spritePath, int _numberOfAttacks, float _attackRate, float _xDisplacement)
    {
        attackType = AttackType.MultipleBlast;
        name = _name;
        description = _description;
        damage = _damage;
        speed = _projectileSpeed;
        element = _element;
        damage = _damage;
        elementEffect = _elementalEffect;
        animationLength = _animationLength;
        animationCancelLength = _animationCancelLength;
        spritePath = _spritePath;
        lifetime = _lifetime;
        multiFireCount = _numberOfAttacks;
        multiFireRate = _attackRate;
        joystickCommand = joystickInput;
        knockback = _knockback;
        xDisplacement = _xDisplacement;
    }

    /// <summary>
    /// Multi hitting attacks
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_description"></param>
    /// <param name="joystickInput"></param>
    /// <param name="_damage"></param>
    /// <param name="_knockback"></param>
    /// <param name="_delay"></param>
    /// <param name="_projectileSpeed"></param>
    /// <param name="_element"></param>
    /// <param name="_elementalEffect"></param>
    /// <param name="_lifetime"></param>
    /// <param name="_animationLength"></param>
    /// <param name="_animationCancelLength"></param>
    /// <param name="_spritePath"></param>
    /// <param name="_numberOfAttacks"></param>
    /// <param name="_attackRate"></param>
    public Attack(string _name, string _description, List<int> joystickInput, int _damage, Vector2 _knockback, float _delay, float _projectileSpeed, Element _element, ElementEffect _elementalEffect, float _lifetime, float _animationLength, float _animationCancelLength, string _spritePath, int _numberOfAttacks, float _attackRate)
    {
        attackType = AttackType.MultipleBlast;
        name = _name;
        description = _description;
        damage = _damage;
        speed = _projectileSpeed;
        element = _element;
        damage = _damage;
        elementEffect = _elementalEffect;
        animationLength = _animationLength;
        animationCancelLength = _animationCancelLength;
        spritePath = _spritePath;
        lifetime = _lifetime;
        multiFireCount = _numberOfAttacks;
        multiFireRate = _attackRate;
        joystickCommand = joystickInput;
        knockback = _knockback;
    }


    // Melee/Projectile Attacks
    /// <summary>
    /// Projectile Attack
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_description"></param>
    /// <param name="joystickInput"></param>
    /// <param name="_damage"></param>
    /// <param name="_knockback"></param>
    /// <param name="_delay"></param>
    /// <param name="_projectileSpeed"></param>
    /// <param name="_element"></param>
    /// <param name="_elementalEffect"></param>
    /// <param name="_attackType"></param>
    /// <param name="_lifetime"></param>
    /// <param name="_animationLength"></param>
    /// <param name="_animationCancelLength"></param>
    /// <param name="_spritePath"></param>
    public Attack(string _name, string _description, List<int> joystickInput, int _damage, Vector2 _knockback, float _delay, float _projectileSpeed, Element _element, ElementEffect _elementalEffect, AttackType _attackType, float _lifetime, float _animationLength, float _animationCancelLength, string _spritePath)
    {
        name = _name;
        description = _description;
        damage = _damage;
        speed = _projectileSpeed;
        element = _element;
        damage = _damage;
        elementEffect = _elementalEffect;
        animationLength = _animationLength;
        animationCancelLength = _animationCancelLength;
        spritePath = _spritePath;
        lifetime = _lifetime;
        joystickCommand = joystickInput;
        knockback = _knockback;
        delay = _delay;
        attackType = _attackType;
    }

    /// <summary>
    /// Projectile Attack with XDisplacement
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_description"></param>
    /// <param name="joystickInput"></param>
    /// <param name="_damage"></param>
    /// <param name="_knockback"></param>
    /// <param name="_delay"></param>
    /// <param name="_projectileSpeed"></param>
    /// <param name="_element"></param>
    /// <param name="_elementalEffect"></param>
    /// <param name="_attackType"></param>
    /// <param name="_lifetime"></param>
    /// <param name="_animationLength"></param>
    /// <param name="_animationCancelLength"></param>
    /// <param name="_spritePath"></param>
    /// <param name="_xDisplacement"></param>
    public Attack(string _name, string _description, List<int> joystickInput, int _damage, Vector2 _knockback, float _delay, float _projectileSpeed, Element _element, ElementEffect _elementalEffect, AttackType _attackType, float _lifetime, float _animationLength, float _animationCancelLength, string _spritePath, float _xDisplacement)
    {
        name = _name;
        description = _description;
        damage = _damage;
        speed = _projectileSpeed;
        element = _element;
        damage = _damage;
        elementEffect = _elementalEffect;
        animationLength = _animationLength;
        animationCancelLength = _animationCancelLength;
        spritePath = _spritePath;
        lifetime = _lifetime;
        joystickCommand = joystickInput;
        knockback = _knockback;
        xDisplacement = _xDisplacement;
        delay = _delay;
        attackType = _attackType;
    }

    //Attacks with followups
    /// <summary>
    /// Attack with follow up and XDisplacement
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_description"></param>
    /// <param name="joystickInput"></param>
    /// <param name="_damage"></param>
    /// <param name="_knockback"></param>
    /// <param name="_delay"></param>
    /// <param name="_speed"></param>
    /// <param name="_element"></param>
    /// <param name="_elementalEffect"></param>
    /// <param name="_attackType"></param>
    /// <param name="_lifetime"></param>
    /// <param name="_animationLength"></param>
    /// <param name="_animationCancelLength"></param>
    /// <param name="_spritePath"></param>
    /// <param name="_xDisplacement"></param>
    /// <param name="_followUp"></param>
    public Attack(string _name, string _description, List<int> joystickInput, int _damage, Vector2 _knockback, float _delay, float _speed ,Element _element, ElementEffect _elementalEffect, AttackType _attackType, float _lifetime, float _animationLength, float _animationCancelLength, string _spritePath, float _xDisplacement, Attack _followUp)
    {
        name = _name;
        description = _description;
        damage = _damage;
        speed = _speed;
        element = _element;
        damage = _damage;
        followUpAttack = _followUp;
        elementEffect = _elementalEffect;
        animationLength = _animationLength;
        animationCancelLength = _animationCancelLength;
        spritePath = _spritePath;
        lifetime = _lifetime;
        joystickCommand = joystickInput;
        knockback = _knockback;
        xDisplacement = _xDisplacement;
        delay = _delay;
        attackType = _attackType;
    }

    /// <summary>
    /// Attack with followup
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Description"></param>
    /// <param name="Joystick Input"></param>
    /// <param name="Damage"></param>
    /// <param name="Knockback"></param>
    /// <param name="Delay"></param>
    /// <param name="Speed"></param>
    /// <param name="Element"></param>
    /// <param name="Elemental Effect"></param>
    /// <param name="Attack Type"></param>
    /// <param name="Lifetime"></param>
    /// <param name="Animation Length"></param>
    /// <param name="Animation Cancel Length"></param>
    /// <param name="Sprite Path"></param>
    /// <param name="Follow Up"></param>
    public Attack(string _name, string _description, List<int> joystickInput, int _damage, Vector2 _knockback, float _delay, float _speed, Element _element, ElementEffect _elementalEffect, AttackType _attackType, float _lifetime, float _animationLength, float _animationCancelLength, string _spritePath, Attack _followUp)
    {
        name = _name;
        description = _description;
        damage = _damage;
        speed = _speed;
        element = _element;
        damage = _damage;
        followUpAttack = _followUp;
        elementEffect = _elementalEffect;
        animationLength = _animationLength;
        animationCancelLength = _animationCancelLength;
        spritePath = _spritePath;
        lifetime = _lifetime;
        joystickCommand = joystickInput;
        knockback = _knockback;
        delay = _delay;
        attackType = _attackType;
    }
}
