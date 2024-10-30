using System.Collections;
using UnityEngine;

public class BoxDestruction : MonoBehaviour
{
    private Animator animator; // 애니메이터 컴포넌트
    private Collider2D boxCollider; // 상자 콜라이더
    private bool isBreaking = false; // 파괴 상태 확인

    void Start()
    {
        // 애니메이터와 콜라이더 컴포넌트 가져오기
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어와 충돌했는지 확인
        if (collision.gameObject.CompareTag("Player") && !isBreaking)
        {
            BreakBox(); // 상자 파괴 시작
        }
    }

    private void BreakBox()
    {
        isBreaking = true; // 파괴 상태로 설정
        animator.SetTrigger("Break"); // 애니메이션 트리거 발동
        boxCollider.enabled = false; // 상자 충돌 비활성화
        StartCoroutine(DestroyAfterAnimation()); // 애니메이션 종료 후 상자 제거 코루틴 실행
    }

    private IEnumerator DestroyAfterAnimation()
    {
        // 애니메이션 재생이 끝날 때까지 대기
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject); // 상자 오브젝트 삭제
    }
}
