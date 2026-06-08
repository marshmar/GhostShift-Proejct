using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    private float maxSpeed = 8f;
    protected float h;

    private SpriteRenderer spriteRenderer;

    public MoveState(PlayerControllerR controller) : base(controller)
    {
        spriteRenderer = controller.SpriteRenderer;
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {
        rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        anim.SetBool("isWalking", false);
    }

    public override void DoAction(float deltaTime)
    {
        Move();
        FlipSprite(1.0f);
        SetMoveAnim();
    }

    public override State HandleInput()
    {
        h = Input.GetAxisRaw("Horizontal");
        if(h == 0)
        {
            return new IdleState(controller);
        }

        if (Input.GetButtonDown("Jump"))
        {
            return new JumpState(controller);
        }

        if (rigid.velocity.y < -2.0f)
        {
            return new FallingState(controller);
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
    }

    public void SetMoveAnim()
    {
        //Animation
        anim.SetBool("isWalking", Mathf.Abs(rigid.velocity.x) > 0.5);
    }

    public void FlipSprite(float value)
    {
        // sprite
        spriteRenderer.flipX = (h == value);
    }
}
