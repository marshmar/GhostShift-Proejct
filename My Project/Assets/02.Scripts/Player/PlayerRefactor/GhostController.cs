using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : PlayerControllerR
{
    public bool CanDash { get; set; }

    protected override void Awake()
    {
        base.Awake();

        CanDash = true;
    }
    public override State HandleSpecialStateInput()
    {
        if(Input.GetMouseButtonDown(0) && CanDash)
        {
            Vector2 dirUnit = GetPlayerToMouseUnitVector();
            return new DashState(this);
        }

        return null;
    }
}
