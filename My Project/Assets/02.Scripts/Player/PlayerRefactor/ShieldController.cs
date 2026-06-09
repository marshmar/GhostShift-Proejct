using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : PlayerControllerR
{
    public GameObject shield;
    public Vector3 shieldPosition;
    private Shield shieldScr;
    private AudioClip swingSfx;
    private bool isDefended;

    protected override void Awake()
    {
        base.Awake();
        shieldPosition = new Vector3(0.8263f, 0, 0);
        shieldScr = shield.GetComponent<Shield>();
        shieldScr.defendSuccess += () => isDefended = true;
    }

    protected override void Start()
    {
        base.Start();
        swingSfx = Resources.Load<AudioClip>("PlayerAudios/swing");
    }

    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if(enabled == false)
        {
            return;
        }

        if(collider.CompareTag("Enemy"))
        {
            DamagePlayerAndKnockBack(collider);
        }

        // 총알과 충돌했을 경우
        if (collider.CompareTag("Bullet"))
        {
            if(isDefended)
            {
                isDefended = false;
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

    public override State HandleSpecialInput()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            return new DefendState(this);
        }

        if (Input.GetMouseButtonDown(0))
        {
            GenerateShakeEffect();
            playerManager.ChangeCharType(PlayerType.PLAYERGHOST);
            return new DashState(GetCurrentController());
        }

        if (Input.GetMouseButtonDown(1))
        {
            return new ParryState(this);
        }

        return base.HandleSpecialInput();
    }

    public void SetShieldObject(bool value)
    {
        shield.SetActive(value);
    }

    public void SetParryState(bool value)
    {
        shieldScr.isParrying = value;
    }

    public void PlaySwingSfx()
    {
        AudioSource.clip = swingSfx;
        AudioSource.Play();
    }
}
