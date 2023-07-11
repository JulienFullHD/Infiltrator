using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
    [SerializeField]private GhostRunner ghostRunner;
    public AK.Wwise.Event myKillzoneDeath;
    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag.Equals("Player"))
        {
            ghostRunner.StopRun();
            myKillzoneDeath.Post(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}
