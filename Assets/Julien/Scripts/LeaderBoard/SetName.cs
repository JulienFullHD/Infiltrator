using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetName : MonoBehaviour
{
    [SerializeField]private TMP_InputField inputField;

    public void SetPlayerName()
    {
        if(inputField.text != null)
        {
            PlayerPrefs.SetString("PlayerName", inputField.text);
            PlayerPrefs.Save();
        }
    }
}
