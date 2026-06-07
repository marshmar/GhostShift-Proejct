using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : State
{
    private float jumpPower = 33.0f;

    private Rigidbody2D rigid;
    private Animator anim;
    public JumpState(PlayerControllerR controller) : base(controller)
    {
        rigid = controller.Rigid2D;
        anim = controller.Anim;
    }

    public override void DoAction(float deltaTime)
    {
        CheckGround();
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
        float h = Input.GetAxisRaw("Horizontal");
        if (h != 0f)
        {
            return new MoveState(controller);
        }

        if (anim.GetBool("isJumping") == false)
        {
            return new IdleState(controller);
        }
        return null;
    }


    // 플레이어 점프
    public void Jump()
    {
        // 점프
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        //// 점프 효과음 재생
        //audio.PlayOneShot(jumpSfx);
        // 점프 애니메이션 재생
        anim.SetBool("isJumping", true);
    }

    public void CheckGround()
    {
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 1.0f)
                {
                    anim.SetBool("isJumping", false);
                }
            }
        }

    }
}
