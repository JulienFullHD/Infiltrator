using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DetectionSystem : MonoBehaviour
{   
    [SerializeField]private float SusMeter = 0f;
    private float SusMeterHearing = 0f;
    public bool PlayerInVision = false;
    private int sightCounter = 0;
    [SerializeField]private List<VisionCollider> VisionCollider = new List<VisionCollider>();

    private void Update() 
    {
        if(SusMeter > 0)
        {   
            sightCounter = 0;
            foreach (var item in VisionCollider)
            {
                if(item.inVision == true)
                {
                    sightCounter++;
                }
            }
            if(sightCounter > 0)
            {
                PlayerInVision = true;
            }else
            {
                PlayerInVision = false;
            } 
        }else
        {
            PlayerInVision = false;
        }
        
    }




    public void SetSusMeter(float _susRatio)
    {
        float alpha;
        if(SusMeter <= 100)
        {
            alpha = SusMeter;
        }else
        {
            alpha = 100;
        }
        //SI_System.CreateIndicator(this.transform,alpha);
        SusMeter += _susRatio;
        
    }

    public float GetSusMeter()
    {
        return SusMeter;
    }
}
