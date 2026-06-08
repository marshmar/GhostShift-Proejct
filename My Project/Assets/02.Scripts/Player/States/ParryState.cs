using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryState : State
{
    private float elapsedTime;
    private float parryDuration;
    private SpriteRenderer spriteRenderer;
    public ParryState(PlayerControllerR controller) : base(controller)
    {
        parryDuration = 0.25f;
        spriteRenderer = controller.SpriteRenderer;
    }

    public override void DoAction(float deltaTime)
    {
        elapsedTime += deltaTime;
    }

    public override void Enter()
    {
        ShieldController shieldController = controller as ShieldController;
        if (shieldController != null)
        {
            shieldController.SetShieldObject(true);
            shieldController.SetParryState(true);
            shieldController.PlaySwingSfx();

            if(spriteRenderer.flipX)
            {
                shieldController.shield.transform.localPosition = shieldController.shieldPosition;
            }
            else
            {
                shieldController.shield.transform.localPosition = shieldController.shieldPosition * -1;
            }
        }

        anim.SetBool("isParrying", true);
    }

    public override void Exit()
    {
        elapsedTime = 0.0f;
        ShieldController shieldController = controller as ShieldController;
        if (shieldController != null)
        {
            shieldController.SetShieldObject(false);
            shieldController.SetParryState(false);
        }
        anim.SetBool("isParrying", false);
    }

    public override State HandleInput()
    {
        if(elapsedTime >= parryDuration)
        {
            return new IdleState(controller);
        }
        return null;
    }
}
