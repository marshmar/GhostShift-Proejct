using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class GogglesController : PlayerControllerR
{
    public Action<bool> IsInNVDMode;

    private GameObject tileMap;
    private GameObject background;

    private Material originalBackground;
    private Material NVDBackground;
    private Tilemap tileMapSpr;

    protected override void Awake()
    {
        base.Awake();

        IsInNVDMode += nvdAction;
    }

    protected override void Start()
    {
        base.Start();
        NVDBackground = Resources.Load<Material>("Materials/NVDMaterial");
    }

    private void OnEnable()
    {
        tileMap = GameObject.Find("Tilemap");
        tileMapSpr = tileMap.GetComponent<Tilemap>();
        background = GameObject.Find("Quad");
        originalBackground = background.GetComponent<MeshRenderer>().material;
    }

    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (enabled == false)
        {
            return;
        }

        if (collider.CompareTag("Enemy"))
        {
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

    public override State HandleSpecialStateInput()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            return new NVDState(this);
        }

        if (Input.GetMouseButtonDown(0))
        {
            GenerateShakeEffect();
            playerManager.ChangeCharType(PlayerType.PLAYERGHOST);
            return new DashState(GetCurrentController());
        }

        return base.HandleSpecialStateInput();
    }

    private void nvdAction(bool isInNVDModes)
    {
        if (isInNVDModes)
            ChangeColorToGreen();
        else
            ChangeColorToWhite();
    }

    public void ChangeColorToGreen()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.green;
        tileMapSpr.color = Color.green;
        background.GetComponent<MeshRenderer>().material = NVDBackground;
    }

    public void ChangeColorToWhite()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        tileMapSpr.color = Color.white;
        background.GetComponent<MeshRenderer>().material = originalBackground;
    }
}
