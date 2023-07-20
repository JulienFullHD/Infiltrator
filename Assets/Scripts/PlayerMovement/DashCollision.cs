using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCollision : MonoBehaviour
{
    [SerializeField] private Dashing dashingScript;

    private void OnTriggerEnter(Collider other)
    {
        dashingScript.OnDashTriggerEnter(other);
    }
}
