using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Debug Script to draw lines between objects
// Used to visualise the camera path in the main menu scene
public class DebugGizmoLine : MonoBehaviour
{
    public Transform drawLineTo;

    private void OnDrawGizmos()
    {
        if (drawLineTo != null) 
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, drawLineTo.position);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 4);
    }
}
