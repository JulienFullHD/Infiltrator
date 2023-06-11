using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Kunai : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Material material;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private PlayerWeaponsManager weaponManager;
    private Vector3 LastPos;

    public void Init(float _launchSpeed, PlayerWeaponsManager _weaponManager)
    {
        weaponManager = _weaponManager;
        meshRenderer.material = new Material(material);
        rb.AddForce(transform.forward * _launchSpeed, ForceMode.Impulse);
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (Physics.Linecast(transform.position, LastPos,out RaycastHit collision))
        {
            if (collision.transform.gameObject.CompareTag("Enemy"))
            {
                weaponManager.HitEnemy(hitType: HitType.Kunai, enemyGameObject: collision.transform.gameObject);

                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
        LastPos = transform.position;

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"KUNAI HIT: {collision.gameObject.name}");

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        //gameObject.tag = "CollectableKunai";
        //gameObject.layer = 0;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            weaponManager.HitEnemy(hitType: HitType.Kunai, enemyGameObject: collision.gameObject);
        }

        //StartCoroutine(VanishingSequence(5));
    }

    private IEnumerator VanishingSequence(float secondsToVanish)
    {
        Color temp = meshRenderer.material.color;
        while(temp.a > 0)
        {
            yield return new WaitForSeconds(0.2f);
            temp.a -= 0.2f / secondsToVanish;
            meshRenderer.material.color = temp;
        }

        Destroy(gameObject);
        //Duration = Time from Alpha 100 -> 0

        
    }
}
