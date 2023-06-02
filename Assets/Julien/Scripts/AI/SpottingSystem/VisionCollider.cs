using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCollider : MonoBehaviour
{
    [SerializeField]private DetectionSystem detectionSystem;
    [SerializeField]private Transform head;
    [HideInInspector]public float SusMeter = 0;
    private bool inVisionCollider = false;
    [HideInInspector]public bool inVision = false;
    private float susTimer = 0.01f;
    private float startSusTimer;
    RaycastHit objHit;
    private Transform target;
    private Transform targetHead;
    [SerializeField]private LayerMask ignoreLayer;
    public bool activated = true;

    void Start()
    {
        startSusTimer = susTimer;
    }

    void Update()
    {
        //Debug.Log(this.name+": " + inVision);
        if(inVisionCollider && activated)
        {
            if(Physics.Raycast(head.position,(targetHead.position - head.position),out objHit, 100, ~ignoreLayer))
            {
                //Debug.Log(objHit.transform.name);
                var selection = objHit.collider.transform.gameObject;
                if(selection.tag.Equals("Player"))
                {   
                    inVision = true;
                    if(susTimer <= 0 && SusMeter < 1000)
                    {
                        SusMeter++;
                        detectionSystem.SetSusMeter(1f);
                        susTimer = startSusTimer;
                    }
                    susTimer -= Time.deltaTime;
                }else
                {
                    inVision = false;
                    if(susTimer <= 0 && SusMeter > 0)
                    {
                        SusMeter--;
                        detectionSystem.SetSusMeter(-1f);
                        susTimer = startSusTimer;
                    }
                    susTimer -= Time.deltaTime;
                }
            }else
            {
                inVision = false;
                if(susTimer <= 0 && SusMeter > 0)
                {
                    SusMeter--;
                    detectionSystem.SetSusMeter(-1f);
                    susTimer = startSusTimer;
                }
                susTimer -= Time.deltaTime;
            }
            
        }else
        {
            inVision = false;
            if(susTimer <= 0 && SusMeter > 0)
            {
                SusMeter--;
                detectionSystem.SetSusMeter(-1f);
                susTimer = startSusTimer;
            }
            susTimer -= Time.deltaTime;
        }


    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            inVisionCollider = true;
            target = other.gameObject.transform;
            targetHead = other.gameObject.transform.parent.Find("CameraPos");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            inVisionCollider = false;
        }
    }
}
