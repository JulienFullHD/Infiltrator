using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform camTransform;
    void Start() 
    {
        camTransform = Camera.main.transform;
    }
    void Update()
    {
        transform.LookAt(camTransform);
    }
}
