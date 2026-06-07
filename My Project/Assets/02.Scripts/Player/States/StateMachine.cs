using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public State CurrentState { get; private set; } 

    public void Initialize(State initState)
    {
        CurrentState = initState;
        CurrentState.Enter();
    }

    public void ChangeState(State nextState)
    {
        if (nextState == null || CurrentState == nextState) return;

        CurrentState.Exit();
        CurrentState = nextState;
        CurrentState.Enter();
    }

    public void UpdateState()
    {
        if(CurrentState == null) return;    

        State nextState = CurrentState.HandleInput();
        if (nextState != null)
        {
            ChangeState(nextState);
        }

        CurrentState.DoAction();
    }

    public void FixedUpdateState()
    {
        if (CurrentState == null) return;

        CurrentState.FixedDoAction();
    }
}
