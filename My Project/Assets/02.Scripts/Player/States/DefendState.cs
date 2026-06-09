using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendState : State
{
    private SpriteRenderer spriteRenderer;
    public DefendState(PlayerControllerR controller) : base(controller)
    {
        spriteRenderer = controller.SpriteRenderer;
    }

    public override void DoAction(float deltaTime)
    {
        float h = Input.GetAxisRaw("Horizontal");
        if (h == 1)
        {
            spriteRenderer.flipX = true;
        }
        else if (h == -1)
        {
            spriteRenderer.flipX = false;
        }

        ShieldController shieldController = controller as ShieldController;
        if(shieldController != null)
        {
            shieldController.shield.transform.localPosition
                = spriteRenderer.flipX ? shieldController.shieldPosition : shieldController.shieldPosition * -1.0f;
        }
    }

    public override void Enter()
    {
        ShieldController shieldController = controller as ShieldController;
        if (shieldController != null)
        {
            shieldController.SetShieldObject(true);
        }
        anim.SetBool("isDefending", true);
    }

    public override void Exit()
    {
        ShieldController shieldController = controller as ShieldController;
        if (shieldController != null)
        {
            shieldController.SetShieldObject(false);
        }
        anim.SetBool("isDefending", false);
    }

    public override State HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            return new IdleState(controller);
        }
        return null;
    }
}
