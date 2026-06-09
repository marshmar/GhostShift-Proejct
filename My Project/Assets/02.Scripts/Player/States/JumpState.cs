using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : MoveState
{
    private float jumpPower = 33.0f;

    public JumpState(PlayerControllerR controller) : base(controller)
    { }

    public override void DoAction(float deltaTime)
    {
        Move();
        FlipSprite(1.0f);
    }

    public override void Enter()
    {
        Jump();
    }

    public override void Exit()
    {

    }

    public override State HandleInput()
    {
        h = Input.GetAxisRaw("Horizontal");

        if (rigid.velocity.y < 0)
        {
            return new FallingState(controller);
        }
        return null;
    }


    // 플레이어 점프
    public void Jump()
    {
        // 점프
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        // 점프 효과음 재생
        controller.PlayJumpAudio();
        // 점프 애니메이션 재생
        anim.SetBool("isJumping", true);
    }
}
