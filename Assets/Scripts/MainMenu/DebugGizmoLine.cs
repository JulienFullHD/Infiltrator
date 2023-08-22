using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
