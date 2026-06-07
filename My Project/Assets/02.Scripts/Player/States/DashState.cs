using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : State
{
    public DashState(PlayerControllerR controller) : base(controller)
    { }
    public override void DoAction()
    {
        Debug.Log("Dash State!");
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override State HandleInput()
    {
        return null;
    }
}
