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
    [SerializeField] private float swordAttackCountdownMS;
    [SerializeField] private float swordAttackCooldownMS;
    [ReadOnly, SerializeField] private bool canAttackSword;
    [ReadOnly, SerializeField] private Collider[] swordHits;

    [Header("Kunai Settings")]
    [SerializeField] private GameObject kunaiPrefab;
    [SerializeField] private Transform kunaiLaunchLocation;
    [SerializeField] private float kunaiLaunchSpeed; //Reminder to change rigidbody to Continuous
    [SerializeField] private int kunaiAmount;

    [Header("Smokebomb Settings")]
    [SerializeField] private GameObject smokebombPrefab;
    [SerializeField] private Transform smokebombLaunchLocation;
    [SerializeField] private float smokebombLaunchSpeed;
    [SerializeField] private float smokebombAttackCooldownMS;
    [ReadOnly, SerializeField] private bool canThrowSmokebomb;

    [Header("Score Manager")]
    [SerializeField] public ScoreManger scoreManager;

    [Header("Debug Settings")]
    [SerializeField] private bool showGizmo;
    [SerializeField] private float gizmoRadius;

    private void Awake()
    {
        kunaiAmount = 300;
        canAttackSword = true;
        canThrowSmokebomb = true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            ThrowKunai();
        }

        if (Input.GetKey(KeyCode.Mouse0) && canAttackSword)
        {
            canAttackSword = false;
            Invoke(nameof(AttackSword), swordAttackCountdownMS);
        }

        if (Input.GetKeyDown(KeyCode.F) && canThrowSmokebomb)
        {
            canThrowSmokebomb = false;
            ThrowSmoke();
        }

        #region FOR_TESTING_ONLY
        //FOR DEBUG ONLY; ENABLES FULL AUTO KUNAI SPRAY
        if (Input.GetKey(KeyCode.B))
        {
            AddKunai();
            ThrowKunai();
        }

        //Give to 3 Kunais
        if (Input.GetKeyDown(KeyCode.N))
        {
            AddKunai();
            AddKunai();
            AddKunai();
        }
        #endregion

    }

    private void AttackSword()
    {
        Invoke(nameof(AllowMelee), swordAttackCooldownMS/1000);

        swordHits = Physics.OverlapBox(transform.position + (transform.rotation * swordBoxDisplacement), swordBoxSize / 2,Quaternion.identity, LayerMask.GetMask("Enemy"));

        //swordHits = Physics.OverlapBox(transform.position, swordBoxSize / 2,Quaternion.identity);

        Debug.Log($"Sword hit {swordHits.Length} enemies");
        if(swordHits.Length > 0)
        {
            for (int i = 0; i < swordHits.Length; i++)
            {
                HitEnemy(hitType: HitType.Sword, enemyGameObject: swordHits[i].gameObject);   
            }
        }
    }

    private void ThrowSmoke()
    {
        Invoke(nameof(AllowSmokeThrow), smokebombAttackCooldownMS/1000);

        GameObject smokebomb = Instantiate(original: smokebombPrefab, position: smokebombLaunchLocation.position, rotation: smokebombLaunchLocation.rotation);
        smokebomb.GetComponent<Smokebomb>().Init(_launchSpeed: smokebombLaunchSpeed, _weaponManager: this);
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
        //Tell enemy they were hit, and what by (Smokebomb; no damage, only stun)
        //Tell score manager about the hit
        Debug.Log($"HitEnemy({hitType}, {enemyGameObject.name})");
        
        if(hitType == HitType.Kunai ||
           hitType == HitType.Sword ||
           hitType == HitType.Dash)
        {
            Debug.Log(enemyGameObject.transform.parent.name);
            if(enemyGameObject.transform.parent.TryGetComponent<AI_HPSystem>(out AI_HPSystem aI_HPSystem))aI_HPSystem.TakeDamage(1);
            
            scoreManager.HitToScore(hitType: hitType);
        }
        
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

        if(!canAttackSword && showGizmo)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(swordBoxDisplacement, swordBoxSize);
            Gizmos.matrix = Matrix4x4.identity;
        }
    }


    private void AllowMelee()
    {
        canAttackSword = true;
    }

    private void AllowSmokeThrow()
    {
        canThrowSmokebomb = true;
    }
}

public enum HitType
{
    UNDEFINED,
    Sword,
    Kunai,
    Dash,
    Smokebomb,
    Environment
}