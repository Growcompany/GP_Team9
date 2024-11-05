using System.Collections;
using UnityEngine;

public class BreakableBox : MonoBehaviour
{
    public int health = 3;               // 박스의 체력 (3번 공격당하면 파괴)
    public GameObject monsterPrefab;     // 몬스터 프리팹 (파괴될 때 생성할 경우 사용)
    public Transform spawnPoint;         // 몬스터가 생성될 위치
    public float destroyDelay = 1f;      // 박스 파괴 후 삭제 딜레이

    private Animator animator;
    private bool isDestroyed = false;    // 박스가 이미 파괴되었는지 확인

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어의 공격 범위와 충돌 시
        if (other.CompareTag("PlayerAttack") && !isDestroyed) // "PlayerAttack" 태그 확인
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        health--; // 체력 감소
        TriggerAnimation("break"); // 공격 애니메이션 실행 (필요한 경우)

        if (health <= 0 && !isDestroyed)
        {
            BreakBox();
        }
    }

    private void BreakBox()
    {
        isDestroyed = true; // 파괴 상태로 설정

        // 박스 깨지는 애니메이션 재생
        if (animator != null)
        {
            animator.SetTrigger("Break"); // "Break" 트리거를 Animator에 설정
        }

        // 박스가 파괴된 후 몬스터 생성 (필요한 경우)
        if (monsterPrefab != null)
        {
            SpawnMonsters();
        }

        // 일정 시간이 지난 후 박스 오브젝트 삭제
        Destroy(gameObject, destroyDelay);
    }

    private void SpawnMonsters()
    {
        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : transform.position;
        Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
    }

    private void TriggerAnimation(string animation)
    {
        if (animator != null)
        {
            animator.SetTrigger(animation);
        }
    }
}
