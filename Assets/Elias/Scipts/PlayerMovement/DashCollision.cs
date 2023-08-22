using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is used to avoid conflict of having multiple triggers on the player
public class DashCollision : MonoBehaviour
{
    [SerializeField] private Dashing dashingScript;

    /// <summary>
    /// Calls specific method of dashing script
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        dashingScript.OnDashTriggerEnter(other);
    }
}
