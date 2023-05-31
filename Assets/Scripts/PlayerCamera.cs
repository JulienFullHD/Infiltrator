using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float sensitivityX;
    [SerializeField] private float sensitivityY;
    [SerializeField] private Transform flatOrientation;
    [SerializeField] private float xRotation;
    [SerializeField] private float yRotation;
    [SerializeField] private float mouseX;
    [SerializeField] private float mouseY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        mouseX = Input.GetAxisRaw("Mouse X") * sensitivityX;
        mouseY = Input.GetAxisRaw("Mouse Y") * sensitivityY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -89.99f, 89.99f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        flatOrientation.rotation = Quaternion.Euler(0, yRotation, 0);

        
        if(mouseX < -12f || mouseX > 12f)
        {
            Debug.LogWarning($"{yRotation} || {mouseX} || {Time.deltaTime}");
        }
        else
        {
            Debug.Log($"{yRotation} || {Input.GetAxisRaw("Mouse X")} || {Time.deltaTime}");
        }
    }
}
