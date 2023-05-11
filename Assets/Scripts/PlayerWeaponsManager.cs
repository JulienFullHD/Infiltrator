using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponsManager : MonoBehaviour
{
    [Header("Sword Settings")]
    [SerializeField] private Vector3 swordBoxDisplacement;
    [SerializeField] private Vector3 swordBoxSize;
    [SerializeField] private float preAttackCountdownMS;
    [SerializeField] private float swordAttackCooldownMS;
    [SerializeField] private bool canAttackSword;
    [SerializeField] private Collider[] swordHits;

    [Header("Kunai Settings")]
    [SerializeField] private GameObject kunaiPrefab;
    [SerializeField] private Transform kunaiLaunchLocation;
    [SerializeField] private float kunaiLaunchSpeed; //Reminder to change rigidbody to Continuous
    [SerializeField] private float kunaiAmount;
    [SerializeField] private SphereCollider collectionTrigger;

    [Header("Dash Settings")]
    [SerializeField] private float dashMaxDistance;
    [SerializeField] private float dashInterpDurationMS; //For position interpolation
    [SerializeField] private Vector3 dashMoveHitbox;
    [SerializeField] private Vector3 dashAttackHitbox;
    [SerializeField] private float preDashCountdownMS;
    [SerializeField] private float dashAttackCooldownMS;
    [SerializeField] private bool canDash;
    [SerializeField] private float calculatedDistance;
    [SerializeField] private float speedAfterDash;
    [SerializeField] private Rigidbody rb;
    private RaycastHit[] dashHits;

    [Header("Smokebomb Settings")]
    [SerializeField] private GameObject smokebombPrefab;
    [SerializeField] private Transform smokebombLaunchLocation;
    [SerializeField] private float smokebombLaunchSpeed;
    [SerializeField] private bool canThrowSmokebomb;

    [Header("Debug Settings")]
    [SerializeField] private bool showGizmo;
    [SerializeField] private float gizmoRadius;
    private Matrix4x4 rotationMatrix;
    private Vector3 dashStartPos;
    private Vector3 dashEndPos;

    private void Awake()
    {
        kunaiAmount = 3;
        canAttackSword = true;
        canDash = true;
        canThrowSmokebomb = true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Space))
        {
            ThrowKunai();
        }

        //FOR DEBUG ONLY; ENABLES FULL AUTO KUNAI SPRAY
        if (Input.GetKey(KeyCode.B))
        {
            AddKunai();
            ThrowKunai();
        }

        if (Input.GetKey(KeyCode.Mouse0) && canAttackSword)
        {
            canAttackSword = false;
            StartCoroutine(PreAttackCountdown(preAttackCountdownMS));
        }

        if(Input.GetKey(KeyCode.LeftShift) && canDash)
        {
            canDash = false;
            StartCoroutine(PreDashCountdown(preDashCountdownMS));
        }

        if (Input.GetKeyDown(KeyCode.E) && canThrowSmokebomb)
        {
            canThrowSmokebomb = false;

        }
    }

    private void AttackDash()
    {
        StartCoroutine(DashCooldown(swordAttackCooldownMS));

        //Check for physics collisions to not put player into walls
        //Physics.BoxCast with LayerMask Environment to check how far the dash could go
        if (Physics.BoxCast(transform.position, dashMoveHitbox / 2, transform.forward, out RaycastHit hitInfo, Quaternion.identity, dashMaxDistance, LayerMask.GetMask("World")))
        {
            calculatedDistance = hitInfo.distance;
        }
        else
        {
            calculatedDistance = dashMaxDistance;
        }


        //Physics.BoxCastAll start->end to check for all enemies hit within dash range
        dashHits = Physics.BoxCastAll(transform.position, dashAttackHitbox / 2, transform.forward, Quaternion.identity, calculatedDistance, LayerMask.GetMask("Enemy"));

        Debug.Log($"Dash hit {dashHits.Length} enemies");
        if (dashHits.Length > 0)
        {
            for (int i = 0; i < dashHits.Length; i++)
            {
                Debug.Log($"Hit: {dashHits[i].collider.name}");

                HitEnemy(hitType: HitType.Dash, enemyGameObject: dashHits[i].collider.gameObject);
            }
        }

        //Move Player forward
        rb.velocity = Vector3.zero;

        transform.position += transform.forward * calculatedDistance;
        
        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - TO DO - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        //UNCOMMENT WHEN IMPLEMENTED IN CHARACTE CONTROLLER:
        //rb.velocity = transform.forward * speedAfterDash;

        
        
        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - TO DO - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        // POSITION INTERPOLATION INSTEAD OF TELEPORTATION
    }

    private void AttackSword()
    {
        StartCoroutine(AttackCooldown(swordAttackCooldownMS));

        swordHits = Physics.OverlapBox(transform.position + (transform.rotation * swordBoxDisplacement), swordBoxSize / 2,Quaternion.identity, LayerMask.GetMask("Enemy"));

        //swordHits = Physics.OverlapBox(transform.position, swordBoxSize / 2,Quaternion.identity);

        Debug.Log($"Sword hit {swordHits.Length} enemies");
        if(swordHits.Length > 0)
        {
            for (int i = 0; i < swordHits.Length; i++)
            {
                Debug.Log($"Hit: {swordHits[i].name}");

                HitEnemy(hitType: HitType.Sword, enemyGameObject: swordHits[i].gameObject);
            }
        }
    }

    private void ThrowSmoke()
    {

    }

    private void ThrowKunai()
    {
        //Small amount of Kunais does not warrant use of pooling
        if(kunaiAmount > 0)
        {
            kunaiAmount--;

            GameObject kunai = Instantiate(original: kunaiPrefab, position: kunaiLaunchLocation.position, rotation: kunaiLaunchLocation.rotation);
            kunai.GetComponent<Kunai>().Init(_launchSpeed: kunaiLaunchSpeed, _weaponManager: this);

        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CollectableKunai"))
        {
            Destroy(obj: other.gameObject);
            AddKunai();
        }
    }

    public void AddKunai()
    {
        kunaiAmount++;
    }

    public void HitEnemy(HitType hitType, GameObject enemyGameObject)
    {

    }

    private void OnDrawGizmos()
    {
        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 2000f, ~LayerMask.GetMask("Player","Kunai")) && showGizmo)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(hitInfo.point, radius: gizmoRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, hitInfo.point);
        }

        if(!canAttackSword)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(swordBoxDisplacement, swordBoxSize);
            Gizmos.matrix = Matrix4x4.identity;
        }

        if (!canDash)
        {
            
        }
    }

    private IEnumerator PreAttackCountdown(float ms)
    {
        if(ms > 0)
        {
            yield return new WaitForSeconds(ms / 1000);
        }
        AttackSword();
    }
    private IEnumerator AttackCooldown(float ms)
    {
        if (ms > 0)
        {
            yield return new WaitForSeconds(ms / 1000);
        }
        canAttackSword = true;
    }

    private IEnumerator PreDashCountdown(float ms)
    {
        if (ms > 0)
        {
            yield return new WaitForSeconds(ms / 1000);
        }
        AttackDash();
    }

    private IEnumerator DashCooldown(float ms)
    {
        if (ms > 0)
        {
            yield return new WaitForSeconds(ms / 1000);
        }
        canDash = true;
    }
}

public enum HitType
{
    UNDEFINED,
    Sword,
    Kunai,
    Dash,
    Environment
}