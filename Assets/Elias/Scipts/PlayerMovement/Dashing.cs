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
    [SerializeField] private PlayerWeaponsManager weaponManager;

    [Header("Phasing through enemies")]
    [SerializeField] private float phaseTimer;
    [SerializeField] private GameObject collisionObject;
    [SerializeField] private CapsuleCollider attackCollider;
    [ReadOnly, SerializeField] private int enemiesHitPerDash;

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
    [SerializeField] private AbilityUI abilityUI;

    //Wwise
    [Header("Wwise Events")]
    public AK.Wwise.Event PlayerDash;


    private void Start()
    {
        canDash = true;
    }

    /// <summary>
    /// Test every frame if the dash button has been pressed
    /// </summary>
    private void Update()
    {
        if (!PauseMenu.isPaused && Input.GetKey(UserSettings.Instance.KeybindDash) && canDash)
        {
            Dash();
            PlayerDash.Post(gameObject);
        }

        if(attackCollider.enabled == true)
        {
            Debug.Log("Phasing");
        }
    }

    /// <summary>
    /// Start the dash procedure
    /// </summary>
    private void Dash()
    {
        StartPhasing();

        canDash = false;

        movementManager.isDashing = true;

        forceToApply = playerCamera.forward * dashForce + orientation.up * dashUpForce;

        if (disableGravity)
        {
            rb.useGravity = false;
        }

        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);

        Invoke(nameof(AllowDash), dashCooldown);
        abilityUI.StartDashCooldown(dashCooldown);

        Invoke(nameof(ResetDash), dashDuration);
    }

    /// <summary>
    /// Sets the player velocity after a dash to "keep" some momentum
    /// </summary>
    private void DelayedDashForce()
    {
        if (resetVelocity)
        {
            rb.velocity = Vector3.zero;
        }

        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    /// <summary>
    /// Stops the dash and reenables gravity
    /// </summary>
    private void ResetDash()
    {
        movementManager.isDashing = false;

        if (disableGravity)
        {
            rb.useGravity = true;
        }
    }

    /// <summary>
    /// Makes dash ability useable again
    /// </summary>
    private void AllowDash()
    {
        canDash = true;
    }

    /// <summary>
    /// Start the phasing procedure to allow phasing through enemies while dashing
    /// </summary>
    private void StartPhasing()
    {
        collisionObject.layer = LayerMask.NameToLayer(layerName: "PlayerDash");
        attackCollider.enabled = true;

        Invoke(nameof(StopPhasing), phaseTimer);
    }

    /// <summary>
    /// Stop phasing to collide with enemies again
    /// </summary>
    private void StopPhasing()
    {
        collisionObject.layer = LayerMask.NameToLayer(layerName: "Player");
        attackCollider.enabled = false;

        if(enemiesHitPerDash == 2)
        {
            // Double Dash
            weaponManager.scoreManager.AddScoreType(scoreType: ScoreManger.ScoreType.DoubleDash);
        }
        else if(enemiesHitPerDash >= 3)
        {
            // Triple Dash
            weaponManager.scoreManager.AddScoreType(scoreType: ScoreManger.ScoreType.TripleDash);
        }
        enemiesHitPerDash = 0;
    }

    /// <summary>
    /// Test to hit enemies while phasing
    /// </summary>
    /// <param name="other"></param>
    public void OnDashTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            weaponManager.HitEnemy(hitType: HitType.Dash, enemyGameObject: other.gameObject);
            enemiesHitPerDash++;
            Debug.Log("Dash hit");
        }
    }
}
