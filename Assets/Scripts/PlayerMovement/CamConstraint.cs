using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CamConstraint : MonoBehaviour
{
    [SerializeField] private Transform cameraPosition;

    private void Update()
    {
        transform.position = cameraPosition.position;
    }
}
