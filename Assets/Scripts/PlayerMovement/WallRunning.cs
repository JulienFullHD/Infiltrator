using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning Settings")]
    [SerializeField] private LayerMask wallLayers;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float wallrunForce;
    [SerializeField] private float wallrunUpwardsForce;
    [SerializeField] private float wallJumpUpForce;
    [SerializeField] private float wallJumpSideForce;
    [SerializeField] private float maxWallrunTime;
    [ReadOnly, SerializeField] private float wallrunTimer;
    [ReadOnly, SerializeField] private Vector3 forceToApply;
    [ReadOnly, SerializeField] private Vector3 wallSurfaceNormal;
    [ReadOnly, SerializeField] private Vector3 wallrunDirection;
    [ReadOnly, SerializeField] private bool wallrunStartUseGravity;

    [Header("Exit Wallrun Settings")]
    [SerializeField] private bool exitWallRequiresGroundedToggle;
    [ReadOnly, SerializeField] private bool exitWallRequiresGrounded;
    [SerializeField] private float exitWallTimeOnJump;
    [SerializeField] private float exitWallTimeOnFall;
    [ReadOnly, SerializeField] private bool exitingWall;
    [ReadOnly, SerializeField] private float exitWallTimer;

    [Header("Input")]
    [SerializeField] private KeyCode upwardsWallrunKey;
    [SerializeField] private bool alwaysRise;
    private float horizontalInput;
    private float verticalInput;

    [Header("Wall Detection")]
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float minJumpHeight;
    [SerializeField] private RaycastHit leftWallhit;
    [SerializeField] private RaycastHit rightWallhit;
    [ReadOnly, SerializeField] private bool wallLeftExists;
    [ReadOnly, SerializeField] private bool wallRightExists;

    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private PlayerMovement movementManager;
    [SerializeField] private Rigidbody rb;

    private void Update()
    {
        if (movementManager.isWallrunning)
            Debug.Log("Wallrunning");

        HandleInput();
        CheckForWall();
        HandleWallrunState();        
    }

    private void FixedUpdate()
    {
        if (movementManager.isWallrunning)
        {
            WallRunningMovement();
        }
    }

    private void CheckForWall()
    {
        wallRightExists = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, wallLayers);
        wallLeftExists = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, wallLayers);
    }

    private bool InAir()
    {
        //Better solution lol
        //return !movementManager.isGrounded;

        //Check if player is in air above ground by at least minJumpHeight
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, groundLayers);
    }

    private void HandleInput()
    {
        // Framerate indepentant input values for movement keys ASWD
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void HandleWallrunState()
    {        
        // Needs to touch wall, press forward and be in air
        if((wallLeftExists || wallRightExists) && verticalInput > 0 && InAir() && rb.velocity.y > -30 && !exitingWall)
        {
            // Start wallrun
            if (!movementManager.isWallrunning)
            {
                StartWallRun();
            }

            if(wallrunTimer > 0)
            {
                wallrunTimer -= Time.deltaTime;
            }
            else if(movementManager.isWallrunning)
            {
                exitingWall = true;
                exitWallTimer = exitWallTimeOnFall;

                if (exitWallRequiresGroundedToggle)
                {
                    exitWallRequiresGrounded = true;
                }
            }


            if (Input.GetKeyDown(KeyCode.Space))
            {
                WallJump();
            }
        }
        else if (exitingWall)
        {
            if (movementManager.isWallrunning)
            {
                StopWallRun();
            }

            if(exitWallTimer > 0)
            {
                exitWallTimer -= Time.deltaTime;
            }
            else if(!(exitWallRequiresGrounded && !movementManager.isGrounded))
            {
                //If doesnt require and not grounded
                //If doesnt require and grounded
                //NOT If require and not grounded
                //If require and grounded
                //If jumping


                exitingWall = false;
                exitWallRequiresGrounded = false;
            }
        }
        else
        {
            if (movementManager.isWallrunning)
            {
                exitingWall = true;
                exitWallTimer = exitWallTimeOnFall;

                if (exitWallRequiresGroundedToggle)
                {
                    exitWallRequiresGrounded = true;
                }

                StopWallRun();
            }
        }
    }

    private void StartWallRun()
    {
        movementManager.ResetJump();

        wallrunStartUseGravity = true;

        movementManager.isWallrunning = true;

        //rb.useGravity = false;
        wallrunTimer = maxWallrunTime;
    }
    
    private void WallRunningMovement()
    {
        // Use Gravity while initially going upwards
        if (rb.velocity.y < 0)
        {
            wallrunStartUseGravity = false;
            rb.useGravity = false;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        }
        else if (wallrunStartUseGravity)
        {
            rb.useGravity = true;
        }

        wallSurfaceNormal = wallRightExists ? rightWallhit.normal : leftWallhit.normal;

        wallrunDirection = Vector3.Cross(wallSurfaceNormal, transform.up);

        // Check if player is facing the opposite direction of the calculated cross Vector and if true, flip direction 
        if((orientation.forward - wallrunDirection).magnitude > (orientation.forward + wallrunDirection).magnitude)
        {
            wallrunDirection = -wallrunDirection;
        }

        // Forward force
        rb.AddForce(wallrunDirection * wallrunForce, ForceMode.Force);

        // Upwards force
        if (Input.GetKey(upwardsWallrunKey) || alwaysRise)
        {
            wallrunStartUseGravity = false;
            rb.velocity = new Vector3(rb.velocity.x, wallrunUpwardsForce, rb.velocity.z);
        }

        // Push against wall to allow wallrunning at reflex angles (>180°)
        if (!(wallLeftExists && horizontalInput > 0) || !(wallRightExists && horizontalInput < 0))
        {
            rb.AddForce(-wallSurfaceNormal * 100, ForceMode.Force);
        }
    }

    private void WallJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTimeOnJump;

        forceToApply = transform.up * wallJumpUpForce + wallSurfaceNormal * wallJumpSideForce;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }

    private void StopWallRun()
    {
        rb.useGravity = true;

        movementManager.isWallrunning = false;
    }
}
