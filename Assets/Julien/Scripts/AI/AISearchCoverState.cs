//author Julien Kelch
using UnityEngine;

public class AISearchCoverState : AIBaseState
{
    private float timer;
    private float startTimer = 20;
    public override void EnterState(AIStateManager ai)
    {
        timer = startTimer;
        ai.text.SetText("SearchCover and wait for "+ timer +"Sec");
        ai.destinationSetter.target = ai.moveTarget;
        //ai.destinationSetter.target.position = ai.coverHandler.GetUsableCoverPos();
    }

    public override void UpdateState(AIStateManager ai)
    {
        ai.text.SetText("SearchCover and wait for "+ timer +"Sec");
        if(timer <= 0)
        {
            ai.SwitchState(ai.PatrollState);
        }else
        {
            timer -= Time.deltaTime;
        }
        
    }

    public override void OnCollisionEnter(AIStateManager ai)
    {

    }
}
