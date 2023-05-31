using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_HPSystem : MonoBehaviour
{
    public int HitPoints = 10;
  
    
    public void TakeDamage(int _damage)
    {
        HitPoints -= _damage;
        if(HitPoints <= 0)
        {
            //Die
        }
    }
}
