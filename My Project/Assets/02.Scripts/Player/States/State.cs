using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected Transform player;
    protected Animator anim;

    public State(Transform player, Animator anim)
    {
        this.player = player;
        this.anim = anim;
    }

    public abstract void DoAction();
    public abstract State HandleInput();
    public abstract void Enter();
    public abstract void Exit();
    public virtual void Skill() { }
}
