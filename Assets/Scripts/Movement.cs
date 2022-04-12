using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Movement : MonoBehaviour
{
    [SerializeField] float fJumpVelocity = 5;
    public GameObject bullet;
    public GameObject spawnLocation;
    private bool canFire = true;
    public float attackSpeed = 1f;
    private float currentTime;
    public int runSpeed = 10;
    int horizontalMovement = 0;
    public Animator animator;
    public Vector2 playerVelocity;


    float fJumpPressedRemember = 0;
    [SerializeField]
    float fJumpPressedRememberTime = 0.2f;

    float fGroundedRemember = 0;
    [SerializeField]
    float fGroundedRememberTime = 0.25f;

    [SerializeField]
    float fHorizontalAcceleration = 1;
    [SerializeField]
    [Range(0, 1)]
    float fHorizontalDampingBasic = 0.5f;
    [SerializeField]
    [Range(0, 1)]
    float fHorizontalDampingWhenStopping = 0.5f;
    [SerializeField]
    [Range(0, 1)]
    float fHorizontalDampingWhenTurning = 0.5f;

    [SerializeField]
    [Range(0, 1)]
    float fCutJumpHeight = 0.5f;
    //  [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    public static Movement instance;
    const float k_GroundedRadius = .3f; // Radius of the overlap circle to determine if grounded
    public bool m_Grounded;            // Whether or not the player is grounded.
    int jumpCount = 0;
    public Rigidbody2D m_Rigidbody2D;
    public bool m_FacingRight = true;  // For determining which way the player is currently facing.

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private bool doubleJump = false;
    public int dashCount = 2;
    public int availableDash;

    public bool isHit = false;
    public bool isFiring = false;
    public bool canRun = false;
    //bool wasGrounded;

    private void Awake()
    {
        instance = this;
        availableDash = dashCount;
        animator = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        currentTime = attackSpeed;


        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    private void Update()
    {
        horizontalMovement = Convert.ToInt16(Input.GetAxisRaw("Horizontal"));
        animator.SetInteger("movement", horizontalMovement);
        Jump();
        animator.SetBool("isFiring", isFiring);
        animator.SetBool("isHit", isHit);
        if (playerVelocity.x > 12 || playerVelocity.x < -12)
            playerVelocity.x = 12 * this.transform.localScale.x;

        Fire();

        Move((horizontalMovement * runSpeed) * Time.fixedDeltaTime);



    }
    public void Jump()      // Jump Change the Y position of the player, the longer you hold the higher the jump,
    {
        playerVelocity = m_Rigidbody2D.velocity;
        fGroundedRemember -= Time.deltaTime;    // time when grounded (on Ground)
        if (m_Grounded)
        {
            fGroundedRemember = fGroundedRememberTime;
        }
        fJumpPressedRemember -= Time.deltaTime;     //time while holding
        if (Input.GetButtonDown("Jump"))
        {
            fJumpPressedRemember = fJumpPressedRememberTime;
        }
        if (Input.GetButtonUp("Jump"))              //change the velocity of the player Main Jump Component
        {
            if (m_Rigidbody2D.velocity.y > 0)
            {
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_Rigidbody2D.velocity.y * fCutJumpHeight);
            }
        }

        if ((fJumpPressedRemember > 0) && (fGroundedRemember > 0))          //Reset the jump variables
        {
            fJumpPressedRemember = 0;
            fGroundedRemember = 0;
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, fJumpVelocity);
        }
    }

    private void FixedUpdate()
    {


        //if (jumpCount == 2)
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {

                m_Grounded = true;
                jumpCount = 0;

            }

        }
        //       Jump();
    }


    public void Move(float move)
    {

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {

            Vector2 movement = m_Rigidbody2D.velocity;
            float fHorizontalVelocity = m_Rigidbody2D.velocity.x;
            fHorizontalVelocity += move;

            // These if, else if, else is a calculation of the stopping motion of the player
            if (Mathf.Abs(move) < 0.01f)
                fHorizontalVelocity *= Mathf.Pow(1f - fHorizontalDampingWhenStopping, Time.deltaTime * 10f);
            else if (Mathf.Sign(move) != Mathf.Sign(fHorizontalVelocity))
                fHorizontalVelocity *= Mathf.Pow(1f - fHorizontalDampingWhenTurning, Time.deltaTime * 10f);
            else
                fHorizontalVelocity *= Mathf.Pow(1f - fHorizontalDampingBasic, Time.deltaTime * 10f);

            m_Rigidbody2D.velocity = new Vector2(fHorizontalVelocity, m_Rigidbody2D.velocity.y);

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

        // FLip The sprite left or right
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }



    // Spawns a Bullet then launches it to the direction of the player is facing
    public void Fire()
    {
        if (Input.GetKey(KeyCode.Z) && canFire) // if canFire == true and Z is pressed
        {
            isFiring = true;
            Rigidbody2D rb = GetComponent<Rigidbody2D>(); 

            // Spawn a Projectile
            GameObject projectile = Instantiate(bullet, spawnLocation.transform.position, Quaternion.identity);
            Vector2 scale = projectile.transform.localScale;
            // Launch to the direction of the player 
            Projectile proj = projectile.GetComponent<Projectile>();
            projectile.GetComponent<Rigidbody2D>().AddForce((Vector2.right) * 50);
            projectile.transform.localScale = new Vector2(scale.x * this.transform.localScale.x, scale.y);
            if (!canFire && currentTime != 0)
            {

            }
            canFire = false;
        }
        // reset the time the player can attack
        else if (!canFire)
        {
            if (currentTime > 0)
                currentTime -= Time.deltaTime;
            else
            {
                canFire = true;
                currentTime = attackSpeed;
            }

        }

    }
}
