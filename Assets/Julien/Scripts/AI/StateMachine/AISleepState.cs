//author Julien Kelch
using UnityEngine;

public class AISleepState : AIBaseState
{
    public override void EnterState(AIStateManager ai)
    {
        ai.Text.SetText("Engage Player");
        
        ai.destinationSetter.target = ai.Player;
        ai.Animator.SetFloat("Speed", 0);
        ai.Animator.SetBool("Aiming", false);

        ai.destinationSetter.target = ai.transform;
    }

    public override void UpdateState(AIStateManager ai)
    {
        foreach (var mate in ai.ConnectedMates)
        {
            if(mate.DetectionSystem.GetSusMeter() >= 100)
            {
                ai.SwitchState(ai.SnipeState);
            }
        }
    }

    public override void OnCollisionEnter(AIStateManager ai)
    {

    }
}
