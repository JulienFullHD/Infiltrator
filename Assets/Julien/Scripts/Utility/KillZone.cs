using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
    public AK.Wwise.Event myKillzoneDeath;
    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag.Equals("Player"))
        {
            myKillzoneDeath.Post(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}
