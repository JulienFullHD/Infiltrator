//author Julien Kelch
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPatrollState : AIBaseState
{
    private int currIndex = 0;
    private float startWaitTime = 5;
    private float waitTime = 0;
    private bool wait;
    public override void EnterState(AIStateManager ai)
    {
        ai.Text.SetText("Partoll to Waypoint " + currIndex);
        if(ai.PatrollPoints.Count > 0)
        {
            ai.destinationSetter.target = ai.PatrollPoints[currIndex];
            ai.Animator.SetBool("Attack", true);
            //ai.Animator.SetFloat("Speed", 1);
        }else
        {
            ai.destinationSetter.target = ai.transform;
            //ai.Animator.SetFloat("Speed", 0);
        }
    }

    public override void UpdateState(AIStateManager ai)
    {
        if(ai.PatrollPoints.Count > 0)
        {
            if(ai.richAI.reachedDestination)
            {
                if(!wait)
                {
                    waitTime = startWaitTime;
                    wait = true;
                    //ai.Animator.SetFloat("Speed", 0);
                    ai.Animator.SetBool("Attack", false);
                    ai.Visual.localPosition = Vector3.zero;
                }else
                {
                    waitTime -= Time.deltaTime;
                    if(waitTime <= 0)
                    {
                        ai.destinationSetter.target = GetNextCheckpoint(ai);
                        ai.Text.SetText("Partoll to Waypoint " + currIndex);
                        //ai.Animator.SetFloat("Speed", 1);
                        ai.Animator.SetBool("Attack", true);
                        wait = false;
                    }
                }
            }
        }
        

        if(ai.DetectionSystem.GetSusMeter() >= 100)
        {
            ai.SwitchState(ai.EngageState);
        }
    }

    public override void OnCollisionEnter(AIStateManager ai)
    {
        
    }
    private Transform GetNextCheckpoint(AIStateManager ai)
    {
        currIndex++;

        currIndex = currIndex % ai.PatrollPoints.Count;

        return ai.PatrollPoints[currIndex];
    }
}
