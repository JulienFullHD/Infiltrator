//author Julien Kelch
using UnityEngine;

public class AIEngageState : AIBaseState
{
    public override void EnterState(AIStateManager ai)
    {
        ai.Text.SetText("Engage Player");
        
        ai.destinationSetter.target = ai.Player;
        ai.Animator.SetBool("Attack", true);
    }

    public override void UpdateState(AIStateManager ai)
    {
        if(ai.richAI.reachedEndOfPath)
        {
            //ai.Animator.SetFloat("Speed", 0);
        }else
        {
            //ai.Animator.SetFloat("Speed", 1);
        }
        if(sightOnPlayer(ai.Player, ai))
        {
            ai.Weapon.Shoot(ai.Player);
        }
    }

    public override void OnCollisionEnter(AIStateManager ai)
    {

    }

    public bool sightOnPlayer(Transform _target, AIStateManager ai)
    {
        RaycastHit hit;
        Vector3 dir = (_target.position - ai.Head.position).normalized;
        Debug.DrawRay(ai.Head.position, dir*100 , Color.red);
        if(Physics.Raycast(ai.Head.position, dir, out hit, 100,~ai.IgnoreLayer))
        {
            if(hit.transform.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }
}
