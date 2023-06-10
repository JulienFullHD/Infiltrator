using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] public string menuName;
    [SerializeField] public Transform cameraPos;
    [SerializeField] private Turntable turntable;
    [ReadOnly, SerializeField] public bool isOpen;

    public void Open()
    {
        turntable.ResetRotation();
        isOpen = true;
        gameObject.SetActive(true);
        CameraMovePositions.Instance.MoveCam(cameraPos.position);
    }
    public void Close()
    {
        isOpen = false;
        gameObject.SetActive(false);
    }
}
