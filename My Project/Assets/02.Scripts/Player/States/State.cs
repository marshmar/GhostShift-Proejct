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

    public abstract void DoAction();
    public abstract State HandleInput();
    public abstract void Enter();
    public abstract void Exit();

    public virtual void FixedDoAction() { }
    public virtual void Skill() { }
}
