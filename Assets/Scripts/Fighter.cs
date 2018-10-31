using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveSet))]
[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(CharacterController2D))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Inventory))]
public class Fighter : MonoBehaviour {

    public float speed;
    public CharacterController2D controller;
    SpellDatabase spellList;

    public bool attackIsInQueue;

    Attack attackInQueue;

    public Transform spellCastPoint;

    InputHandler input;

    float horizontalMove = 0f;

    public float runSpeed = 40f;

    public MoveSet moveset;    

    public bool jump = false;

    public float forwardLeapSpeed, backStepSpeed, backwardLeapSpeed;

    public GameObject fireBullet;

    Transform firePos;

    CharacterController2D fighterController;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private Rigidbody2D rb;

    public float castTime = 0;
    public float finishedCast = 1;


    // Use this for initialization
    void Start()
    {
        fighterController = GetComponent<CharacterController2D>();
        controller = GetComponent<CharacterController2D>();
        rb = GetComponent<Rigidbody2D>();
        moveset = GetComponent<MoveSet>();
        firePos = transform.Find("SpellSpawner");
        spellList = FindObjectOfType<SpellDatabase>();
        input = GetComponent<InputHandler>();
    }

    void Update()
    {


        

        if(input.joystickPosition == 4)
        {
            horizontalMove = -(runSpeed);
        }
        else if(input.joystickPosition == 6)
        {
            horizontalMove = runSpeed;
        }
        else
        {
            horizontalMove = 0;
        }

        //if(input)
        

        //animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        /* Legacy Input
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButton("Fire3"))
        {
            jump = true;
             
        }

        #region F Key
        if (Input.GetKey(KeyCode.F) || Input.GetButton("Fire1"))
        {
            castTime += Time.deltaTime;
            if(castTime > 10)
            {
                SetVibration(0.4f, 0.4f);
                SetVibration(0.5f, 0.5f);
                SetVibration(0.6f, 0.6f);
            }
            else
            {
                SetVibration((castTime)/2, (castTime)/2);
            }
            
        }

        if (Input.GetKeyUp(KeyCode.F) || Input.GetButtonUp("Fire1") && (castTime > finishedCast))
        {
            UseAttack(moveset.Attacks[0]);
            SetVibration(0, 0);
            castTime = 0;

        }
        else if (Input.GetKeyUp(KeyCode.F) || Input.GetButtonUp("Fire1") && (castTime < finishedCast))
        {
            SetVibration(0, 0);
            castTime = 0;
        }
        #endregion

        #region G Key
        if (Input.GetKey(KeyCode.G) || Input.GetButton("Fire2") )
        {
            print("Phase1");
            castTime += Time.deltaTime;

            if (castTime > 3)
            {
                SetVibration(0.4f, 0.4f);
                SetVibration(0.5f, 0.5f);
            }
            else
            {
                SetVibration((castTime) / 2f, (castTime) / 2f);
                print((castTime) / 2f);
            }
        }

        if (Input.GetKeyUp(KeyCode.G) || Input.GetButtonUp("Fire2") && (castTime > 0.3f))
        {
            print("Phase3");
            UseAttack(moveset.Attacks[1]);
            SetVibration(0f, 0f);
            castTime = 0;

        }
        else if (Input.GetKeyUp(KeyCode.G) || Input.GetButtonUp("Fire2") && (castTime < 0.3f))
        {
            print("Phase2");
            SetVibration(0f, 0f);
            castTime = 0;
        }
        #endregion

    */

    }

    private void FixedUpdate()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Fire3"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        //move character
        controller.Move(horizontalMove * Time.deltaTime, false, jump);
        jump = false;

    }

    public void UseAttack(Attack attack)
    {
        print("in base function");

        castTime = 0;

        if(attack.attackType == Attack.AttackType.Special)
        {
            // stop time for attack length
        }
        else if(attack.attackType == Attack.AttackType.Blast)
        {
            print("casting attack");
            CastProjectile(attack);
        }
        else if(attack.attackType == Attack.AttackType.MultipleBlast)
        {
            StartCoroutine(MultiCast(attack));
        }
        else if(attack.attackType == Attack.AttackType.Melee)
        {

        }
        else if(attack.attackType == Attack.AttackType.Beam)
        {

        }

        attackInQueue = null;
    }

    public void CastProjectile(Attack projectile)
    {
        print("cast projectile");
        GameObject projectileObject = new GameObject("projectile");
        AttackScript spell = projectileObject.AddComponent<AttackScript>();
        spell.attack = projectile;
        spell.origin = spellCastPoint.position;
        spell.direction = controller.m_FacingRight? 1 : -1;
        //print(controller.m_FacingRight);
        //print("cast complete");
    }

	IEnumerator MultiCast (Attack projectile) {

        for(int i = 0; i < projectile.multiFireCount; i++){
            CastProjectile(projectile);
            yield return new WaitForSeconds(projectile.multiFireRate);
        }
        
	}

    public void SetAttackQueue(Attack attack)
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
    }

    public void RelayButtonInput()
    {
        print("holding attack: " + attackInQueue);

        if(attackInQueue != null)
        {
            castTime += Time.deltaTime;

            if (castTime > attackInQueue.chargeTime)
            {

            }
        }
        else
        {

        }
        
    }

    public void RelayJumpButtonInput()
    {
        
    }

    public void OnAttackButtonRelease()
    {
        print("releasing attack: " + attackInQueue);

        if (attackInQueue != null)
        {
            if (castTime >= attackInQueue.chargeTime)
            {
                UseAttack(attackInQueue);
            }
        }
        else
        {

        }
        
    }

    void SetVibration(float leftMotor, float rightMotor)
    {
        input.vibrateLeftMotor = leftMotor;
        input.vibrateRightMotor = rightMotor;
    }
}
