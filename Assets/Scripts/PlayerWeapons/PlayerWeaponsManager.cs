using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerWeaponsManager : MonoBehaviour
{
    [Header("Sword Settings")]
    [SerializeField] private Vector3 swordBoxDisplacement;
    [SerializeField] private Vector3 swordBoxSize;
    [SerializeField] private float swordAttackCountdownMS;
    [SerializeField] private float swordAttackCooldownMS;
    [ReadOnly, SerializeField] private bool canAttackSword;
    [ReadOnly, SerializeField] private Collider[] swordHits;
    [SerializeField] private VisualEffect slashEffect;

    [Header("Kunai Settings")]
    [SerializeField] private GameObject kunaiPrefab;
    [SerializeField] private Transform kunaiLaunchLocation;
    [SerializeField] private float kunaiLaunchSpeed; //Reminder to change rigidbody to Continuous
    [SerializeField] private int kunaiAmount;
    public int KunaiAmount
    {
        get
        {
            return kunaiAmount;
        }
        set
        {
            kunaiAmount = value;

            if(kunaiAmount < 0)
            {
                kunaiAmount = 0;
            }
            else if (kunaiAmount > 3)
            {
                kunaiAmount = 3;
            }

            abilityUI.ChangeKunaiAmmoUI(kunaiAmount);
        }
    }

    [Header("Smokebomb Settings")]
    [SerializeField] private GameObject smokebombPrefab;
    [SerializeField] private Transform smokebombLaunchLocation;
    [SerializeField] private float smokebombLaunchSpeed;
    [SerializeField] private float smokebombAttackCooldownMS;
    [ReadOnly, SerializeField] private bool canThrowSmokebomb;

    [Header("Score Manager")]
    [SerializeField] public ScoreManger scoreManager;

    [Header("Hitmarker Settings")]
    [SerializeField] private GameObject hitmarkerObject;
    [SerializeField] private float hitmarkerDisplayMS;


    [Header("Debug Settings")]
    [SerializeField] private bool showGizmo;
    [SerializeField] private float gizmoRadius;
    [SerializeField] private AbilityUI abilityUI;

    [Header("Wwise Events")]
    public AK.Wwise.Event myKunai;
    public AK.Wwise.Event mySmokebomb;
    public AK.Wwise.Event myKatanaSwing;
    

    private void Awake()
    {
        KunaiAmount = 3;
        canAttackSword = true;
        canThrowSmokebomb = true;        
    }

    private void Update()
    {
        if (!PauseMenu.isPaused && Input.GetKeyDown(UserSettings.Instance.KeybindKunai))
        {
            ThrowKunai();
        }

        if (!PauseMenu.isPaused && Input.GetKey(KeyCode.Mouse0) && canAttackSword)
        {
            myKatanaSwing.Post(gameObject);
            canAttackSword = false;
            Invoke(nameof(AttackSword), swordAttackCountdownMS);
        }

        if (!PauseMenu.isPaused && Input.GetKeyDown(UserSettings.Instance.KeybindSmokebomb) && canThrowSmokebomb)
        {
            canThrowSmokebomb = false;
            ThrowSmoke();
        }
    }

    /// <summary>
    /// Play sword animation and check for hits
    /// </summary>
    private void AttackSword()
    {
        slashEffect.Play();

        Invoke(nameof(AllowMelee), swordAttackCooldownMS/1000);

        swordHits = Physics.OverlapBox(transform.position + (transform.rotation * swordBoxDisplacement), swordBoxSize / 2,Quaternion.identity, LayerMask.GetMask("Enemy"));

        //swordHits = Physics.OverlapBox(transform.position, swordBoxSize / 2,Quaternion.identity);

        //Debug.Log($"Sword hit {swordHits.Length} enemies");
        if(swordHits.Length > 0)
        {
            for (int i = 0; i < swordHits.Length; i++)
            {
                HitEnemy(hitType: HitType.Sword, enemyGameObject: swordHits[i].gameObject);   
            }
        }
    }

    /// <summary>
    /// Throw a smokebomb to stun enemies
    /// </summary>
    private void ThrowSmoke()
    {
        abilityUI.StartSmokeCooldown(smokebombAttackCooldownMS / 1000);
        Invoke(nameof(AllowSmokeThrow), smokebombAttackCooldownMS/1000);

        GameObject smokebomb = Instantiate(original: smokebombPrefab, position: smokebombLaunchLocation.position, rotation: smokebombLaunchLocation.rotation);
        smokebomb.GetComponent<Smokebomb>().Init(_launchSpeed: smokebombLaunchSpeed, _weaponManager: this);

        //Wwise
        mySmokebomb.Post(gameObject);
    }

    /// <summary>
    /// Throw a Kunai 
    /// </summary>
    private void ThrowKunai()
    {
        //Small amount of Kunais does not warrant use of pooling
        if(KunaiAmount > 0)
        {
            KunaiAmount--;

            GameObject kunai = Instantiate(original: kunaiPrefab, position: kunaiLaunchLocation.position, rotation: kunaiLaunchLocation.rotation);
            kunai.GetComponent<Kunai>().Init(_launchSpeed: kunaiLaunchSpeed, _weaponManager: this);

            //Wwise
            myKunai.Post(gameObject);
            
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

    /// <summary>
    /// Add a Kunai if possible
    /// </summary>
    public void AddKunai()
    {
        if(kunaiAmount < 3)
            KunaiAmount++;
    }

    /// <summary>
    /// Add score, add it to combo, deal damage to enemies and display hitmarker
    /// </summary>
    /// <param name="hitType">Type of weapon</param>
    /// <param name="enemyGameObject">Reference to enemy</param>
    public void HitEnemy(HitType hitType, GameObject enemyGameObject)
    {
        //Tell enemy they were hit, and what by (Smokebomb; no damage, only stun)
        //Tell score manager about the hit
                
        if(hitType == HitType.Kunai ||
           hitType == HitType.Sword ||
           hitType == HitType.Dash)
        {
            if(enemyGameObject.transform.parent.TryGetComponent<AI_HPSystem>(out AI_HPSystem aI_HPSystem))
            {
                aI_HPSystem.TakeDamage(1);
                AddKunai();
            }
            
            scoreManager.HitToScore(hitType: hitType);

            // Display Hitmarker for x ms
            hitmarkerObject.SetActive(true);
            Invoke(nameof(HideHitmarker), hitmarkerDisplayMS/1000);
        }
        
    }

    /// <summary>
    /// Hide UI Hitmarker
    /// </summary>
    private void HideHitmarker()
    {
        hitmarkerObject.SetActive(false);
    }

#if UNITY_EDITOR
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
#endif

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