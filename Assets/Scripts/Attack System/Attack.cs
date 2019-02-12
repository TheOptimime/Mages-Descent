using System.Collections;
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

    
}
