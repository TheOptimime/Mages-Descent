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
    bool slowTurn;

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    [HideInInspector]
	public bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;

	public bool m_FacingRight = true;  // For determining which way the player is currently facing.
    public bool m_doubleJumpUsed, m_doubleJumpEnabled;

    public Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
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

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
                {
                    OnLandEvent.Invoke();
                    m_doubleJumpUsed = false;
                }
					
			}
		}
	}


	public void Move(float move, bool jump)
	{
      

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
            
			// If crouching
			

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

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
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
        else if(!m_Grounded && m_doubleJumpEnabled && !m_doubleJumpUsed && jump)
        {
                m_doubleJumpUsed = true;
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_DoubleJumpForce));
        }
	}

    public void Move(float move, bool jump, JumpType _jumpType)
    {

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

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
        // If the player should jump...
        if (m_Grounded && _jumpType == JumpType.Normal)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
        else if (!m_Grounded && m_doubleJumpEnabled && !m_doubleJumpUsed && jump)
        {
            m_doubleJumpUsed = true;
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_DoubleJumpForce));
        }
        else if (m_Grounded && _jumpType == JumpType.High)
        {
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_HighJumpForce));
            m_doubleJumpUsed = true;
            // might just change this to use a different air speed
        }
        else if (m_Grounded && _jumpType == JumpType.Long)
        {
            m_Grounded = false;
            m_Rigidbody2D.AddForce(m_LongJumpForce);
            m_doubleJumpUsed = true;
        }
        else if (m_Grounded && _jumpType == JumpType.Back)
        {
            m_Grounded = false;
            m_Rigidbody2D.AddForce(m_BackJumpForce);
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
