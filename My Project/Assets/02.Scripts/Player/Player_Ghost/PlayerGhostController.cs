using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerGhostController : PlayerController
{
    // �뽬�ӵ�
    public float dashSpeed;
    // �뽬���ӽð�
    public float dashDuration;
    // �����Ŀ�
    public float jumpPower;

    // ���� ������ Ȯ���ϱ� ���� ����
    private bool isJumping;
    // �뽬 ������ Ȯ���ϱ� ���� ����
    private bool isDashing;
    // �뽬 ���� ���θ� Ȯ���� ����
    private bool isAbleDash;
    // �޶�ٱ� �������� Ȯ���ϱ� ���� ����
    private bool isSticking;

    // �� ������Ʈ�� ������ ����
    private GameObject enemyObject;
    // �� Ÿ���� ������ ����
    private EnemyType enemyType;

    // ���� ȿ����
    private AudioClip jumpSfx;
    // �뽬 ȿ����
    private AudioClip dashSfx;
    // ���� ȿ������
    private AudioClip attack1Sfx;
    private AudioClip attack2Sfx;

    // Animator �Ķ������ �ؽð� ����
    private readonly int hashWalk = Animator.StringToHash("isWalking");
    private readonly int hashJump = Animator.StringToHash("isJumping");
    private readonly int hashDash = Animator.StringToHash("isDashing");
    private readonly int hashStick = Animator.StringToHash("isSticking");

    void Start()
    {
        // ĳ�� ���� �Լ� ȣ��
        SetScrCash();
        SetCashComponent();

        // ���ҽ� �ε� �Լ� ȣ��
        LoadResources();
        // �÷��̾� �⺻ �ɷ�ġ ���� �Լ� ȣ��
        Init();
    }

    // ��ũ��Ʈ�� Ȱ��ȭ �ɶ� ���� ȣ��
    void OnEnable()
    {
        // ĳ�� ���� �Լ� ȣ��
        SetScrCash();
        SetCashComponent();
        // �÷��̾� �⺻ �ɷ�ġ ���� �Լ� ȣ��
        Init();
    }

    void OnDisable()
    {
        Init();
    }

    void Update()
    {
        // �߷� �Լ� ȣ��
        Gravity();

        //Jump
        Jump();

        HandleMouseInput();

    }


    // Update is called once per frame
    void FixedUpdate()
    {

        // ������ üũ �Լ� ȣ��
        Move();
    }


    /// <summary>
    /// �÷��̾� ���ǵ�, ���� �Ŀ�, �뽬 ���ӽð� �� �÷��̾� �⺻ �ɷ�ġ ���� �Լ�
    /// </summary>
    public override void Init()
    {
        // �̵� �ӵ� ����
        maxSpeed = 10.0f;

        // ���� �Ŀ� ����
        jumpPower = 33.0f;

        // �뽬 ���ӽð� ����
        dashDuration = 0.2f;

        isJumping = false;
        isDashing = false;
        isAbleDash = false;
        isSticking = false;
    }
    public override void LoadResources()
    {
        // ���� ȿ���� �ε�
        jumpSfx = Resources.Load<AudioClip>("PlayerAudios/jump");
        // �뽬 ȿ���� �ε�
        dashSfx = Resources.Load<AudioClip>("PlayerAudios/dash");
        // ����1 ȿ���� �ε�
        attack1Sfx = Resources.Load<AudioClip>("PlayerAudios/attack1");
        // ����2 ȿ���� �ε�
        attack2Sfx = Resources.Load<AudioClip>("PlayerAudios/attack2");
    }


    // �÷��̾� �߷�
    public override void Gravity()
    {
        //Landing Platform
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 1.0f)
                {
                    isJumping = false;
                    isAbleDash = true;
                    anim.SetBool("isJumping", false);
                }
                    
            }
        }  
        
    }

    // �÷��̾� ����
    public void Jump()
    {
        if (Input.GetButtonDown("Jump") && !isJumping && !isKnockBack)
        {
            // ����
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            // ���� ȿ���� ���
            audio.PlayOneShot(jumpSfx);
            // ���� �ִϸ��̼� ���
            anim.SetBool(hashJump, true);
            // ���� ���·� ����
            isJumping = true;
        }
    }
    // �÷��̾� �̵�
    public override void Move()
    {
        // �뽬 ���̰ų� �޶�ٱ�, �˹� ���� ��� ������ x
        if (isDashing || isSticking || isKnockBack) return;

        // �̵��ӵ��� 0.5f ������ ��� �̵� �ִϸ��̼� ����
        if (Mathf.Abs(rigid.velocity.x) < 0.5)
        {
            anim.SetBool(hashWalk, false);
        }
        // �̵� �ִϸ��̼� ����
        else
        {
            anim.SetBool(hashWalk, true);
        }

        // ������ ����
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        
        if (rigid.velocity.x > maxSpeed) // ������ ���� �ִ� �ӷ� ����
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) // ���� ���� �ִ� �ӷ� ����
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);


        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        // ��������Ʈ ���� ����
        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == 1;
        }
    }
    /// <summary>
    /// ���콺 �̺�Ʈ�� ���� �׼� ����
    /// </summary>
    public void HandleMouseInput()
    {
        // �Ͻ����� �޴� Ŭ���� �ÿ� �Ǵ°� ����
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                // �޶���� �����ϰ�� �޶�ٱ⸦ �����ϰ� �뽬
                if (isSticking)
                {

                    // �޶�ٱ� ���� ����
                    isSticking = false;
                    // ���� üũ
                    if (enemyObject == null)
                    {
                        Debug.LogError($"{0}: ������ �� ��ü�� �������� �ʽ��ϴ�.", this);
                        return;
                    }
                    // �� �˹� ����(�÷��̾ �뽬�ϴ� �ݴ� �������� �˹�)
                    enemyObject.GetComponent<Enemy>().KnockBack(-1 * GetPlayerToMouseUnitVector());
                    // �� ��ü ����
                    Destroy(enemyObject, 1.5f);
                    enemyObject = null;
                    // ���� ���� 2���� �ϳ� ���� ���
                    audio.PlayOneShot(Random.Range(0, 2) == 1 ? attack1Sfx : attack2Sfx);

                    // ����Ʈ ����
                    GenerateEffects();
                    // �߷°� ����
                    rigid.gravityScale = 8.0f;
                }
                // �뽬
                StartCoroutine(Dash());
            }
            // ���콺 ��Ŭ�� �̺�Ʈ
            if (Input.GetMouseButtonDown(1))
            {
                // �޶�ٱ� ���� ��츸 ��Ŭ�� �̺�Ʈ ����
                if (isSticking)
                {
                    // ���� ���� ������ ��ü�� ���
                    if(enemyType != EnemyType.NONE && enemyType != EnemyType.GUNNER)
                    {
                        // ���� üũ
                        if (enemyObject == null) {
                            Debug.LogError($"{0}: ������ �� ��ü�� �������� �ʽ��ϴ�.", this);
                            return;
                        } 

                        // �� ��ü ����
                        Destroy(enemyObject);

                        // �߷°� ����
                        rigid.gravityScale = 8.0f;

                        // �뽬 ���ɻ��·� ����
                        isAbleDash = true;

                        // �� Ÿ�Կ� ���� ���� ����
                        playerScr.ChangePlayer(GetChangePlayerType(enemyType));
                    }
                }
            }
        }
    }

    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // ��ũ��Ʈ�� ��Ȱ��ȭ ���� ��� return
        if (!enabled || isSticking) return;

        // ���� �浹���� ���
        if (collider.CompareTag("Enemy"))
        {
            if (isDashing)
            {
                // �뽬 ���� ��� �޶�ٱ� ����
                Debug.Log("�뽬 �� �޶�ٱ� ����");
                
                if (collider.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    // �� ��ü ����
                    enemyObject = collider.gameObject;
                    // �� ���¸� die�� ����
                    enemy.Died();
                    // �� ��ü Ÿ�� ����
                    enemyType = enemy.enemyType;
                    // ��ġ�� �� ��ü�� ���� ������
                    tr.position = new Vector2(collider.transform.position.x, collider.transform.position.y + 0.5f);
                    // �뽬 �ڷ�ƾ ����
                    StopCoroutine(Dash());
                    // �޶�ٱ� �ڷ�ƾ ȣ��
                    StartCoroutine(StickTo());
                }
            }
            else
            {
                if (isSticking || healthScr.isInvincible) return;
                // �뽬 ���� �ƴ� ���¿��� ���� �浹���� ��
                if(collider.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    // ���� �̹� ���� �����̸� ü�� ���� x
                    if (enemy.IsDied() == true) return;
                }
                Debug.Log($"{0}: ���� �浹�Ͽ� ü�� �ޱ�", this);
                // ���� �浹�Ͽ� �˹� ����
                if (healthScr.Damaged(1))
                {
                    Vector2 knockBackVec;
                    // �÷��̾ ���������� ���� ���� ��
                    if(rigid.velocity.x > 0.5f)
                    {
                        knockBackVec = new Vector2(-1.0f, 1.0f);
                    }
                    else if(rigid.velocity.x < -0.5f)
                    {
                        knockBackVec = new Vector2(1.0f, 1.0f);
                    }
                    else
                    {
                        if(collider.GetComponent<Rigidbody2D>().velocity.x >= 0)
                        {
                            knockBackVec = new Vector2(1.0f, 1.0f);
                        }
                        else
                        {
                            knockBackVec = new Vector2(-1.0f, 1.0f);
                        }
                    }
                    StartCoroutine(KnockBack(knockBackVec));
                }
                    
            }
        }
        // �Ѿ˰� �浹���� ���
        if (collider.CompareTag("Bullet"))
        {
            if (isSticking || healthScr.isInvincible) return;
            // �Ѿ��� ������ �о���� ���� ��ũ��Ʈ ������Ʈ ������
            if(collider.TryGetComponent<BulletController>(out BulletController bulletControllerScr))
            {
                Debug.Log($"{0}: �Ѿ˰� �浹�Ͽ� ü�� �ޱ�", this);
                // �Ѿ��� ���� �������� �˹�
                if (healthScr.Damaged(1))
                    StartCoroutine(KnockBack(new Vector2(Mathf.Sign(collider.gameObject.GetComponent<Rigidbody2D>().velocity.x), 1.0f)));
                Destroy(collider.gameObject);
            }

        }

    }

    /// <summary>
    /// �����κ��� �÷��̾� ĳ���͸� ������ EnemyType�� �޾ƿ��� �Լ�
    /// </summary>
    /// <param name="enemyType">������ ���ʹ� Ÿ��</param>
    /// <returns>�÷��̾� Ÿ�� ����</returns>
    public PlayerType GetChangePlayerType(EnemyType enemyType)
    {
        PlayerType playerType = PlayerType.PLAYERGHOST;
        switch (enemyType)
        {               
            // �� Ÿ���� ���к��� ���
            case EnemyType.SHIELD:
                // �ٲ� Ÿ���� ���з� ����
                playerType = PlayerType.PLAYERSHIELD;
                break;
            // �� Ÿ���� ��ۺ��� ���
            case EnemyType.GOGGLES:
                // �ٲ� Ÿ���� ��۷� ����
                playerType = PlayerType.PLAYERGOGGLES;
                break;
            // �� Ÿ���� û�Һ��� ���
            case EnemyType.CLEANER:
                // �ٲ� Ÿ���� û�Һη� ����
                playerType = PlayerType.PLAYERCLEANER;
                break;
        }
        // �÷��̾� Ÿ�� ��ȯ
        return playerType;
    }

    public void ChangePlayerToGhost()
    {
        playerScr.IsPossesing = false;
        // �÷��̾ �ٽ� �������� ����� ����Ʈ ������ �Բ� �뽬�ϱ�.
        GenerateEffects();

        // �뽬 ���ɻ��·� ����
        isAbleDash = true;
        StartCoroutine(Dash());
        Debug.Log($"{0}: �÷��̾ ���� Ÿ������ ��ȯ", this);
    }

    //�뽬
    /*
     * �뽬 �ð����� �߷°��� 0���� ���� �Ŀ�, �÷��̾��� ���� ��ǥ�� ��ũ�� ��ǥ�� ��ȯ�� �Ŀ� ���콺�� ��ũ�� ��ǥ���� �y��������
     * ���� ���� ���͸� ���ϰ� �̸� �������ͷ� �����Ѵ�. �׸��� rigid�� velocity(�ӷ�)���� ���� ���� ���Ϳ� �ӷ� ���� ���� ������ �����Ͽ�
     * �÷��̾ �� �������� �뽬�� �� �ֵ��� �����Ѵ�.
     * 
     * Raycast�� ����ؼ� �� ���⿡ Ÿ�ϸ��� �ִٸ� �׳� �������� �뽬�� �� �ֵ��� �����غ��� �ҵ���.
     * */
    public IEnumerator Dash()
    {
        //�뽬�� ������ ���
        if (isAbleDash)
        {
            // �뽬 �ִϸ��̼� ����
            anim.SetBool(hashDash, true);
            // �뽬 ȿ���� ���
            audio.PlayOneShot(dashSfx);
            // �÷��̾� ���¸� �뽬 ���·� ����
            isDashing = true;
            // �뽬 �Ұ��� ���·� ����
            isAbleDash = false;

            // ���� �߷°� ����
            var originalGravityScale = rigid.gravityScale;
            if(rigid.gravityScale == 0)
            {
                originalGravityScale = 8.0f;
            }
            // �߷°��� 0���� ����
            rigid.gravityScale = 0f;
            

            Vector2 playerToMouseVector = GetPlayerToMouseUnitVector();
            
            //�뽬�� �� ���콺 ��ġ�� ���� ȸ��
            if (playerToMouseVector.x > 0)
            {
                if (spriteRenderer.flipX == false)
                    spriteRenderer.flipX = true;
            }
            else
            {
                if (spriteRenderer.flipX == true)
                    spriteRenderer.flipX = false;
            }

            // ���� �������� �뽬(���ӷ�)
            rigid.velocity = playerToMouseVector * dashSpeed;

            // �뽬 ���ӽð� ���� ����� �絵
            yield return new WaitForSeconds(dashDuration);

            // �뽬 ���� ����
            isDashing = false;
            // �뽬 �ִϸ��̼� ����
            anim.SetBool(hashDash, false);
            
            if(!isSticking)
                // �߷°� �ٽ� ����.
                rigid.gravityScale = originalGravityScale;
        }
    }
    //�޶�ٱ�
    public IEnumerator StickTo()
    {
        isSticking = true;
        isAbleDash = true;
        // �޶�ٱ� �ִϸ��̼�, ���� ���� �� �߷°�, �ӵ� �ʱ�ȭ 
        anim.SetBool(hashStick, true);
        rigid.gravityScale = 0f;
        rigid.velocity = new Vector2(0, 0);

        // �޶�ٱⰡ ���� �ɶ����� ���
        yield return new WaitUntil(() => isSticking == false);

        // �޶�ٱ� �ִϸ��̼� ���� �� �߷°� ����
        anim.SetBool(hashStick, false);
    }

}
