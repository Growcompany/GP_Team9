using UnityEngine;

public class DestructibleObstacle : MonoBehaviour
{
    private Animator animator;
    public float destroyDelay = 1f; // 애니메이션이 끝난 후 오브젝트가 파괴되는 지연 시간

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어의 공격 범위와 충돌 시 애니메이션 재생 후 파괴
        if (other.gameObject.name == "AttackArea") // 공격 범위 오브젝트 이름이 "AttackArea"인지 확인
        {
            PlayDestroyAnimation();
        }
    }

    private void PlayDestroyAnimation()
    {
        animator.SetTrigger("Disappear"); // Animator에서 "Disappear" 트리거를 설정하여 애니메이션 시작
        Destroy(gameObject, destroyDelay); // destroyDelay 후에 오브젝트 삭제
    }
}
