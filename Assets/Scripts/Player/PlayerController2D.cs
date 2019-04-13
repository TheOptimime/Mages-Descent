using UnityEngine;
using UnityEngine.Events;


public class PlayerController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f, m_HighJumpForce = 900,m_DoubleJumpForce = 400;                         // Amount of force added when the player jumps.
    [SerializeField] private Vector2 m_LongJumpForce = new Vector2(20,200), m_BackJumpForce = new Vector2(-20, 30), m_SlideForce = new Vector2(200,0);
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] public bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching
    public bool m_ceilingHold;
    public int m_jumpCount;
    bool slowTurn;

	const float k_GroundedRadius = .02f; // Radius of the overlap circle to determine if grounded
    public bool m_Grounded, m_lockDirection;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .02f; // Radius of the overlap circle to determine if the player can stand up
    public Rigidbody2D m_Rigidbody2D { get; private set; }

	public bool m_FacingRight = true;  // For determining which way the player is currently facing.
    public bool m_doubleJumpUsed, m_doubleJumpEnabled;
    public Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;
    
	private bool m_wasCrouching = false;
	


    

    public enum JumpType
    {
        Normal,
        High,
        Long,
        Back
    }



	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();


        

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
        
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
        
        m_Grounded = CheckGrounded();
        print("grounded: " + m_Grounded);

        
        Collider2D[] groundColliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < groundColliders.Length; i++)
		{
			if (groundColliders[i].gameObject != gameObject)
			{
                print("grounded is true");
				m_Grounded = true;
                m_jumpCount = 0;
				if (!wasGrounded)
                {
                    OnLandEvent.Invoke();
                    m_doubleJumpUsed = false;
                }
					
			}
		}

        //print(m_Velocity);

    }

    public void CeilingCling()
    {
        if (!m_Grounded)
        {
            Collider2D[] ceilingColliders = Physics2D.OverlapCircleAll(m_CeilingCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < ceilingColliders.Length; i++)
            {
                if (ceilingColliders[i].gameObject != gameObject)
                {
                    m_ceilingHold = true;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(m_GroundCheck.position, k_GroundedRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(m_CeilingCheck.position, k_GroundedRadius);
    }

    public bool CheckGrounded()
    {
       return Physics2D.Raycast(m_GroundCheck.transform.position, Vector2.down, 0.1f, m_WhatIsGround);
    }

    public void Move(float move, bool jump)
	{
        print("Move Function A");

        if (jump)
        {
            m_jumpCount++;
        }

        if (m_ceilingHold && m_Grounded != true && jump)
        {
            m_ceilingHold = false;
            jump = false;
        }

        if (!m_ceilingHold)
        {
            if (m_Grounded != true && m_Rigidbody2D.velocity.y == 0 && !jump)
            {
                //print("should fall");
                //m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_Rigidbody2D.gravityScale);
            }
        }
        

		if (!m_ceilingHold) {
			//only control the player if grounded or airControl is turned on
			if (m_Grounded || m_AirControl) {
				// Move the character by finding the target velocity
				Vector3 targetVelocity = new Vector2 (move * 10f, m_Rigidbody2D.velocity.y);
				// And then smoothing it out and applying it to the character
				m_Rigidbody2D.velocity = Vector3.SmoothDamp (m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

				// If the input is moving the player right and the player is facing left...
				if (!m_lockDirection) {
					if (move > 0 && !m_FacingRight || move < 0 && m_FacingRight) {
						// ... flip the player.
						Flip ();
					}
				}
                

			}
			// If the player should jump...
			if (m_Grounded && jump) {
				// Add a vertical force to the player.
				print ("single ready");
				m_Grounded = false;
				m_Rigidbody2D.velocity = (new Vector2 (0f, m_JumpForce));

				//m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
			} else if (!m_Grounded && m_doubleJumpEnabled && !m_doubleJumpUsed && jump) {
				print ("double ready");
				m_doubleJumpUsed = true;
				m_Rigidbody2D.velocity = new Vector2 (m_Rigidbody2D.velocity.x, 0);
				m_Rigidbody2D.velocity = (new Vector2 (0f, m_DoubleJumpForce));

			}


			
			
		

		}
	}




    public void Move(float move, bool jump, JumpType _jumpType)
    {
        print("Move Function B");

        if (jump)
        {
            m_jumpCount++;
        }

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            if (!m_lockDirection)
            {
                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            
        }
        // If the player should jump...
        if (m_Grounded && _jumpType == JumpType.Normal)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Rigidbody2D.velocity = (new Vector2(0f, m_JumpForce));

        }
        else if (!m_Grounded && m_doubleJumpEnabled && !m_doubleJumpUsed && jump)
        {
            m_doubleJumpUsed = true;
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
            m_Rigidbody2D.velocity = (new Vector2(0f, m_DoubleJumpForce));
        }
        else if (m_Grounded && _jumpType == JumpType.High)
        {
            m_Grounded = false;
            m_Rigidbody2D.velocity = (new Vector2(0f, m_HighJumpForce));
            m_doubleJumpUsed = true;
            // might just change this to use a different air speed
        }
        else if (m_Grounded && _jumpType == JumpType.Long)
        {
            m_Grounded = false;
            m_Rigidbody2D.velocity = (m_LongJumpForce);
            m_doubleJumpUsed = true;
        }
        else if (m_Grounded && _jumpType == JumpType.Back)
        {
            m_Grounded = false;
            m_Rigidbody2D.velocity = (m_BackJumpForce);
            m_doubleJumpUsed = true;
        }
        
    }

    public void FreezeVelocity()
    {
        m_Rigidbody2D.velocity = Vector2.zero;
    }

	public void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
