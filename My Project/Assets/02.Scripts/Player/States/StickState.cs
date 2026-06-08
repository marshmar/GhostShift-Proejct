using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickState : State
{
    private float originGravity;
    private Enemy target;
    public StickState(PlayerControllerR controller, Enemy enemy) : base(controller)
    { 
        if(enemy == null)
        {
            Debug.Log("Enemy is nullptr");
        }
        target = enemy; 
    }

    public override void DoAction(float deltaTime)
    {

    }

    public override void Enter()
    {
        originGravity = rigid.gravityScale;
        rigid.gravityScale = 0.0f;
        controller.Transform.position = target.transform.position;
        rigid.velocity = Vector3.zero;

        anim.SetBool("isSticking", true);
    }

    public override void Exit()
    {
        rigid.gravityScale = originGravity;
        originGravity = 0.0f;
        target = null;
        anim.SetBool("isSticking", false);
    }

    public override State HandleInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            target.KnockBack(-1 * controller.GetPlayerToMouseUnitVector());
            target.DestroyGameObject(1.5f);
            
            // 공격 사운드 2개중 하나 랜덤 출력
            //audio.PlayOneShot(Random.Range(0, 2) == 1 ? attack1Sfx : attack2Sfx);

            // 이펙트 생성
            //GenerateEffects();
            return new DashState(controller);
        }
        if(Input.GetMouseButtonDown(1))
        {
            GhostController ghostController = controller as GhostController;
            if (controller != null)
            {
                ghostController.ChangeCharacter();
                target.DestroyGameObject(0.0f);

                var nextController = controller.GetCurrentController();
                return new FallingState(nextController);

            }
        }
        return null;
    }
}
