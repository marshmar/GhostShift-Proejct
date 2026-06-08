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

    private Dictionary<PlayerType, GameObject> charObjs;
    private Dictionary<PlayerType, PlayerController> charControllers;
    
    protected virtual void Awake()
    {
        Rigid2D = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Anim = GetComponentInChildren<Animator>();
        Collider = GetComponentInChildren<CapsuleCollider2D>();
        Transform = GetComponent<Transform>();
        AudioSource = GetComponent<AudioSource>();
        playerManager = GetComponent<PlayerManager>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    protected virtual void Update()
    {

    }

    public virtual void Initialize()
    {

    }

    public virtual State HandleSpecialStateInput()
    {
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

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Enemy"))
        {
            Debug.Log("Enemy collision");
        }
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
}
