using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected PlayerControllerR controller;

    public State(PlayerControllerR controller)
    {
        this.controller = controller;
    }

    public abstract void DoAction(float deltaTime);
    public abstract State HandleInput();

    public abstract void Enter();
    public abstract void Exit();

    public virtual void FixedDoAction() { }
    public virtual void Skill() { }

    public virtual State HandleSpecialInput()
    {
        return controller.HandleSpecialStateInput();
    }
}
