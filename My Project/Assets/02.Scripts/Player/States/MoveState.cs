using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    private float maxSpeed = 8f;
    private float h;

    private Rigidbody2D rigid;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    public MoveState(PlayerControllerR controller) : base(controller)
    { }

    public override void Enter()
    {
        rigid = controller.Rigid2D;
        spriteRenderer = controller.SpriteRenderer;
        anim = controller.Anim;
    }

    public override void Exit()
    {
        rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        anim.SetBool("isWalking", false);
    }

    public override void DoAction(float deltaTime)
    {
        Move();
    }

    public override State HandleInput()
    {
        h = Input.GetAxisRaw("Horizontal");
        if(h == 0)
        {
            return new IdleState(controller);
        }

        if (Input.GetButtonDown("Jump") && anim.GetBool("isJumping") == false)
        {
            return new JumpState(controller);
        }
        return null;
    }

    public void Move()
    {
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed) // Right Max Speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) // Left Max Speed
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        // sprite
        spriteRenderer.flipX = (h == 1);

        //Animation
        anim.SetBool("isWalking", Mathf.Abs(rigid.velocity.x) > 0.5);
    }
}
