using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : PlayerControllerR
{
    public bool CanDash { get; set; }

    private bool isCalledStickEvent;
    private Enemy target;

    // 대쉬 효과음
    private AudioClip dashSfx;
    // 공격 효과음들
    private AudioClip attack1Sfx;
    private AudioClip attack2Sfx;

    protected override void Awake()
    {
        base.Awake();

        CanDash = true;
        isCalledStickEvent = false;
    }

    protected override void Start()
    {
        base.Start();

        // 대쉬 효과음 로드
        dashSfx = Resources.Load<AudioClip>("PlayerAudios/dash");
        // 공격1 효과음 로드
        attack1Sfx = Resources.Load<AudioClip>("PlayerAudios/attack1");
        // 공격2 효과음 로드
        attack2Sfx = Resources.Load<AudioClip>("PlayerAudios/attack2");
    }

    public override State HandleSpecialInput()
    {
        if(Input.GetMouseButtonDown(0) && CanDash)
        {
            Vector2 dirUnit = GetPlayerToMouseUnitVector();
            return new DashState(this);
        }

        if(isCalledStickEvent)
        {
            isCalledStickEvent = false;
            return new StickState(this, target);
        }

        return base.HandleSpecialInput();
    }

    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (enabled == false)
        {
            return;
        }

        if (collider.CompareTag("Enemy"))
        {
            if(playerManager.GetCurrentState() is DashState)
            {
                Debug.Log("Change to stick state");
                Enemy enemy = collider.GetComponent<Enemy>();   
                if(enemy != null)
                {
                    target = enemy;
                    target.Died();
                    isCalledStickEvent = true;
                    return;
                }
            }

            if ((playerManager.GetCurrentState() is StickState) == false)
            {
                DamagePlayerAndKnockBack(collider);
            }
        }

        // 총알과 충돌했을 경우
        if (collider.CompareTag("Bullet"))
        {
            if (playerManager.GetCurrentState() is StickState)
            {
                return;
            }

            // 총알의 방향을 읽어오기 위해 스크립트 컴포넌트 얻어오기
            if (collider.TryGetComponent<BulletController>(out BulletController bulletControllerScr))
            {
                Debug.Log($"{0}: 총알과 충돌하여 체력 달기", this);
                Vector2 knockBackVec = new Vector2(Mathf.Sign(collider.gameObject.GetComponent<Rigidbody2D>().velocity.x), 1.0f);
                DamagePlayerAndKnockBack(knockBackVec);
                Destroy(collider.gameObject);
            }
        }
    }


    public void ChangeCharacter()
    {
        if(CanPosses(target.enemyType) == false)
        {
            return;
        }

        playerManager.ChangeCharType(GetChangePlayerType(target.enemyType));
        target = null;
    }

    private bool CanPosses(EnemyType enemyType)
    {
        if (enemyType == EnemyType.NONE || enemyType == EnemyType.GUNNER)
            return false;
        return true;
    }

    private PlayerType GetChangePlayerType(EnemyType enemyType)
    {
        PlayerType playerType = PlayerType.PLAYERGHOST;
        switch (enemyType)
        {
            case EnemyType.SHIELD:
                playerType = PlayerType.PLAYERSHIELD;
                break;
            case EnemyType.GOGGLES:
                playerType = PlayerType.PLAYERGOGGLES;
                break;
            case EnemyType.CLEANER:
                playerType = PlayerType.PLAYERCLEANER;
                break;
        }
        return playerType;
    }

    public void PlayDashSfx()
    {
        AudioSource.PlayOneShot(dashSfx);
    }

    public void PlayAttackSfx()
    {
        AudioSource.PlayOneShot(Random.Range(0, 2) == 1 ? attack1Sfx : attack2Sfx);
    }
}
