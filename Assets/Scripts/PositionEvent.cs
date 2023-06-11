using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PositionEvent : MonoBehaviour
{
    [SerializeField] private Collider positionDetection;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;

}
