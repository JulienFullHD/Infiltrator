//author Julien Kelch
using UnityEngine;

public class AIPatrollState : AIBaseState
{
    private int currIndex = 0;
    public override void EnterState(AIStateManager ai)
    {
        ai.text.SetText("Partoll to Waypoint " + currIndex);
        if(ai.PatrollPoints.Count > 0)
        {
            ai.destinationSetter.target = ai.PatrollPoints[currIndex];
        }
    }

    public override void UpdateState(AIStateManager ai)
    {
        if(ai.PatrollPoints.Count > 0)
        {
            if(ai.richAI.reachedDestination)
            {
                ai.destinationSetter.target = GetNextCheckpoint(ai);
                ai.text.SetText("Partoll to Waypoint " + currIndex);
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
