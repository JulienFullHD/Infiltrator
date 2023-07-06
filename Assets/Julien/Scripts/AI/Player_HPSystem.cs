using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_HPSystem : MonoBehaviour
{
    public int HitPoints = 10;
    [SerializeField]private GhostRunner ghostRunner;
  
    
    public void TakeDamage(int _damage)
    {
        HitPoints -= _damage;
        if(HitPoints <= 0)
        {
            ghostRunner.StopRun();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
