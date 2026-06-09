using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerR : MonoBehaviour
{
    // ------------------------------------------
    public Rigidbody2D Rigid2D;
    public SpriteRenderer SpriteRenderer;
    public Animator Anim;
    public CapsuleCollider2D Collider;
    public Transform Transform;
    public AudioSource AudioSource;

    // ------------------------------------------

    protected PlayerManager playerManager;

    private Health healthScr;
    private Vector2 knockBackVec;
    private Dictionary<PlayerType, GameObject> charObjs;
    private Dictionary<PlayerType, PlayerController> charControllers;
    private AudioClip damagedSfx;
    private AudioClip jumpSfx;
    private bool isCalledKnockbackEvent;

    public GameObject hitEffect;

    protected virtual void Awake()
    {
        Rigid2D = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Anim = GetComponentInChildren<Animator>();
        Collider = GetComponentInChildren<CapsuleCollider2D>();
        Transform = GetComponent<Transform>();
        AudioSource = GetComponent<AudioSource>();
        playerManager = GetComponent<PlayerManager>();
        healthScr = GetComponent<Health>();
    }

    protected virtual void Start()
    {
        isCalledKnockbackEvent = false;
        Cursor.lockState = CursorLockMode.Confined;

        // 효과음 로드
        jumpSfx = Resources.Load<AudioClip>("PlayerAudios/jump");
        damagedSfx = Resources.Load<AudioClip>("PlayerAudios/damaged");
    }

    protected virtual void Update()
    {}

    public virtual void Initialize()
    {}

    public virtual State HandleSpecialInput()
    {
        if(isCalledKnockbackEvent)
        {
            isCalledKnockbackEvent = false;
            return new KnockBackState(this, knockBackVec);
        }
        return null;
    }

    // 마우스 입력 좌표로부터 플레이어 까지의 방향 벡터 구하기
    public virtual Vector2 GetPlayerToMouseUnitVector()
    {
        // 플레이어의 월드 좌표를 스크린 좌표로 변경
        Vector2 playerScreenPosition = transform.position;
        // 마우스 좌클릭시의 마우스 스크린 좌표
        Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // 마우스 클릭 지점과 플레이어의 스크린 좌표의 방향 벡터
        Vector2 playerToMouseVector = (mouseScreenPosition - playerScreenPosition).normalized;

        return playerToMouseVector;
    }

    public void SetCashComponent()
    {
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Anim = GetComponentInChildren<Animator>();
        Collider = GetComponentInChildren<CapsuleCollider2D>();
    }

    public PlayerControllerR GetCurrentController()
    {
        return playerManager.GetCurrentController();
    }

    public void PlayKnockBackAudio()
    {
        AudioSource.clip = damagedSfx;
        AudioSource.Play();
    }

    public void PlayJumpAudio()
    {
        AudioSource.clip = jumpSfx;
        AudioSource.Play();
    }

    public void DamagePlayerAndKnockBack(Collider2D collider)
    {
        Debug.Log("Enemy collision");
        // 적과 충돌하여 넉백 실행
        if (healthScr.Damaged(1))
        {
            // 플레이어가 오른쪽으로 가고 있을 때
            if (Rigid2D.velocity.x > 0.5f)
            {
                knockBackVec = new Vector2(-1.0f, 1.0f);
            }
            else if (Rigid2D.velocity.x < -0.5f)
            {
                knockBackVec = new Vector2(1.0f, 1.0f);
            }
            else
            {
                if (collider.GetComponent<Rigidbody2D>().velocity.x >= 0)
                {
                    knockBackVec = new Vector2(1.0f, 1.0f);
                }
                else
                {
                    knockBackVec = new Vector2(-1.0f, 1.0f);
                }
            }
            isCalledKnockbackEvent = true;
        }
    }

    public void DamagePlayerAndKnockBack(Vector2 dir)
    {
        Debug.Log("Enemy collision");
        // 적과 충돌하여 넉백 실행
        if (healthScr.Damaged(1))
        {
            knockBackVec = dir;
            isCalledKnockbackEvent = true;
        }
    }

    public void GenerateShakeEffect()
    {
        // 이펙트 게임 오브젝트 생성 및 카메라 쉐이크
        GameObject hitflash = Instantiate(hitEffect, transform.position, transform.rotation);
        CameraShake.Instance.OnShakeCamera();

        // 0.2초뒤에 이펙트 삭제
        Destroy(hitflash, 0.2f);
    }
}
