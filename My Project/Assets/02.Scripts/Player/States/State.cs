using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected PlayerControllerR controller;
    protected Rigidbody2D rigid;
    protected Animator anim;

    public State(PlayerControllerR controller)
    {
        this.controller = controller;
        rigid = this.controller.Rigid2D;
        anim = this.controller.Anim;
    }

    public abstract void DoAction(float deltaTime);
    public abstract State HandleInput();

    public abstract void Enter();
    public abstract void Exit();

    public virtual void FixedDoAction() { }

    public virtual State HandleSpecialInput()
    {
        return controller.HandleSpecialInput();
    }
}
