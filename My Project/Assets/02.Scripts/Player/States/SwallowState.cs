using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwallowState : State
{
    private SpriteRenderer spriteRenderer;
    private Vector3 swallowRange;
    private float swallowSpeed; 
    public SwallowState(PlayerControllerR controller) : base(controller)
    {
        spriteRenderer = controller.SpriteRenderer;
        swallowRange = new Vector3(8.0f, 1.0f, 0.0f);
        swallowSpeed = 8.0f;
    }

    public override void DoAction(float deltaTime)
    {
        CleanerController cleanerController = controller as CleanerController;
        if(cleanerController != null)
        {
            // 플레이어 이동 방향에 따른 삼키는 방향 설정
            if ((spriteRenderer.flipX == false && swallowRange.x > 0) || (spriteRenderer.flipX == true && swallowRange.x < 0))
            {
                swallowRange.x *= -1;
            }

            Transform tr = controller.Transform;
            //Physics2D.OverlapAreaAll : 가상의 직사각형을 만들어 추출하려는 반경 이내에 들어와 있는 콜라이더들을 배열 형태로반환하는 함수
            Collider2D[] colliderArray = Physics2D.OverlapAreaAll(tr.position, tr.position + swallowRange);

            // 콜라이더 배열을 순환하면서
            for (int i = 0; i < colliderArray.Length; i++)
            {
                // null이면 continue;
                if (colliderArray[i] == null) continue;
                // 주위에 에너미가 있으면
                if (colliderArray[i].tag == "Enemy")
                {
                    // 적 객체를 Die상태로 변경
                    if (colliderArray[i].TryGetComponent<Enemy>(out Enemy enemy))
                    {
                        enemy.Died();
                    }
                    // 더 이상 에너미가 없으면 반복문 종료
                    if (colliderArray[i] == null)
                    {
                        break;
                    }

                    // 적과 플레이어의 방향 벡터를 구하고
                    Vector3 dir = (colliderArray[i].transform.position - tr.position).normalized;
                    // 적의 포지션에 방향 벡터를 더하여 적을 플레이어의 위치로 끌어당김
                    dir = new Vector3(dir.x * 2.0f, dir.y * 2.0f, dir.z * 2.0f);
                    // 끌어당기는 속도 설정
                    dir *= swallowSpeed * Time.deltaTime;
                    // 끌어당기기
                    colliderArray[i].transform.position -= dir;
                }
            }
        }
        
    }


    public override void Enter()
    {
        anim.SetBool("isSwallowing", true);
        CleanerController cleanerController = controller as CleanerController;
        if (cleanerController != null)
        {
            cleanerController.IsSwallowing = true;
        }
    }

    public override void Exit()
    {
        anim.SetBool("isSwallowing", false);

        CleanerController cleanerController = controller as CleanerController;
        if (cleanerController != null)
        {
            if (cleanerController.IsSwallowed)
            {
                anim.SetBool("isSwallowed", true);
            }
        }
    }

    public override State HandleInput()
    {
        if(Input.GetMouseButtonUp(1))
        {
            return new IdleState(controller);
        }

        CleanerController cleanerController = controller as CleanerController;
        if (cleanerController != null)
        {
            if (cleanerController.IsSwallowed)
            {
                return new IdleState(controller);
            }
        }
        return null;
    }
}
