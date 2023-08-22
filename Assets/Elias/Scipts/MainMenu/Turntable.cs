using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turntable : MonoBehaviour
{
    [SerializeField] private float spinSpeed;
    [SerializeField] private bool rotationIsLocal;
    [ReadOnly, SerializeField] private Vector3 rotate;
    private Vector3 defaultRotation;

    /// <summary>
    /// Sets start values
    /// </summary>
    private void Start()
    {
        defaultRotation = transform.eulerAngles;

        if (rotationIsLocal)
        {
            rotate = transform.localEulerAngles;
        }
        else
        {
            rotate = transform.eulerAngles;
        }
    }

    /// <summary>
    /// Reset rotations to default values
    /// </summary>
    public void ResetRotation()
    {
        rotate.y = defaultRotation.y;
        transform.localEulerAngles = rotate;
    }

    /// <summary>
    /// Rotates gameObject
    /// </summary>
    private void Update()
    {
        if (rotationIsLocal)
        {
            rotate.y += spinSpeed * Time.deltaTime;
            transform.localEulerAngles = rotate;
        }
        else
        {
            rotate.y += spinSpeed * Time.deltaTime;
            transform.eulerAngles = rotate;
        }
    }
}
