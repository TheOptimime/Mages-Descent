using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveSet))]
[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(CharacterController2D))]
[RequireComponent(typeof(Health))]
public class Fighter : MonoBehaviour {

    public float speed;
    CharacterController2D controller;
    SpellDatabase spellList;

    public Transform spellCastPoint;

    float horizontalMove = 0f;

    float runSpeed = 40f;

    public MoveSet moveset;    

    bool jump = false;

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
    }

    void Update()
    {


        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        //horizontalMove(); = Input.GetAxisRaw("Horizontal") * runSpeed;

        //animator.SetFloat("Speed", Mathf.Abs(horizontalMove));


        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
             
        }

        #region F Key
        if (Input.GetKey(KeyCode.F))
        {
            castTime += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.F) && (castTime > finishedCast))
        {
            UseAttack(moveset.Attacks[0]);
            castTime = 0;

        }
        else if (Input.GetKeyUp(KeyCode.F) && (castTime < finishedCast))
        {
            castTime = 0;
        }
        #endregion

        #region G Key
        if (Input.GetKey(KeyCode.G))
        {
            print("Phase1");
            castTime += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.G) && (castTime > 0.3f))
        {
            print("Phase3");
            UseAttack(moveset.Attacks[1]);
            castTime = 0;

        }
        else if (Input.GetKeyUp(KeyCode.G) && (castTime < 0.3f))
        {
            print("Phase2");
            castTime = 0;
        }
        #endregion

    }


    private void FixedUpdate()
    {
        //move character
        controller.Move(horizontalMove * Time.deltaTime, false, jump);
        jump = false;

    }

    void UseAttack(Attack attack)
    {
        print("in base function");
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
    }

    void CastProjectile(Attack projectile)
    {
        print("cast projectile");
        GameObject projectileObject = new GameObject("projectile");
        AttackScript spell = projectileObject.AddComponent<AttackScript>();
        spell.attack = projectile;
        spell.origin = spellCastPoint.position;
        spell.direction = controller.m_FacingRight? 1 : -1;
        print(controller.m_FacingRight);
        print("cast complete");
    }

	IEnumerator MultiCast (Attack projectile) {

        for(int i = 0; i < projectile.multiFireCount; i++){
            CastProjectile(projectile);
            yield return new WaitForSeconds(projectile.multiFireRate);
        }
        
	}

}
