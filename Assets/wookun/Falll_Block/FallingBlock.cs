using System.Collections;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    public GameObject explosionEffectPrefab; // 파괴 이펙트 프리팹
    private Animator animator;
    private Rigidbody2D rb;
    private bool isFalling = false;
    private bool isDestroyed = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // 초기 상태를 Kinematic으로 설정하여 중력 영향을 받지 않음
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어가 감지 트리거에 들어오면 박스가 떨어지기 시작
        if (!isFalling && other.CompareTag("Player"))
        {
            isFalling = true;
            rb.bodyType = RigidbodyType2D.Dynamic; // 감지 후 Dynamic으로 변경하여 중력 적용
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 땅이나 플레이어와 충돌 시 파괴
        if (!isDestroyed && isFalling && (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Player")))
        {
            isDestroyed = true;
            TriggerDestruction();
        }
    }

    private void TriggerDestruction()
    {
        // 파괴 애니메이션 트리거 설정
        animator.SetTrigger("fall");

        // Rigidbody를 Kinematic으로 설정하여 중력 영향을 받지 않도록 함
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector2.zero; // 파괴 시 속도를 0으로 초기화하여 멈춤

        // 파괴 이펙트 생성 (선택 사항)
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // 파괴 애니메이션 완료 후 제거
        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        // 애니메이션의 실제 재생 시간 동안 대기
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // 오브젝트 제거
        Destroy(gameObject);
    }
}
