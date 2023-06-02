using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCam : MonoBehaviour
{
    [Header("Camera settings")]
    public float horizontalSpeed;
    public float verticalSpeed;
    public float straightSpeed;
    public float horizontalRotationSpeed;
    public float verticalRotationSpeed;
    public float arrowRotationSpeed;
    public float sprintMultiplier;
    private float sprintSpeed;
    private Vector3 newPos;
    private Vector3 moveVector;

    private void Awake()
    {
        moveVector = transform.eulerAngles;
    }

    private void Update()
    {
        CameraLook();
        CameraMove();
    }

    private void CameraMove()
    {
        if (Input.GetKey(KeyCode.V))
        {
            sprintSpeed = sprintMultiplier;
        }
        else
        {
            sprintSpeed = 1;
        }

        newPos = transform.position;

        if (Input.GetKey(KeyCode.W))
        {
            newPos += transform.forward * straightSpeed * Time.deltaTime * sprintSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            newPos -= transform.forward * straightSpeed * Time.deltaTime * sprintSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            newPos += transform.right * horizontalSpeed * Time.deltaTime * sprintSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            newPos -= transform.right * horizontalSpeed * Time.deltaTime * sprintSpeed;
        }
        if (Input.GetKey(KeyCode.E))
        {
            newPos += transform.up * verticalSpeed * Time.deltaTime * sprintSpeed;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            newPos -= transform.up * verticalSpeed * Time.deltaTime * sprintSpeed;
        }

        transform.position = newPos;
    }

    private void CameraLook()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveVector.x -= Time.deltaTime * arrowRotationSpeed;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            moveVector.x += Time.deltaTime * arrowRotationSpeed;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveVector.y -= Time.deltaTime * arrowRotationSpeed;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveVector.y += Time.deltaTime * arrowRotationSpeed;
        }

        moveVector.y += Input.GetAxis("Mouse X") * horizontalRotationSpeed;
        moveVector.x -= Input.GetAxis("Mouse Y") * verticalRotationSpeed;

        transform.eulerAngles = moveVector;
    }
}
