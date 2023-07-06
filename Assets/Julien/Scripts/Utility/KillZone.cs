using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
    [SerializeField]private GhostRunner ghostRunner;
    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag.Equals("Player"))
        {
            ghostRunner.StopRun();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}
