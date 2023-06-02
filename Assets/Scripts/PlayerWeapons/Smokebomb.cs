using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smokebomb : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject particles;
    [SerializeField] private GameObject visualizer;
    [SerializeField] private PlayerWeaponsManager weaponManager;
    [SerializeField] private float durationMS;
    [SerializeField] private Vector3 stunHitbox;
    [ReadOnly, SerializeField] private Collider[] stunHits;

    public void Init(float _launchSpeed, PlayerWeaponsManager _weaponManager)
    {
        weaponManager = _weaponManager;

        rb.AddForce(transform.forward * _launchSpeed, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.detectCollisions = false;

        stunHits = Physics.OverlapBox(transform.position, stunHitbox / 2, Quaternion.identity, LayerMask.GetMask("Enemy"));

        if (stunHits.Length > 0)
        {
            for (int i = 0; i < stunHits.Length; i++)
            {
                Debug.Log($"Stunned: {stunHits[i].name}");

                weaponManager.HitEnemy(hitType: HitType.Smokebomb, enemyGameObject: stunHits[i].gameObject);
            }
        }

        visualizer.SetActive(false);
        particles.SetActive(true);
        Destroy(gameObject, durationMS/1000);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (particles.activeSelf)
        {
            Gizmos.DrawWireCube(transform.position, stunHitbox);
        }
    }
}
