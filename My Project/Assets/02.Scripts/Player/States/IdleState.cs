using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public IdleState(PlayerControllerR controller) : base(controller)
    { }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override State HandleInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if( h != 0f)
        {
            return new MoveState(controller);
        }

        if (Input.GetButtonDown("Jump"))
        {
            return new JumpState(controller);
        }

        return null;
    }

    public override void DoAction(float deltaTime)
    {

    }
}
