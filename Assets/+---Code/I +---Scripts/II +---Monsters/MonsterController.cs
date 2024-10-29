using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField]
    public float hp;
    [SerializeField]
    public float speed;
    [SerializeField]
    public float AttackRange;
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
    public int experiencePoints; // 각 몬스터가 줄 경험치

    protected virtual void Awake()
    {
        polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // ��������Ʈ ������ ��������
    }

    protected virtual void Start()
    {
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

        DirCheck(); // �÷��̾��� ��ġ�� ���� ��������Ʈ ������

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        //Debug.Log("Player detected at position: " + player.position + ", distanceToPlayer: " + distanceToPlayer);

        // �÷��̾ ���� ���� ���� ������ ����
        if (distanceToPlayer <= detectionRange)
        {
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
        }

        if (spriteRenderer.sprite != previousSprite)
        {
            UpdateCollider();
            previousSprite = spriteRenderer.sprite; // ���� ��������Ʈ�� ���� ��������Ʈ�� ����
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
        anim.SetTrigger(Animation); // ������ ����� �ִϸ��̼� �̸��� ���
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

    public virtual void Damaged(float amount)
    {
        Debug.Log("Monster is dying Damaged: "+amount);
        anim.SetTrigger("hit");
        hp -= amount;
        if (hp <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (isDying) return; // 이미 죽음 상태라면 중복 호출 방지

        Debug.Log("Monster is dying");
        isDying = true; // 죽음 상태 설정
        anim.SetTrigger("death"); // 죽음 애니메이션 시작
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.ExpUp(experiencePoints); // 경험치 전달
        }
    }
}