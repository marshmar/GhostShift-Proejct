using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanerController : PlayerControllerR
{
    public bool IsSwallowing;
    public bool IsSwallowed;

    private Queue<EnemyType> swalloedEnemy = new Queue<EnemyType>();
    private Sprite[] enemyDiedBulletSprites = new Sprite[5];
    private GameObject enemyDiedBulletPrefab;
    private AudioClip attack1Sfx;

    protected override void Start()
    {
        // 플레이어 총알 받아오기
        enemyDiedBulletPrefab = Resources.Load<GameObject>("BulletPrefabs/PlayerBullet");
        // 유령 타입 적 사망 스프라이트 로드
        enemyDiedBulletSprites[0] = Resources.Load<Sprite>("EnemyImages/Enemy_Ghost/Enemy_ghost_dead");
        // 방패병 타입 적 사망 스프라이트 로드
        enemyDiedBulletSprites[1] = Resources.Load<Sprite>("EnemyImages/Enemy_Shield/Enemy_shield_dead");
        // 고글 타입 적 사망 스프라이트 로드
        enemyDiedBulletSprites[2] = Resources.Load<Sprite>("EnemyImages/Enemy_Goggles/Enemy_goggles_dead");
        // 거너 타입 적 사망 스프라이트 로드
        enemyDiedBulletSprites[3] = Resources.Load<Sprite>("EnemyImages/Enemy_Gunner/Enemy_gun_dead");
        // 청소부 타입 적 사망 스프라이트 로드
        enemyDiedBulletSprites[4] = Resources.Load<Sprite>("EnemyImages/Enemy_Gunner/Enemy_gun_dead");
        attack1Sfx = Resources.Load<AudioClip>("PlayerAudios/attack1");
    }

    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (enabled == false)
        {
            return;
        }

        if (collider.CompareTag("Enemy"))
        {   
            if(IsSwallowing)
            {
                IsSwallowing = false;
                IsSwallowed = true;
                Destroy(collider.gameObject);
                if (collider.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    swalloedEnemy.Enqueue(enemy.enemyType);
                }
                return;
            }

            DamagePlayerAndKnockBack(collider);
        }

        // 총알과 충돌했을 경우
        if (collider.CompareTag("Bullet"))
        {
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

    public override State HandleSpecialInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GenerateShakeEffect();
            playerManager.ChangeCharType(PlayerType.PLAYERGHOST);
            return new DashState(GetCurrentController());
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (IsSwallowed)
            {
                Fire(swalloedEnemy.Dequeue());
                IsSwallowed = false;
            }
            else
            {
                return new SwallowState(this);
            }

        }

        return base.HandleSpecialInput();
    }

    private void Fire(EnemyType enemyType)
    {
        Anim.Play("player_cleaner_fire", -1);
        AudioSource.PlayOneShot(attack1Sfx);
        // 흡수한 적 총알 객체 생성
        GameObject enemyDiedBullet = Instantiate(enemyDiedBulletPrefab, Transform.position, Transform.rotation);
        // 스프라이트 받아오기
        SpriteRenderer enemyDiedBulletSprite = enemyDiedBullet.GetComponent<SpriteRenderer>();

        switch (enemyType)
        {
            // 흡수한 적 객체가 유령타입일 경우 총알을 유령 Died 스프라이트로 변경
            case EnemyType.NONE:
                enemyDiedBulletSprite.sprite = enemyDiedBulletSprites[0];
                break;
            // 흡수한 적 객체가 방패타입일 경우 총알을 방패병 Died 스프라이트로 변경
            case EnemyType.SHIELD:
                enemyDiedBulletSprite.sprite = enemyDiedBulletSprites[1];
                break;
            // 흡수한 적 객체가 고글타입일 경우 총알을 고글 Died 스프라이트로 변경
            case EnemyType.GOGGLES:
                enemyDiedBulletSprite.sprite = enemyDiedBulletSprites[2];
                break;
            // 흡수한 적 객체가 거너타입일 경우 총알을 거너 Died 스프라이트로 변경
            case EnemyType.GUNNER:
                enemyDiedBulletSprite.sprite = enemyDiedBulletSprites[3];
                break;
            // 흡수한 적 객체가 청소부타입일 경우 총알을 청소부 Died 스프라이트로 변경
            case EnemyType.CLEANER:
                enemyDiedBulletSprite.sprite = enemyDiedBulletSprites[4];
                break;
        }

        // 마우스 클릭 지점과 플레이어의 스크린 좌표의 방향 벡터
        Vector2 playerToMouseVector = GetPlayerToMouseUnitVector();

        // 총알 발사: 총알 프리팹으로 부터 RigidBody를 받아와 마우스 클릭 지점으로 힘을 주어 총알 발사.
        Rigidbody2D enemyDiedBulletRigid = enemyDiedBullet.GetComponent<Rigidbody2D>();
        float bulletSpeed = enemyDiedBullet.GetComponent<BulletController>().GetBulletSpeed();
        enemyDiedBulletRigid.AddForce(playerToMouseVector * bulletSpeed);

        // 애니메이션 설정(삼킴 상태 해제)
        Anim.SetBool("isSwallowed", false);
    }
}
