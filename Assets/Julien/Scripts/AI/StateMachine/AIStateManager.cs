//author Julien Kelch
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro;

public class AIStateManager : MonoBehaviour
{
    public AIBaseState CurrentState;
    public AIPatrollState PatrollState = new AIPatrollState();
    public AIEngageState EngageState = new AIEngageState();
    public List<Transform> PatrollPoints;
    public DetectionSystem DetectionSystem;
    public AI_Weapon Weapon;
    public Transform Player;
    public Transform Head;
    public LayerMask IgnoreLayer;
    public AIDestinationSetter destinationSetter;
    public RichAI richAI;
   
    public Transform MoveTarget;
    public TMP_Text Text;
    public Animator Animator;
    public Transform Visual;

    void Start()
    {
        CurrentState = PatrollState;
        CurrentState.EnterState(this);
        //destinationSetter = this.GetComponent<AIDestinationSetter>();
        //richAI = this.GetComponent<RichAI>();
    }

    // Update is called once per frame
    void Update()
    {
        CurrentState.UpdateState(this);
    }


    public void SwitchState(AIBaseState state)
    {
        CurrentState = state;
        state.EnterState(this);
    }

}
