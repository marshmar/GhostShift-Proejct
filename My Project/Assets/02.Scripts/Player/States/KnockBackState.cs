using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackState : State
{
    private float elapsedTime;
    private float knockbackDuration;
    private Vector2 knockbackDir;

    public KnockBackState(PlayerControllerR controller, Vector2 dir) : base(controller)
    {
        knockbackDir = dir;
        knockbackDuration = 0.3f;
    }

    public override void DoAction(float deltaTime)
    {
        elapsedTime += deltaTime;
    }

    public override void Enter()
    {
        Debug.Log("Enter KnockBackState");

        // 피격 효과음 재생
        controller.PlayKnockBackAudio();
        const float knockBackPower = 21.0f;
        rigid.AddForce(knockBackPower * knockbackDir, ForceMode2D.Impulse);
    }

    public override void Exit()
    {
        elapsedTime = 0.0f;
        knockbackDir = Vector2.zero;
    }

    public override State HandleInput()
    {
        if(elapsedTime >= knockbackDuration)
        {
            return new FallingState(controller);
        }
        return null;
    }
}
