using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#region
[DisallowMultipleComponent]
[RequireComponent(typeof(MoveSet))]
[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(PlayerController2D))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AilmentHandler))]
[RequireComponent(typeof(UI_Health))]
[RequireComponent(typeof(FighterAnimationScript))]
#endregion
public partial class Fighter : MonoBehaviour {

    #region Variables
    public float speed;
    [HideInInspector] public PlayerController2D cc;
    public float dabometer;
    SpellDatabase spellList;

    [HideInInspector] public int PlayerID;
    [HideInInspector] public bool justLanded;
    public int comboCount;
    public float comboTimer, comboTime, defaultComboTime = 4;
    public bool attackIsInQueue, attackInProgress, attackIsSpecialHeld;
    float vibrateLength;

    AttackScript specialHold;
    Attack attackInQueue;

    public Transform spellCastPoint, meteorSpellCastPoint, backSpellCastPoint;

    InputHandler input;
    DoubleTime movementFreezeLength;
    public Vector2 recoveryVelocity;

    public bool hasSword;
    public bool isDead;

    Vector2 movePos;

    public float horizontalMove = 0f, verticalMove = 0f;

    public float recoveryTimer, recoveryTime;

    public float runSpeed = 40f;

    public int maxNumberOfJumps;

    [HideInInspector] public MoveSet moveset;

    public bool jump = false, specialJump = false;
    public bool isDashing, recentlyAttacked;
    public bool isFacingRight;
    public bool canDoubleJump, doubleJumpUsed;
    public bool lockMovement, lockInput, canRecover;

    public bool isLeaping, isBackLeaping, isBackStepping;

    public float forwardLeapSpeed, backStepSpeed, backwardLeapSpeed;
    public float cameraShakeDuration;
    public float cameraShakeAmount;

    public GameObject fireBullet;
    public GameObject lowHitbox, midHitbox, highHitbox;
    public GameObject cameraFocusPoint;

    Vector2 cameraFocusPointDefaultPosition, playerOffset;

    GameManager gm;

    [HideInInspector] public SpriteRenderer sr;

    public Animator anim;

    AudioSource audioSource;

    public CircleCollider2D vibrationSphere;
    
    IEnumerator multicastCoroutine;

    //Transform firePos;
    [HideInInspector] public Health health;
    //CharacterController2D cc;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [HideInInspector] public Rigidbody2D rb;

    public float castTime = 0;
    public float finishedCast = 1;
    public bool respawnCalled;

    KnockbackListener knockbackListener;
    AilmentHandler ailmentHandler;

    RespawnManager rm;

    FighterAnimationScript fas;

	public GameObject dustParticle; 
	public Transform dustParticleSpawn;
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        cc = GetComponent<PlayerController2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        moveset = GetComponent<MoveSet>();
        input = GetComponent<InputHandler>();
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        audioSource = GetComponent<AudioSource>();
        ailmentHandler = GetComponent<AilmentHandler>();
        knockbackListener = GetComponent<KnockbackListener>();
        cameraFocusPointDefaultPosition = cameraFocusPoint.transform.position;
        playerOffset = transform.position;
        spellList = FindObjectOfType<SpellDatabase>();
        fas = GetComponent<FighterAnimationScript>();

        input.player = this;
        input.spellDatabase = spellList;
        fas.fighter = this;
        fas.cc = cc;
        fas.anim = anim;

        /*
        Fighter[] fighters = GameObject.FindObjectsOfType<Fighter>();

        for(int i = 0; i < fighters.Length; i++)
        {
            if(fighters[i] == this)
            {
                if(PlayerID == fighters[i].PlayerID)
                {
                    Destroy(fighters[i]);
                }
            }
        }
        */

                
        gm = FindObjectOfType<GameManager>();
        rm = FindObjectOfType<RespawnManager>();

        //DontDestroyOnLoad(this.gameObject);

        cc.m_doubleJumpEnabled = canDoubleJump;

        comboTimer = defaultComboTime;
    }

    void Update()
    {
        vibrateLength -= Time.deltaTime;

        if (vibrateLength <= 0)
        {
            SetVibration(0);
        }
        

        if (health.currentHealth <= 0 && respawnCalled != true)
        {
            print("dead");
            isDead = true;
            lockMovement = true;
            lockInput = true;
            anim.SetBool("death", true);
            respawnCalled = true;
            StartCoroutine(SlowReload());
        }

        if (!isDead)
        {
            if (dabometer >= 100)
            {
                dabometer = 100;

            }

            if (recentlyAttacked)
            {
                if (recoveryTimer > 0)
                {
                    lockInput = lockMovement = true;
                    cc.m_lockDirection = true;

                    if (recoveryTime > recoveryTimer)
                    {
                        //lockInput = lockMovement = false;
                    }
                    recoveryTimer -= Time.deltaTime;
                }
                else
                {
                    recentlyAttacked = false;
                }
            }
            else
            {
                if (movementFreezeLength.cancelTime > 0)
                {
                    lockMovement = true;
                }
                else if (movementFreezeLength.cancelTime < 0)
                {
                    lockMovement = false;
                    recentlyAttacked = false;
                }
            }


            //print(movementFreezeLength.cancelTime + " " + movementFreezeLength.defaultTime);


            if (recentlyAttacked && lockMovement != true)
            {
                // Player was just attacked

                print("recovery time set: " + recoveryTimer);
                recoveryTime = 0;
                attackIsInQueue = false;
                attackInQueue = null;
                attackInProgress = false;
                lockMovement = true;
            }
            else if (recentlyAttacked)
            {
                print("recovering");
                recoveryTimer -= Time.deltaTime;

                if (recoveryTime > recoveryTimer)
                {
                    print("recovered");

                    if (cc.m_Grounded)
                    {
                        // Insert some lag time and stuff
                        lockMovement = recentlyAttacked = false;
                    }
                    else
                    {
                        if (jump)
                        {
                            RecoveryJump();
                        }
                    }



                }
            }


            if (!lockMovement)
            {
                Move();
            }
            else
            {
                //input.joystickPosition = 5;
                //input.joystickRecord.Clear();
            }

            isFacingRight = cc.m_FacingRight;


            if (cc.m_Grounded && IsGrounded())
            {
                cc.m_doubleJumpUsed = doubleJumpUsed = false;
                //jumpCount = 0;
                specialJump = false;
                isLeaping = isBackLeaping = isBackStepping = false;
                cc.m_AirControl = true;
            }

            if (attackIsInQueue)
            {
                if (!attackInProgress)
                {

                }
            }

            movementFreezeLength.Decrement();
            //print(movementFreezeLength.cancelTime);

            //SetVibration((castTime / 100));
            cc.m_lockDirection = lockMovement;
            //print("Leaping: " + isLeaping + " BackStep: " + isBackStepping + "BackLeaping: " + isBackLeaping);

            comboTime += Time.deltaTime;

            if (comboTime > comboTimer)
            {
                comboCount = 0;
            }
        }
		
        

        CameraShake();
    }

    private void Move()
    {
        if (!lockMovement)
        {
            if (input.joystickPosition == 4 || input.state.ThumbSticks.Left.X < -input.deadzone)
            {
                if (!isDashing)
                {
                    horizontalMove = -(speed);
                }
                else
                {
                    horizontalMove = -(runSpeed);
                }
            }
            else if (input.joystickPosition == 6 || input.state.ThumbSticks.Left.X > input.deadzone)
            {
                if (!isDashing)
                {
                    horizontalMove = speed;
                }
                else
                {
                    horizontalMove = runSpeed;
                }
            }
            else
            {
                horizontalMove = 0;
            }

            
        }
        else
        {
            horizontalMove = 0;
        }
        
    }

    void RecoveryJump()
    {
        Vector2 finalVelocity = recoveryVelocity;


        if (input.joystickPosition == 6)
        {
            finalVelocity.x *= Mathf.Abs(finalVelocity.x);
        }
        else if(input.joystickPosition == 4)
        {
            finalVelocity.x *= -Mathf.Abs(finalVelocity.x);
        }
        else
        {
            finalVelocity.x *= cc.m_FacingRight ? 1 : -1;
        }
        

        rb.velocity = (finalVelocity);
        lockMovement = recentlyAttacked = false;
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            if (rb.velocity.y < 0)
            {

                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1);

            }
            else if (rb.velocity.y > 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 0.8f);
            }

            print(jump);

            if (jump && cc.m_jumpCount < 2 && rb.velocity.y < 0 && !recentlyAttacked)
            {
                rb.velocity = Vector2.zero;
            }

            //move character
            if (!lockMovement && !specialJump)
            {
                cc.Move(horizontalMove, jump && cc.m_jumpCount < 2);

                if (cc.m_jumpCount > 0 && jump)
                {
                    doubleJumpUsed = true;

                }

                if (cc.m_doubleJumpEnabled)
                {
                    if (cc.m_jumpCount < 1)
                    {

                        //jumpCount++;
                    }
                }

                if (cc.m_jumpCount > 1)
                {
                    cc.m_doubleJumpUsed = doubleJumpUsed = true;
                }


            }
            else if (!lockMovement && specialJump)
            {
                if (isLeaping)
                {
                    print("is leaping");
                    Leap();
                }
                else if (isBackLeaping)
                {
                    BackLeap();
                }
                else if (isBackStepping)
                {
                    BackStep();
                }
            }
        }
        

        jump = false;
        horizontalMove = 0;
        
    }

    public bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, sr.sprite.bounds.extents.y);
    }

    public void Leap()
    {
        cc.m_AirControl = false;
        //jumpCount += 2;
        specialJump = true;
        int direction = cc.m_FacingRight ? 1 : -1;
        cc.Move(forwardLeapSpeed * direction, jump, PlayerController2D.JumpType.Long);
        canDoubleJump = false;
    }

    public void BackLeap()
    {
        cc.m_AirControl = false;
        //jumpCount += 2;
        specialJump = true;
        cc.Move(backwardLeapSpeed, jump, PlayerController2D.JumpType.Back);
        canDoubleJump = false;
    }

    public void BackStep()
    {
        cc.m_AirControl = false;
        //jumpCount += 2;
        specialJump = true;
        cc.Move(backStepSpeed, jump, PlayerController2D.JumpType.Back);
        canDoubleJump = false;
    }

    public void SetVibration(float vibration)
    {
        input.vibrateLeftMotor = input.vibrateRightMotor = vibration;
    }

    public void SetVibration(float leftMotor, float rightMotor)
    {
        input.vibrateLeftMotor = leftMotor;
        input.vibrateRightMotor = rightMotor;

        print("l: " + leftMotor + " r: " + rightMotor);
    }

    public void SetVibration(float leftMotor, float rightMotor, float vibrationLength)
    {
        input.vibrateLeftMotor = leftMotor;
        input.vibrateRightMotor = rightMotor;

        print("l: " + leftMotor + " r: " + rightMotor);
    }

    private void CameraShake()
    {
       if (cameraShakeDuration > 0)
        {
            cameraFocusPoint.transform.position = new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle * cameraShakeAmount;
            cameraShakeDuration -= Time.deltaTime;
        }
        else
        {
            cameraFocusPoint.transform.localPosition = Vector3.zero;
            cameraShakeAmount = 0;
        }
    }

    public void ShakeCamera(float duration, float intensity)
    {
        cameraShakeDuration = duration;
        cameraShakeAmount = intensity;
    }

    void Respawn()
    {
        transform.position = rm.activeSpawnPoint.position;
        anim.SetBool("death", false);
        respawnCalled = isDead = false;
        health.currentHealth = health.maxHealth;
    }

    public void IncrementComboChain(Attack atk)
    {
        print("Yeet");

        if(comboTime < comboTimer)
        {
            print("combo Inc");
            comboCount++;
            comboTimer -= comboCount/64;
            comboTime = 0;
        }
        else if(comboTime > comboTimer)
        {
            print("combo Inc");
            comboCount = 0;
            comboTimer = defaultComboTime;
        }

        comboTime = 0;        
    }

    public void SetHitstunTimer(DoubleTime recovery)
    {
        recoveryTimer = recovery.defaultTime;
        recoveryTime = recovery.cancelTime;
    }

	public void isLanding() {
			
	
	}

    IEnumerator SlowReload()
    {
        yield return new WaitForSeconds(4);
        Respawn();
    }

	public void particlePlayer() {
		if (IsGrounded()) {
			Instantiate (dustParticle, dustParticleSpawn.position, Quaternion.identity);
		}
	}

    private void OnLevelWasLoaded(int level)
    {
        
    }
}
