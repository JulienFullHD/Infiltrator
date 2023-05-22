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
    public AISearchCoverState EngageState = new AISearchCoverState();
    public List<Transform> PatrollPoints;
    [HideInInspector]public AIDestinationSetter destinationSetter;
    [HideInInspector]public RichAI richAI;
    public LayerMask layerMask;
    //public CoverHandler coverHandler;
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
