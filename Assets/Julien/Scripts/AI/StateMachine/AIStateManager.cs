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
    [HideInInspector]public AIDestinationSetter destinationSetter;
    [HideInInspector]public RichAI richAI;
   
    public Transform moveTarget;
    public TMP_Text text;

    void Start()
    {
        CurrentState = PatrollState;
        CurrentState.EnterState(this);
        destinationSetter = this.GetComponent<AIDestinationSetter>();
        richAI = this.GetComponent<RichAI>();
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
