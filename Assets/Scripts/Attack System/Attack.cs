using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Attack : ScriptableObject{

    public new string name;
    public string spritePath;

    [TextArea] public string description; 

    public int damage, multiFireCount;

    public float attackLength;
    [HideInInspector] public float animationLength, animationCancelLength, destroyTime;
    public float xDisplacement, multiFireRate, speed, lifetime, delay;
    public float chargeTime = 0;


    public float hitStun, attackCharge;
    public bool hasSpecialChargeFunction, instantCast, burstOnDestroy;

    public int spellPoints;

    public Vector2 knockback;

    public List<int> _joystickCommand;

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

    public enum JoystickCommands
    {
        None,
        QuarterCircleDownRight,
        QuarterCircleRightDown,
        QuarterCircleRightUp,
        QuarterCircleUpRight,
        HalfCircleUnderRightLeft,
        HalfCircleOverLeftRight,
        HalfCircleDownUpRight,
        HalfCircleUpDownRight,
        FullCircleRightUp,
        FullCircleRightDown,
        FourCircleUpRight,
        FourCircleDownRight
    }

    public AttackPath attackPath;
    public Vector3[] keyPoints;
    public AttackType attackType;
    public Element element;
    public ElementEffect elementEffect;
    public ChargeType chargeType;
    public FollowUpType followUpType;
    public JoystickCommands joystickCommand;

    public Attack()
    {
        switch ((int) joystickCommand)
        {
            case 0:
                _joystickCommand = new List<int>();
                break;

            case 1:
                _joystickCommand = new List<int>() { 8, 9, 6 };
                break;

            case 2:
                _joystickCommand = new List<int>() { 6, 9, 8 };
                break;

            case 3:
                _joystickCommand = new List<int>() { 6, 3, 2 };
                break;

            case 4:
                _joystickCommand = new List<int>() { 2, 3, 6 };
                break;

            case 5:
                _joystickCommand = new List<int>() { 6, 9, 8, 7, 4 };
                break;

            case 6:
                _joystickCommand = new List<int>() { 4, 1, 2, 3, 6 };
                break;

            case 7:
                _joystickCommand = new List<int>() { 8, 9, 6, 3, 2 };
                break;

            case 8:
                _joystickCommand = new List<int>() { 2, 3, 6, 9, 8 };
                break;

            case 9:
                _joystickCommand = new List<int>() { 6, 3, 2, 1, 4, 7, 8, 9, 6 };
                break;

            case 10:
                _joystickCommand = new List<int>() { 6, 9, 8, 7, 4, 1, 2, 3, 6 };
                break;

            case 11:
                _joystickCommand = new List<int>() { 2, 3, 6, 9 };
                break;

            case 12:
                _joystickCommand = new List<int>() { 8, 9, 6, 3 };
                break;

            
        }
    }

    /*

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

    */
}
