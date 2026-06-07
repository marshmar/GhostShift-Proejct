using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : State
{
    private float elapsedTime;
    private float dashSpeed = 30.0f;
    private float dashTime = 0.2f;
    private float originGravity;

    private Rigidbody2D rigid;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    public DashState(PlayerControllerR controller) : base(controller)
    {
        rigid = controller.Rigid2D;
        anim = controller.Anim;
        spriteRenderer = controller.SpriteRenderer;
    }
    public override void DoAction(float deltaTime)
    {
        elapsedTime += deltaTime;
        if(elapsedTime < dashTime)
            Dash();
    }

    public override void Enter()
    {
        // 대쉬 애니메이션 설정
        anim.SetBool("isDashing", true);

        elapsedTime = 0.0f;
        originGravity = rigid.gravityScale;

        GhostController ghostController = controller as GhostController;
        if (ghostController != null)
        {
            ghostController.CanDash = false;
        }
    }

    public override void Exit()
    {
        // 대쉬 애니메이션 설정
        anim.SetBool("isDashing", false);

        elapsedTime = 0.0f;
        rigid.gravityScale = originGravity;
    }

    public override State HandleInput()
    {
        if(elapsedTime >= dashTime)
        {
            return new MoveState(controller);
        }
        return null;
    }

    private void Dash()
    {
        // 대쉬 효과음 재생
        //audio.PlayOneShot(dashSfx);

        // 중력값을 0으로 변경
        rigid.gravityScale = 0f;


        Vector2 playerToMouseVector = controller.GetPlayerToMouseUnitVector();


        //대쉬할 때 마우스 위치에 따라 회전
        if (playerToMouseVector.x > 0)
        {
            if (spriteRenderer.flipX == false)
                spriteRenderer.flipX = true;
        }
        else
        {
            if (spriteRenderer.flipX == true)
                spriteRenderer.flipX = false;
        }

        // 지정 방향으로 대쉬(가속력)
        rigid.velocity = playerToMouseVector * dashSpeed;
    }
}
