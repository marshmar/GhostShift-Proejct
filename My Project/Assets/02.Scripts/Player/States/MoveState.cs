using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    private float maxSpeed = 8f;

    public MoveState(PlayerControllerR controller) : base(controller)
    { }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void DoAction()
    {
        float h = Input.GetAxisRaw("Horizontal");
        Rigidbody2D rigid = controller.Rigid2D;
        SpriteRenderer spriteRenderer = controller.SpriteRenderer;
        Animator anim = controller.Anim;

        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed) // Right Max Speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) // Left Max Speed
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);


        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //Direction Sprite
        if (Input.GetButton("Horizontal"))
        {

            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == 1;
            if (Input.GetAxisRaw("Horizontal") == 1)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }

        //Animation
        if (Mathf.Abs(rigid.velocity.x) < 0.5)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
    }

    public override State HandleInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if(h == 0)
        {
            return new IdleState(controller);
        }
        return null;
    }
}
