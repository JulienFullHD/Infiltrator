using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerWeaponsManager : MonoBehaviour
{
    [Header("Sword Settings")]
    [SerializeField] private Vector3 boxPosition;
    [SerializeField] private Vector3 boxSize;
    [SerializeField] private float swordAttackCooldown;

    [Header("Kunai Settings")]
    [SerializeField] private GameObject kunaiPrefab;
    [SerializeField] private Transform kunaiLaunchLocation;
    [SerializeField] private float kunaiLaunchSpeed; //Reminder to change rigidbody to Continuous
    [SerializeField] private float kunaiAmount;
    [SerializeField] private SphereCollider collectionTrigger;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private Vector2 hitboxHeightWidth;

    [Header("Smokebomb Settings")]
    [SerializeField] private GameObject smokebombPrefab;
    [SerializeField] private Transform smokebombLaunchLocation;
    [SerializeField] private float smokebombLaunchSpeed;

    [Header("Debug Settings")]
    [SerializeField] private bool showGizmo;
    [SerializeField] private float gizmoRadius;

    private void Awake()
    {
        kunaiAmount = 3;
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
    }

    private void AttackDash()
    {

    }

    private void AttackSword()
    {

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
            kunai.GetComponent<Kunai>().Init(launchSpeed: kunaiLaunchSpeed);
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

    public void HitEnemy(HitType hitType)
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
