using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private PlayerMovement movementManager;
    [SerializeField] public KeyCode keybindDash = KeyCode.E;

    [Header("Dashing")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashUpForce;
    [SerializeField] private float dashDuration;
    [SerializeField] private Vector3 delayedForceToApply;
    [ReadOnly, SerializeField] private Vector3 forceToApply;
    [Space(10f)]
    [SerializeField] private bool disableGravity;
    [SerializeField] private bool resetVelocity;
    [ReadOnly, SerializeField] private Transform forwardTransform;


    [Header("Cooldown")]
    [SerializeField] private float dashCooldown;
    [ReadOnly, SerializeField] private bool canDash;

    private void Start()
    {
        canDash = true;
    }

    private void Update()
    {
        if (Input.GetKey(keybindDash) && canDash)
        {
            Dash();
        }
    }

    private void Dash()
    {
        canDash = false;
        Invoke(nameof(AllowDash), dashCooldown);

        movementManager.isDashing = true;

        forceToApply = playerCamera.forward * dashForce + orientation.up * dashUpForce;

        if (disableGravity)
        {
            rb.useGravity = false;
        }

        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);

        Invoke(nameof(ResetDash), dashDuration);
    }

    private void DelayedDashForce()
    {
        if (resetVelocity)
        {
            rb.velocity = Vector3.zero;
        }

        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        movementManager.isDashing = false;

        if (disableGravity)
        {
            rb.useGravity = true;
        }
    }

    private void AllowDash()
    {
        canDash = true;
    }
}
