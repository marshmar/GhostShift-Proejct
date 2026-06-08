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
        //Direction Sprite
        if (Input.GetButton("Horizontal"))
        {
            ShieldController shieldController = controller as ShieldController;
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == 1;
            if (Input.GetAxisRaw("Horizontal") == 1)
            {
                spriteRenderer.flipX = true;
                if (shieldController != null)
                {
                    shieldController.shield.transform.localPosition = shieldController.shieldPosition;
                }
            }
            else
            {
                spriteRenderer.flipX = false;
                if (shieldController != null)
                {
                    shieldController.shield.transform.localPosition = shieldController.shieldPosition * -1;
                }
            }
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
