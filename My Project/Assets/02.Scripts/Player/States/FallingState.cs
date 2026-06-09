using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : MoveState
{
    private bool isGround;

    public FallingState(PlayerControllerR controller) : base(controller)
    { }

    public override void DoAction(float deltaTime)
    {
        Move();
        FlipSprite(1.0f);
    }

    public override void Enter()
    {
        anim.SetBool("isJumping", true);
        isGround = false;
    }

    public override void Exit()
    {
        isGround = false;
        anim.SetBool("isJumping", false);
    }

    public override State HandleInput()
    {
        h = Input.GetAxisRaw("Horizontal");

        if(CheckGround())
        {
            return new IdleState(controller);
        }
        return null;
    }

    public bool CheckGround()
    {
        const float rayDistance = 1.5f;
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, rayDistance, LayerMask.GetMask("Platform"));
        if (rayHit.collider != null)
        {
            if (rayHit.distance < rayDistance)
            {
                GhostController ghostController = controller as GhostController;
                if (ghostController != null)
                {
                    ghostController.CanDash = true;
                }
                return true;
            }
        }
        return false;
    }
}
