using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private PlayerMovement movementManager;

    private void FixedUpdate()
    {
        movementManager.isGroundedTrigger = false;
    }

    private void OnTriggerStay(Collider other)
    {
        movementManager.isGroundedTrigger = true;
    }
}
