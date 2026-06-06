using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public IdleState(Transform player, Animator anim) : base(player, anim)
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
            return new MoveState(player, anim);
        }
        return null;
    }

    public override void DoAction()
    {
        Debug.Log("Idle state");
    }
}
