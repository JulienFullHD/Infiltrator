using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speedSprint;
    [SerializeField] private float speedDash;
    [SerializeField] private float speedWallrunning;
    [ReadOnly, SerializeField] private float speedDesired;
    [ReadOnly, SerializeField] private float speedLastDesired;
    [ReadOnly, SerializeField] private float speedToApply;
    [ReadOnly, SerializeField] private bool speedIsChanged;
    [ReadOnly, SerializeField] private bool keepMomentun;
    [Space(10f)]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform orientation;
    [SerializeField] private float groundDrag;
    [ReadOnly, SerializeField] private float speedForceMultiplier = 10f;
    [ReadOnly, SerializeField] private float horizontalInput;
    [ReadOnly, SerializeField] private float verticalInput;
    [ReadOnly, SerializeField] private Vector3 moveDirection;
    [ReadOnly, SerializeField] private Vector3 flatVelocity;
    [ReadOnly, SerializeField] private Vector3 limitedVelocity;

    [Space(20f)]
    [Header("Ground Check Settings")]
    [SerializeField] private LayerMask groundLayers;
    [ReadOnly, SerializeField] public bool isGrounded;
    [ReadOnly, SerializeField] public bool isGroundedLastFrame;
    [ReadOnly, SerializeField] public bool isGroundedTrigger;

    [Header("Dash Settings")]
    [SerializeField] private float speedChangeFactor;
    [ReadOnly, SerializeField] public bool isDashing;

    [Header("Jumping Settings")]
    [SerializeField] private float jumpStrength;
    //[SerializeField] private float jumpCooldown; //Remnant from before CoyoteTime
    [SerializeField] private float aerialSpeedMultiplier;
    [ReadOnly, SerializeField] private bool canJump;

    [Header("Coyote Time")]
    [SerializeField] private float maxCoyoteTime;
    [ReadOnly, SerializeField] private bool coyoteAllowJump;
    [ReadOnly, SerializeField] private float timeInAir;

    [Header("Wallrun Settings")]
    [ReadOnly, SerializeField] public bool isWallrunning;

    [Header("Sound Settings")]
    [SerializeField] private float landingSoundCooldownDuration;
    [ReadOnly, SerializeField] private float landingSoundCooldownTimer;


    //Wwise
    private bool FootstepIsPlaying = false;
    private float lastFootstepTime = 0;
    [Header("Wwise Events")]
    public AK.Wwise.Event myFootstep;
    public AK.Wwise.Event myJump;
    public AK.Wwise.Event myLand;

    [SerializeField] private Animator animator;


    private void Start()
    {
        canJump = true;
    }

    private void Update()
    {
        GroundCheck();
        CoyoteTime();
        Inputs();
        SpeedControl();
        DragControl();
        //Wwise
        if (rb.velocity.magnitude > 0.4 && isGrounded)
        {
            if (!FootstepIsPlaying && lastFootstepTime > 0.20)
            {
                myFootstep.Post(gameObject);
                lastFootstepTime = 0;
            }
            lastFootstepTime += Time.deltaTime;
            FootstepIsPlaying = false;
        }
    }   

    /// <summary>
    /// Check if player is grounded or not
    /// </summary>
    private void GroundCheck()
    {
        isGroundedLastFrame = isGrounded;

        isGrounded = isGroundedTrigger;

        if (!isGroundedLastFrame && isGrounded && landingSoundCooldownTimer <= 0)
        {
            myLand.Post(gameObject); //Wwise




            landingSoundCooldownTimer = landingSoundCooldownDuration;
        }

        if(landingSoundCooldownTimer > 0)
        {
            landingSoundCooldownTimer -= Time.deltaTime;
        }

        //isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + extraRaycastDistance, groundLayers);
    }

    /// <summary>
    /// Allow jumping for a short time after losing grounded status
    /// </summary>
    private void CoyoteTime()
    {
        if (isGrounded)
        {
            timeInAir = 0;
            coyoteAllowJump = true;

            //Execute if player landed this frame
            if (!isGroundedLastFrame)
            {
                canJump = true;
            }
        }
        else
        {
            timeInAir += Time.deltaTime;

            if(timeInAir < maxCoyoteTime)
            {
                coyoteAllowJump = true;
            }
            else
            {
                coyoteAllowJump = false;
            }
        }
    }
    
    /// <summary>
    /// Removes drag when in the air
    /// </summary>
    private void DragControl()
    {
        if (isGrounded && !isDashing)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    /// <summary>
    /// Test for jump input and movement keys
    /// </summary>
    private void Inputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(!PauseMenu.isPaused && Input.GetKey(KeyCode.Space) && canJump && coyoteAllowJump)
        {
            canJump = false;
            Jump();
        }
    }

    /// <summary>
    /// Limits the maximum rigidbody speed while in different states
    /// </summary>
    private void SpeedControl()
    {
        if (isDashing)
        {
            speedDesired = speedDash;
        }
        else if (isWallrunning)
        {
            speedDesired = speedWallrunning;
        }
        else
        {
            speedDesired = speedSprint;
        }

        speedIsChanged = speedLastDesired != speedDesired;
        speedLastDesired = speedDesired;

        if (speedIsChanged)
        {
            if (!isDashing)
            {
                StopAllCoroutines();
                StartCoroutine(SpeedLerp());
            }
            else
            {
                StopAllCoroutines();
                speedToApply = speedDesired;
            }
        }

        //Horizontal
        flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVelocity.magnitude > speedToApply)
        {
            limitedVelocity = flatVelocity.normalized * speedToApply;


            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);

        }

        //Vertical
        if(rb.velocity.y > 0f)
        {
            flatVelocity = new Vector3(0f, rb.velocity.y, 0f);

            if (flatVelocity.magnitude > speedToApply)
            {
                limitedVelocity = flatVelocity.normalized * speedToApply;              

                rb.velocity = new Vector3(rb.velocity.x, limitedVelocity.y, rb.velocity.z); ;

   
            }
        }
    }

    /// <summary>
    /// Smoothly change the maximum rigidbody speed to not make the player suddenly stop
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpeedLerp()
    {
        float time = 0;
        float diff = Mathf.Abs(speedDesired - speedToApply);
        float startValue = speedToApply;

        while(time < diff)
        {
            speedToApply = Mathf.Lerp(startValue, speedDesired, time / diff);

            time += Time.deltaTime * speedChangeFactor;

            yield return null;
        }

        speedToApply = speedDesired;
        isDashing = false;
    }

    /// <summary>
    /// Jump into the air
    /// </summary>
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpStrength, ForceMode.Impulse);

        
        myJump.Post(gameObject); //Wwise
    }

    /// <summary>
    /// Allow jumping
    /// </summary>
    public void ResetJump()
    {
        canJump = true;
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    isGroundedTrigger = true;
    //}


    // - - - - - - - - - - - - - - - Fixed Update stuff - - - - - - - - - - - - - - 
    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// Moves player using Forces
    /// </summary>
    private void Move()
    {        
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if(moveDirection.magnitude == 0)animator.SetBool("Run", false);

        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * speedToApply * speedForceMultiplier, ForceMode.Force);
            if(moveDirection.magnitude > 0)animator.SetBool("Run", true);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * speedToApply * speedForceMultiplier * aerialSpeedMultiplier, ForceMode.Force);
            animator.SetBool("Run", false);
        }
    }
}
