using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turntable : MonoBehaviour
{
    [SerializeField] private float spinSpeed;
    [SerializeField] private bool rotationIsLocal;
    [ReadOnly, SerializeField] private Vector3 rotate;

    private void Start()
    {
        if (rotationIsLocal)
        {
            rotate = transform.localEulerAngles;
        }
        else
        {
            rotate = transform.eulerAngles;
        }
    }

    public void ResetRotation()
    {
        rotate.y = 0;
        transform.localEulerAngles = rotate;
    }

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
