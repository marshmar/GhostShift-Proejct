using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NVDState : State
{
    public NVDState(PlayerControllerR controller) : base(controller)
    {

    }

    public override void DoAction(float deltaTime)
    {

    }

    public override void Enter()
    { 
        GogglesController gogglesController = controller as GogglesController;
        if (gogglesController != null)
        {
            gogglesController.IsInNVDMode?.Invoke(true);
        }
    }

    public override void Exit()
    {
        GogglesController gogglesController = controller as GogglesController;
        if (gogglesController != null)
        {
            gogglesController.IsInNVDMode?.Invoke(false);
        }
    }

    public override State HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            return new IdleState(controller);
        }
        return null;
    }
}
