using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour
{
    [SerializeField]
    public float hp;
    private float maxHp; // 최대 HP 값, Start에서 초기화
    [SerializeField]
    public float speed;
    [SerializeField]
    public float AttackRange;
    [SerializeField]
    protected int experiencePoints;
    [SerializeField] protected float patrolWallDistance = 3f; // 순찰 시 탐지할 최대 거리
    [SerializeField] protected float groundCheckDistance = 0.5f; // 낭떠러지 탐지 거리
    [SerializeField] protected LayerMask groundLayer; // 땅과 벽의 레이어
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackSound; // 공격 소리
    [SerializeField] private Transform hpBarTransform; // HP 바 오브젝트의 Transform
    protected Vector3 patrolDirection = Vector3.right; // 순찰 방향
    protected bool isPatrolling = true;
    protected Transform player;
    protected Animator anim;
    protected PolygonCollider2D polygonCollider;
    protected SpriteRenderer spriteRenderer;
    protected Sprite previousSprite;
    public float detectionRange = 10f;
    protected bool FirstAnimation = false;
    protected string FirstAnim;
    protected string attackAnim;
    protected string idleAnim;
    protected string runAnim;
    protected bool FlipSprite = true;
    private bool isDying = false; // 몬스터가 이미 죽음을 처리 중인지 확인
    private bool isInvincible = false; // 데미지 중복 방지

    protected virtual void Awake()
    {
        polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // ��������Ʈ ������ ��������
    }

    protected virtual void Start()
    {
        maxHp = hp;
        // �±� "Player"�� ������Ʈ ã��
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform; // �÷��̾��� Transform�� ������
        }
        else
        {
            Debug.LogError("Player with tag 'Player' not found!");
        }
    }

    protected virtual void Update()
    {

        if (isDying) // 죽음 상태일 때 나머지 애니메이션 건너뜀
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("death") && stateInfo.normalizedTime >= 1.0f)
            {
                Destroy(gameObject);
            }
            return; // 더 이상 애니메이션 트리거나 업데이트 작업을 하지 않음
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        //Debug.Log("Player detected at position: " + player.position + ", distanceToPlayer: " + distanceToPlayer);

        // �÷��̾ ���� ���� ���� ������ ����
        if (distanceToPlayer <= detectionRange)
        {
            DirCheck();
            if (spriteRenderer.sprite != previousSprite)
            {
                UpdateCollider();
                previousSprite = spriteRenderer.sprite; // ���� ��������Ʈ�� ���� ��������Ʈ�� ����
            }

            // �÷��̾ ���� ���� �ȿ� ������ ����
            if (distanceToPlayer <= AttackRange)
            {
                TriggerAnimation(attackAnim);  // ���� �ִϸ��̼�
                Debug.Log(attackAnim);
            }
            else // ���� ���� ��, ���� ���� ��
            {
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                Move(directionToPlayer);

                if (!FirstAnimation) // ���� ó�� �ѹ��� ����
                {
                    if (FirstAnim != null)
                    {
                        TriggerAnimation(FirstAnim); // ó�� �����ϴ� �ִϸ��̼�
                    }
                    FirstAnimation = true;
                }
                TriggerAnimation(runAnim);
            }
        }
        else
        {
            TriggerAnimation(idleAnim);
            Debug.Log("idleAnim");
            // 플레이어가 탐지되지 않았을 때 순찰 동작 실행
            Patrol();
        }

    }
    protected virtual void Patrol()
    {
        RaycastHit2D wallHit = Physics2D.Raycast(transform.position, patrolDirection, patrolWallDistance, groundLayer);
        Debug.DrawLine(transform.position, transform.position + (Vector3)patrolDirection * patrolWallDistance, Color.red);

        // 발 밑에서 아래 방향으로 조금 앞을 감지하는 레이캐스트
        Vector2 groundCheckStart = (Vector2)transform.position + Vector2.down * 2f + (Vector2)patrolDirection * 1f;
        RaycastHit2D groundHit = Physics2D.Raycast(groundCheckStart, Vector2.down, groundCheckDistance, groundLayer);
        Debug.DrawLine(groundCheckStart, groundCheckStart + Vector2.down * groundCheckDistance, Color.blue);

        if (wallHit.collider != null || groundHit.collider == null)
        {
            patrolDirection = -patrolDirection; // 방향을 반대로 전환
            if (patrolDirection.x > 0)
            {
                spriteRenderer.flipX = false; // 오른쪽을 바라보도록
            }
            else
            {
                spriteRenderer.flipX = true; // 왼쪽을 바라보도록
            }
        }
        else
        {
            Move(patrolDirection);
            TriggerAnimation(runAnim);
        }
    }

    // Gizmos�� ���� ������ ���� ���� �׸���
    private void OnDrawGizmosSelected()
    {
        // Ž�� ������ �Ķ������� ǥ��
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // ���� ������ ���������� ǥ��
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }

    protected virtual void TriggerAnimation(string Animation)
    {
        anim.SetTrigger(Animation);
        // 공격 애니메이션 트리거 시 공격 소리를 한 번만 재생
        if (Animation == attackAnim && attackSound != null && audioSource != null)
        {
            if (!audioSource.isPlaying) // 현재 오디오가 재생 중인지 확인
            {
                audioSource.PlayOneShot(attackSound);
            }
        }
    }

    void UpdateCollider()
    {
        polygonCollider.pathCount = 0;

        // 현재 Sprite의 모양을 가져옵니다.
        List<Vector2> physicsShape = new List<Vector2>();
        spriteRenderer.sprite.GetPhysicsShape(0, physicsShape);

        // Sprite가 플립된 상태를 반영해 Collider 좌표 반전 적용
        for (int i = 0; i < physicsShape.Count; i++)
        {
            Vector2 point = physicsShape[i];

            if (spriteRenderer.flipX)
                point.x *= -1; // X축이 뒤집히도록 설정

            if (spriteRenderer.flipY)
                point.y *= -1; // Y축이 뒤집히도록 설정

            physicsShape[i] = point;
        }

        // Collider의 모양을 업데이트
        polygonCollider.SetPath(0, physicsShape.ToArray());

        // Collider의 Trigger 상태를 항상 유지
        polygonCollider.isTrigger = true;
    }

    void DirCheck()
    {
        if (player.position.x > transform.position.x)
        {
            // �÷��̾ �����ʿ� ���� �� (������ ����)
            if (FlipSprite)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            // �÷��̾ ���ʿ� ���� �� (���� ����)
            if (FlipSprite)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    // �⺻ �̵� �Լ�
    public virtual void Move(Vector3 direction)
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name);

        // 충돌한 오브젝트가 "AttackedRange"인지 확인
        if (collision.gameObject.name == "AttackedRange")
        {
            // 부모 오브젝트에서 PlayerController를 가져오기
            PlayerController playerController = collision.transform.parent.GetComponent<PlayerController>();

            if (playerController != null)
            {
                // 데미지 처리
                playerController.Damaged();
                Debug.Log("Player damaged by AttackedRange");
            }
            else
            {
                Debug.LogWarning("PlayerController not found on the parent object.");
            }
        }
    }

    public virtual void UpdateHPBar()
    {
        if (hpBarTransform != null)
        {
            float hpPercentage = hp / maxHp; // 현재 HP를 최대 HP로 나눈 비율

            if (hpPercentage < 0)
            {
                hpPercentage = 0;
            }
            hpBarTransform.localScale = new Vector3(hpPercentage, 1f, 1f); // X 스케일 조정
        }
    }

    public virtual void Damaged(float amount)
    {
        if (isInvincible) return; // 무적 상태일 때는 데미지를 받지 않음

        Debug.Log("Monster is taking damage: " + amount);
        anim.SetTrigger("hit"); // 히트 애니메이션 실행

        hp -= amount;

        UpdateHPBar();

        if (hp <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine(0.2f)); // 0.2초 동안 데미지 안받음 중복방지
        }

    }
    private IEnumerator InvincibilityCoroutine(float duration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }

    protected virtual void Die()
    {
        if (isDying) return; // 이미 죽음 상태라면 중복 호출 방지

        Debug.Log("Monster is dying");
        isDying = true; // 죽음 상태 설정
        anim.SetTrigger("death"); // 죽음 애니메이션 시작
        PlayerController player = Object.FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            player.ExpUp(experiencePoints); // 경험치 전달
        }
    }
}