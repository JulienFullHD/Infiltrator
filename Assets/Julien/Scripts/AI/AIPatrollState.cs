//author Julien Kelch
using UnityEngine;

public class AIPatrollState : AIBaseState
{
    private int currIndex = 0;
    public override void EnterState(AIStateManager ai)
    {
        ai.text.SetText("Partoll to Waypoint " + currIndex);
        ai.destinationSetter.target = ai.PatrollPoints[currIndex];
    }

    public override void UpdateState(AIStateManager ai)
    {
        if(ai.richAI.reachedDestination)
        {
            ai.destinationSetter.target = GetNextCheckpoint(ai);
            ai.text.SetText("Partoll to Waypoint " + currIndex);
        }

        Debug.DrawRay(ai.transform.position, ai.transform.TransformDirection(Vector3.forward) * 100, Color.yellow);
        if (Physics.Raycast(ai.transform.position, ai.transform.TransformDirection(Vector3.forward), 100, ai.layerMask))
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
