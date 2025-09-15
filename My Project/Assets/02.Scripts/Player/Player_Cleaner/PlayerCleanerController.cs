using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCleanerController : PlayerController
{
    // û�Һ� ĳ���� public ����
    #region PlayerCleaner Public Properties
    // ��Ű�� �ӵ�
    public float swallowSpeed;
    #endregion
    // û�Һ� ĳ���� private ����
    #region PlayerCleaner Private Properties
    // ��Ű�� ����
    private Vector3 swallowRange;
    // ��Ű�� ���϶� ����
    private bool isSwallowing;
    // ��Ŵ ������ ����
    private bool isSwallowed;
    // ��Ų ��(��Ÿ��)�� ������ ť
    private Queue<EnemyType> swalloedEnemy = new Queue<EnemyType>();
    // enemyDied ��������Ʈ�� ������ �迭 ����
    private Sprite[] enemyDiedBulletSprites = new Sprite[5];
    // �Ѿ� �߻�� ������Ʈ Ǯ������ �����غ���
    private GameObject enemyDiedBulletPrefab;

    // Audio Clips
    private AudioClip attack1Sfx;
    private AudioClip attack2Sfx;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        SetScrCash();
        SetCashComponent();
        Init();
        LoadResources();

    }

    private void OnEnable()
    {
        SetScrCash();
        SetCashComponent();
        Init();

    }
    void FixedUpdate() {
        // ��Ű�� ���� �ƴ� ���
        if(!isSwallowing)
            // �̵�
            Move();
    }

    // Update is called once per frame
    void Update()
    {
        // ���콺 �Է� ����
        HandleMouseInput();
    }

    // �ʱ�ȭ
    public override void Init()
    {
        base.Init();

        isSwallowed = false;
        isSwallowing = false;

        swallowRange = new Vector3(8.0f, 1.0f, 0.0f);
        

    }

    public override void LoadResources()
    {
        // �÷��̾� �Ѿ� �޾ƿ���
        enemyDiedBulletPrefab = Resources.Load<GameObject>("BulletPrefabs/PlayerBullet");
        // ���� Ÿ�� �� ��� ��������Ʈ �ε�
        enemyDiedBulletSprites[0] = Resources.Load<Sprite>("EnemyImages/Enemy_Ghost/Enemy_ghost_dead");
        // ���к� Ÿ�� �� ��� ��������Ʈ �ε�
        enemyDiedBulletSprites[1] = Resources.Load<Sprite>("EnemyImages/Enemy_Shield/Enemy_shield_dead");
        // ��� Ÿ�� �� ��� ��������Ʈ �ε�
        enemyDiedBulletSprites[2] = Resources.Load<Sprite>("EnemyImages/Enemy_Goggles/Enemy_goggles_dead");
        // �ų� Ÿ�� �� ��� ��������Ʈ �ε�
        enemyDiedBulletSprites[3] = Resources.Load<Sprite>("EnemyImages/Enemy_Gunner/Enemy_gun_dead");
        // û�Һ� Ÿ�� �� ��� ��������Ʈ �ε�
        enemyDiedBulletSprites[4] = Resources.Load<Sprite>("EnemyImages/Enemy_Gunner/Enemy_gun_dead");

        attack1Sfx = Resources.Load<AudioClip>("PlayerAudios/attack1");
        attack2Sfx = Resources.Load<AudioClip>("PlayerAudios/attack2");
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!enabled) return;
        // EnemyŸ���� ��ü�� �浹�ϰ� �ȴٸ�
        if (collider.CompareTag("Enemy"))
        {
            // ��Ű�� ���̶�� ������ X
            if (isSwallowing)
            {
                if(collider.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    // ��Ű�� �� ��Ȱ��ȭ
                    isSwallowing = false;
                    // ��Ŵ ���� Ȱ��ȭ
                    isSwallowed = true;
                    // ��Ŵ �ִϸ��̼� Ȱ��ȭ
                    anim.SetBool("isSwallowed", true);
                    // �� ��ü ���ֱ�
                    Destroy(collider.gameObject);
                    // �߻��� �� ��ü �߰�
                    swalloedEnemy.Enqueue(enemy.enemyType);
                }
               
            }
            // ��Ű�� ���� �ƴ϶��
            else
            {
                Debug.Log("û�Һ� ĳ���� ������ �Ա�");
                // ������ �Ա�
                if (healthScr.Damaged(1))
                {
                    Vector2 knockBackVec;
                    // �÷��̾ ���������� ���� ���� ��
                    if (rigid.velocity.x > 0.5f)
                    {
                        knockBackVec = new Vector2(-1.0f, 1.0f);
                    }
                    else if (rigid.velocity.x < -0.5f)
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
                    StartCoroutine(KnockBack(knockBackVec));
                }
            }
        }
        // �Ѿ˰� �浹���� ���
        if (collider.CompareTag("Bullet"))
        {
            // �Ѿ��� ������ �о���� ���� ��ũ��Ʈ ������Ʈ ������
            if (collider.TryGetComponent<BulletController>(out BulletController bulletControllerScr))
            {
                Debug.Log($"{0}: �Ѿ˰� �浹�Ͽ� ü�� �ޱ�", this);
                // �Ѿ��� ���� ������ �ݴ� �������� �˹�
                if (healthScr.Damaged(1))
                    StartCoroutine(KnockBack(new Vector2(Mathf.Sign(collider.gameObject.GetComponent<Rigidbody2D>().velocity.x), 1.0f)));
                Destroy(collider.gameObject);
            }
        }



    }
 
    private void HandleMouseInput() {
        // �Ͻ����� �޴� Ŭ���� �ÿ� �Ǵ°� ����
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //��Ŵ ������ ��� �÷��̾� ���� �Ұ� �ϵ���
            if (!isSwallowed)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // �÷��̾� �ٲٸ鼭 �뽬
                    ChangePlayer();
                }
            }

            // ��Ŵ ���°� �ƴҶ� ���콺 ������ Ŭ�� ��
            if (!isSwallowed /*|| !anim.GetCurrentAnimatorStateInfo(0).IsName("Fire")*/)
            {
                // ��Ű��
                if (Input.GetMouseButton(1))
                {
                    StartCoroutine(Swallow());
                }
                //
                if (Input.GetMouseButtonUp(1))
                {

                    // ��Ű�� ���� �ƴҶ����� ��Ű�� ���߱�
                    if (!isSwallowing)
                    {
                        anim.SetBool("isSwallowing", false);
                        StopCoroutine(Swallow());
                    }

                }
            }
            // ��Ŵ ������ �� ���콺 ������ Ŭ�� �� �߻� ���� ����
            else
            {
                if (Input.GetMouseButtonDown(1))
                {
                    Fire(swalloedEnemy.Dequeue());
                }
                
            }
           
        }
    }

    // �÷��̾� ����
    public void ChangePlayer()
    {
        Init();
        rigid.gravityScale = 8.0f;
        playerScr.ChangePlayer(PlayerType.PLAYERGHOST);
    }

    // ��Ű��
    IEnumerator Swallow()
    {
        // ��Ű�� �ִϸ��̼� ����
        anim.SetBool("isSwallowing", true);

        // �÷��̾� �̵� ���⿡ ���� ��Ű�� ���� ����
        if((spriteRenderer.flipX == false && swallowRange.x > 0) || (spriteRenderer.flipX == true && swallowRange.x < 0))
        {
            swallowRange.x *= -1;
        }

        //Physics2D.OverlapAreaAll : ������ ���簢���� ����� �����Ϸ��� �ݰ� �̳��� ���� �ִ� �ݶ��̴����� �迭 ���·ι�ȯ�ϴ� �Լ�
        Collider2D[] colliderArray = Physics2D.OverlapAreaAll(tr.position, tr.position + swallowRange);
        // �ݶ��̴� �迭�� ��ȯ�ϸ鼭
        for(int i = 0; i < colliderArray.Length; i++)
        {
            // null�̸� continue;
            if (colliderArray[i] == null) continue;
            // ������ ���ʹ̰� ������
            if(colliderArray[i].tag == "Enemy")
            {
                // ��Ű�� ��
                isSwallowing = true;
                // �� ��ü�� Die���·� ����
                if(colliderArray[i].TryGetComponent<Enemy>(out Enemy enemy))
                {
                    enemy.Died();
                }
                while(isSwallowing)
                {
                    // �� �̻� ���ʹ̰� ������ �ݺ��� ����
                    if (colliderArray[i] == null || isSwallowed) {
                        isSwallowing = false;
                        break;
                    }
                    
                    // ���� �÷��̾��� ���� ���͸� ���ϰ�
                    Vector3 dir = (colliderArray[i].transform.position - transform.position).normalized;
                    // ���� �����ǿ� ���� ���͸� ���Ͽ� ���� �÷��̾��� ��ġ�� ������
                    dir = new Vector3(dir.x * 2.0f, dir.y * 2.0f, dir.z * 2.0f) ;
                    // ������� �ӵ� ����
                    dir *= swallowSpeed * Time.deltaTime;
                    // �������
                    colliderArray[i].transform.position -= dir;

                    // �� ������ ����� �Ѱ��ֱ�
                    yield return null;
                }       
            }
        }
        // �������� ����Ǹ�(���콺 ������ Ȧ���� ����Ǹ�) �ִϸ��̼� �� �ڷ�ƾ ����,
        if (isSwallowed) {
            anim.SetBool("isSwallowing", false);
            yield break;
        } 
        
    }

    // ��Ų �� �߻�
    public void Fire(EnemyType enemyType)
    {
        anim.Play("player_cleaner_fire", -1);
        audio.PlayOneShot(attack1Sfx);
        // ����� �� �Ѿ� ��ü ����
        GameObject enemyDiedBullet = Instantiate(enemyDiedBulletPrefab, tr.position, tr.rotation);
        // ��������Ʈ �޾ƿ���
        SpriteRenderer enemyDiedBulletSprite = enemyDiedBullet.GetComponent<SpriteRenderer>();

        switch (enemyType)
        {
            // ����� �� ��ü�� ����Ÿ���� ��� �Ѿ��� ���� Died ��������Ʈ�� ����
            case EnemyType.NONE:
                enemyDiedBulletSprite.sprite= enemyDiedBulletSprites[0];
                break;
            // ����� �� ��ü�� ����Ÿ���� ��� �Ѿ��� ���к� Died ��������Ʈ�� ����
            case EnemyType.SHIELD:
                enemyDiedBulletSprite.sprite = enemyDiedBulletSprites[1];
                break;
            // ����� �� ��ü�� ���Ÿ���� ��� �Ѿ��� ��� Died ��������Ʈ�� ����
            case EnemyType.GOGGLES:
                enemyDiedBulletSprite.sprite = enemyDiedBulletSprites[2];
                break;
            // ����� �� ��ü�� �ų�Ÿ���� ��� �Ѿ��� �ų� Died ��������Ʈ�� ����
            case EnemyType.GUNNER:
                enemyDiedBulletSprite.sprite = enemyDiedBulletSprites[3];
                break;
            // ����� �� ��ü�� û�Һ�Ÿ���� ��� �Ѿ��� û�Һ� Died ��������Ʈ�� ����
            case EnemyType.CLEANER:
                enemyDiedBulletSprite.sprite = enemyDiedBulletSprites[4];
                break;
        }



        // ���콺 Ŭ�� ������ �÷��̾��� ��ũ�� ��ǥ�� ���� ����
        Vector2 playerToMouseVector = GetPlayerToMouseUnitVector();

        // �Ѿ� �߻�: �Ѿ� ���������� ���� RigidBody�� �޾ƿ� ���콺 Ŭ�� �������� ���� �־� �Ѿ� �߻�.
        Rigidbody2D enemyDiedBulletRigid = enemyDiedBullet.GetComponent<Rigidbody2D>();
        float bulletSpeed = enemyDiedBullet.GetComponent<BulletController>().GetBulletSpeed();
        enemyDiedBulletRigid.AddForce(playerToMouseVector * bulletSpeed);


        // ��Ŵ ���� ����
        isSwallowed = false;
        // �ִϸ��̼� ����(��Ŵ ���� ����)
        anim.SetBool("isSwallowed", false);
        return;
    }
}
