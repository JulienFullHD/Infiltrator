using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSounds : MonoBehaviour
{
    public AK.Wwise.Event ButtonForward;
    public AK.Wwise.Event ButtonBack;

    public void PlayForwardSound()
    {
        Debug.Log("Kekkek");
        ButtonForward.Post(gameObject);
    }

    public void PlayBackSound()
    {
        Debug.Log("Kekkek2");
        ButtonBack.Post(gameObject);
    }

}