using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] public string menuName;
    [SerializeField] public Transform cameraPos;
    [ReadOnly, SerializeField] public bool isOpen;

    public void Open()
    {
        isOpen = true;
        gameObject.SetActive(true);
        CameraMovePositions.Instance.MoveCam(endingPos: cameraPos.position, endingRotation: cameraPos.eulerAngles);
    }
    public void Close()
    {
        isOpen = false;
        gameObject.SetActive(false);
    }
}
