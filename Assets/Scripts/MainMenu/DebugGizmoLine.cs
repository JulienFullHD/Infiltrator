using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGizmoLine : MonoBehaviour
{
    public Transform pos1;
    public Transform pos2;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(pos1.position, pos2.position);
    }
}
