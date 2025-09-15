using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;
    public GameObject playerFoundUi;
    public GameObject canPossesUi;
    private GameObject bulletPrefab;

    protected Animator anim;
    protected CapsuleCollider2D coll;
    protected Rigidbody2D rigid;
    protected SpriteRenderer spriteRenderer;
    protected new AudioSource audio;
    protected float knockBackPower;
    
    // �̵� ����
    private Vector2 moveDir;
    // �̵� ���� ������Ƽ
    public Vector2 MoveDir { get; set; }
    // enemy�� ���� ��ǥ
    private Transform enemyTr;
    // player�� ���� ��ǥ
    private Transform playerTr;

    
    // ��� ����
    private bool waiting = false;
    // Gunner ĳ���� attackCount
    private int attackCount = 0;
    // Gunner ĳ���� shootAudio
    private AudioClip shootSfx;
    // Gunner ĳ���� �Ѿ˹߻� ����Ʈ
    private GameObject shootFlashPrefab;
    // Animator �Ķ������ �ؽð� ����
    private readonly int hashWalk = Animator.StringToHash("isWalking");
    private readonly int hashDie = Animator.StringToHash("Die");
    // Enemy�� ��� ����
    [SerializeField]
    public bool isDied;

    public enum State
    {
        IDLE,
        MOVE,
        FOUND,
        TRACE,
        ESCAPE,
        ATTACK
    }

    // Enemy�� ���� ����
    public State state;
    // ���� ����
    private Vector3 sightRange = new Vector3(10, 0.5f, 0);
    // ���� �ӵ�
    public float traceSpeed = 1.5f;



    IEnumerator Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<CapsuleCollider2D>();
        isDied = false;
        rigid = GetComponent<Rigidbody2D>();
        knockBackPower = 10.0f;
        enemyTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();

        // �Ѿ� ������ �ε�
        bulletPrefab = Resources.Load<GameObject>("BulletPrefabs/Bullet");
        // �Ѿ� ����Ʈ �ε�
        shootFlashPrefab = Resources.Load<GameObject>("Effects/FX_Fire");
        // �߻� ȿ���� �ε�
        shootSfx = Resources.Load<AudioClip>("EnemyAudios/gun_fire");
        // Enemy�� ���¸� üũ�ϴ� �ڷ�ƾ �Լ� ȣ��
        StartCoroutine(CheckEnemyState());
        // ���¿� ���� Enemy�� �ൿ�� �����ϴ� �ڷ�ƾ �Լ� ȣ��
        StartCoroutine(EnemyAction());


        yield return new WaitForSeconds(1.0f);
        if (enemyType == EnemyType.GUNNER)
            yield return new WaitForSeconds(2.0f);
        state = State.IDLE;
        // �̵� ���� ���� ����
        moveDir = Random.Range(0, 2) == 0 ? Vector2.left : Vector2.right;
        SetSpriteFlipX();

        if(enemyType == EnemyType.CLEANER || enemyType == EnemyType.GOGGLES || enemyType == EnemyType.SHIELD)
        {
            canPossesUi.SetActive(true);
        }
        else
        {
            canPossesUi.SetActive(false);
        }
        playerFoundUi.SetActive(false);

    }

    // Update is called once per frame
    void LateUpdate()
    {

    }

    // ������ �������� Enemy�� �ൿ ���¸� üũ
    protected virtual IEnumerator CheckEnemyState()
    {
        while (!isDied)
        {
            // 0.3�� ���� ����(���)�ϴ� ���� ������� �޽��� ������ �纸
            yield return new WaitForSeconds(0.3f);

            if(state == State.TRACE || state == State.ESCAPE || state == State.ATTACK)
            {
                continue;
            }


            if (CheckFront() || CheckBottom()) {
                state = State.IDLE;
                moveDir *= -1;
                waiting = true;
            }
            else
            {
                state = State.MOVE;
            }

            if (CheckPlayerInSight() && state != State.FOUND && !waiting)
            {
                state = State.FOUND;
            }

        }
    }

    protected virtual IEnumerator EnemyAction()
    {
        while (!isDied)
        {
            switch (state)
            {
                case State.IDLE:
                    anim.SetBool(hashWalk, false);
                    rigid.velocity = Vector2.zero;
                    yield return new WaitForSeconds(1.8f);
                    waiting = false;
                    SetSpriteFlipX();
                    break;
                case State.MOVE:
                    Move();
                    break;
                case State.FOUND:
                    // �÷��̾� �߰� ���� �� ����ǥ ��������Ʈ ����
                    if (enemyType == EnemyType.CLEANER || enemyType == EnemyType.GOGGLES || enemyType == EnemyType.SHIELD)
                    {
                        canPossesUi.SetActive(false);
                    }
                    playerFoundUi.SetActive(true);
                    // �� ĳ���� Ÿ���� ������ ��� TRACE ���·� ��ȯ
                    if (enemyType == EnemyType.NONE || enemyType == EnemyType.CLEANER)
                    {
                        state = State.TRACE;
                    }
                    // �Ѻ� Ÿ���� ��� Attack ���·� ��ȯ
                    else if(enemyType == EnemyType.GUNNER)
                    {
                        state = State.ATTACK;
                    }
                    // �� ���ǰ�� Escape ���·� ��ȯ
                    else 
                    {
                        state = State.ESCAPE;
                    }

                    anim.SetBool(hashWalk, false);
                    rigid.velocity = Vector2.zero;
                    break;
                case State.TRACE:
                    if (enemyType == EnemyType.CLEANER || enemyType == EnemyType.GOGGLES || enemyType == EnemyType.SHIELD)
                    {
                        canPossesUi.SetActive(true);
                    }
                    playerFoundUi.SetActive(false);
                    Trace();
                    break;
                case State.ESCAPE:
                    if (enemyType == EnemyType.CLEANER || enemyType == EnemyType.GOGGLES || enemyType == EnemyType.SHIELD)
                    {
                        canPossesUi.SetActive(true);
                    }
                    playerFoundUi.SetActive(false);
                    Escape();
                    break;
                case State.ATTACK:
                    playerFoundUi.SetActive(false);
                    while (attackCount < 3 && !isDied)
                    {
                        Debug.Log("���� ����");
                        Attack();
                        yield return new WaitForSeconds(0.6f);
                        attackCount++;
                    }
                    anim.SetBool("isShooting", false);
                    state = State.IDLE;
                    attackCount = 0;
                    break;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }
    public void Move()
    {
        anim.SetBool(hashWalk, true);
        rigid.velocity = moveDir;
    }
    /// <summary>
    /// �տ� ��ֹ��� üũ�ϴ� �Լ�
    /// </summary>
    /// <returns>���� ������ True, ������ False ��ȯ</returns>
    public bool CheckFront()
    {
        Vector2 frontVec = new Vector2(rigid.position.x + moveDir.x, rigid.position.y-0.2f);
        Debug.DrawRay(frontVec, moveDir * 0.4f, new Color(1, 0, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, moveDir, 0.4f, LayerMask.GetMask("Platform"));
        rayHit = Physics2D.Raycast(frontVec, moveDir, 0.2f, LayerMask.GetMask("Platform"));
        if (rayHit.collider != null)
        {
            return true;
        }
        
        return false;
    }
    /// <summary>
    /// �ٴڿ� �ִ� ��ֹ��� üũ�ϴ� �Լ�
    /// </summary>
    /// <returns>�ٴڿ� ��ֹ��� ������ true ������ false ��ȯ</returns>
    public bool CheckBottom()
    {
        Vector2 frontVec = new Vector2(rigid.position.x + moveDir.x, rigid.position.y);
        Debug.DrawRay(frontVec, Vector2.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector2.down, 1.5f, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null || rayHit.collider.CompareTag("NVDOBJECTS"))
        {
            return true;
        }
        return false;
    }
    public bool CheckPlayerInSight()
    {
        if (Mathf.Sign(sightRange.x) != Mathf.Sign(moveDir.x))
        {
            sightRange.x *= -1;
        }
        //Physics2D.OverlapAreaAll : ������ ���簢���� ����� �����Ϸ��� �ݰ� �̳��� ���� �ִ� �ݶ��̴����� �迭 ���·ι�ȯ�ϴ� �Լ�
        Collider2D[] colliderArray = Physics2D.OverlapAreaAll(enemyTr.position, enemyTr.position + sightRange);
        // �ݶ��̴� �迭�� ��ȯ�ϸ鼭
        for (int i = 0; i < colliderArray.Length; i++)
        {
            // null�̸� continue;
            if (colliderArray[i] == null) continue;
            // ������ ���ʹ̰� ������
            if (colliderArray[i].CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }
    public void Trace()
    {
        float distance = Vector2.Distance(playerTr.position, enemyTr.position);
        float dir = enemyTr.position.x - playerTr.position.x;
        
        if (((Mathf.Sign(moveDir.x) == Mathf.Sign(dir))) || (CheckFront() || (CheckBottom() && enemyType != EnemyType.NONE)))
        {
            moveDir *= -1;
        }
        anim.SetBool(hashWalk, true);
        SetSpriteFlipX();
        rigid.velocity = moveDir * traceSpeed;
    }
    public void Escape()
    {
        float distance = Vector2.Distance(playerTr.position, enemyTr.position);
        float dir = enemyTr.position.x - playerTr.position.x;

        if ((CheckPlayerInSight() && (Mathf.Sign(moveDir.x) != Mathf.Sign(dir)))|| CheckFront() || CheckBottom())
        {
            moveDir *= -1;
        }
        anim.SetBool(hashWalk, true);
        SetSpriteFlipX();
        rigid.velocity = moveDir * traceSpeed;
    }
    public void Attack()
    {
        anim.SetBool("isShooting", true);
        Debug.Log(MoveDir.x);
        GameObject bullet = Instantiate(bulletPrefab, new Vector3(enemyTr.position.x + moveDir.x, enemyTr.position.y, enemyTr.position.z), enemyTr.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(moveDir * 150.0f);
        audio.PlayOneShot(shootSfx);

        // ����Ʈ ���� ������Ʈ ����
        GameObject shootflash = Instantiate(shootFlashPrefab, new Vector3(enemyTr.position.x + moveDir.x, enemyTr.position.y-0.1f, enemyTr.position.z), enemyTr.rotation);

        // 0.2�ʵڿ� ����Ʈ ����
        Destroy(shootflash, 0.2f);
}
    public void SetSpriteFlipX()
    {
        if (moveDir == Vector2.right)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
    public void Died()
    {
        playerFoundUi.SetActive(false);
        anim.SetTrigger(hashDie);
        isDied = true;
        coll.isTrigger = true;
        rigid.velocity = new Vector2(0,0);
    }

    // �� ����� �˹�
    public void KnockBack(Vector2 dir)
    {
        rigid.constraints = RigidbodyConstraints2D.None;
        rigid.AddForce(new Vector2(knockBackPower * dir.x, 30.0f), ForceMode2D.Impulse);
        rigid.gravityScale = 8.0f;
    }
    public bool IsDied() { return this.isDied; }

}
