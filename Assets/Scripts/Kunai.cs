using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Kunai : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Material material;
    [SerializeField] private MeshRenderer meshRenderer;

    public void Init(float launchSpeed)
    {
        meshRenderer.material = new Material(material);
        rb.AddForce(transform.forward * launchSpeed, ForceMode.Impulse);
        rb.freezeRotation = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"KUNAI HIT: {collision.gameObject.name}");

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        gameObject.tag = "CollectableKunai";
        gameObject.layer = 0;

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
