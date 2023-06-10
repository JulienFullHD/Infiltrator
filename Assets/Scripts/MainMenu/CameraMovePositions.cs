using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraMovePositions : MonoBehaviour
{
    public static CameraMovePositions Instance;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private AnimationCurve moveCurve;
    [SerializeField] private AnimationCurve rotateCurve;
    [SerializeField] private float moveDefaultDuration;
    [ReadOnly, SerializeField] private float moveDuration;
    [ReadOnly, SerializeField] private float moveTimer;
    [ReadOnly, SerializeField] private Vector3 startPos;
    [ReadOnly, SerializeField] private Vector3 startRot;
    [ReadOnly, SerializeField] private Vector3 endPos;
    [ReadOnly, SerializeField] private Vector3 endRot;
    [ReadOnly, SerializeField] private bool isMoving = false;

    private void Awake()
    {
        if(Instance is not null)
        {
            Destroy(this);
        }
        Instance = this;

        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }

    public void MoveCam(Vector3 endingPos, Vector3 endingRotation, float duration = -1)
    {
        if (duration < 0.1)
        {
            duration = moveDefaultDuration;
        }
        moveDuration = duration;

        endPos = endingPos;
        endRot = endingRotation;
        startPos = transform.position;
        startRot = transform.eulerAngles;
        moveTimer = 0;

        isMoving = true;
    }

    public void MoveCam(Vector3 endingPos, float duration = -1)
    {
        MoveCam(endingPos, transform.eulerAngles, duration);        
    }

    private void Update()
    {
        if (isMoving)
        {
            moveTimer += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, moveCurve.Evaluate(moveTimer / moveDuration));
            transform.eulerAngles = Vector3.Lerp(startRot, endRot, rotateCurve.Evaluate(moveTimer / moveDuration));

            if(moveTimer > moveDuration)
            {
                isMoving = false;
            }
        }
    }
}
