using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] public string menuName;
    [SerializeField] public Transform cameraPos;
    [ReadOnly, SerializeField] public bool isOpen;

    /// <summary>
    /// Enables the gameObjet this menu is on and tells the camera to move to its new location
    /// </summary>
    public void Open()
    {
        isOpen = true;
        gameObject.SetActive(true);
        CameraMovePositions.Instance.MoveCam(endingPos: cameraPos.position, endingRotation: cameraPos.eulerAngles);
    }

    /// <summary>
    /// Disables the gameObject this menu is on
    /// </summary>
    public void Close()
    {
        isOpen = false;
        gameObject.SetActive(false);
    }
}
