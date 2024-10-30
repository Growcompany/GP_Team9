using UnityEngine;

public class BreakableBox : MonoBehaviour
{
    public GameObject monsterPrefab;     // 몬스터 프리팹
    public Transform spawnPoint;         // 몬스터가 생성될 위치
    public int health = 3;               // 박스의 체력
    public int monsterCount = 1;         // 생성할 몬스터 수
    public float destroyDelay = 1f;      // 박스 파괴 후 삭제 딜레이

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어의 공격 범위와 충돌 시
        if (other.gameObject.name == "AttackArea") // 공격 범위 오브젝트 이름이 "AttackArea"인지 확인
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        health--; // 체력 감소

        if (health <= 0)
        {
            BreakBox();
        }
    }

    private void BreakBox()
    {
        // 박스 깨지는 애니메이션 재생
        if (animator != null)
        {
            animator.SetTrigger("Break"); // "Break" 트리거를 Animator에 설정
        }

        // 박스가 파괴된 후 몬스터 생성
        Invoke("SpawnMonsters", destroyDelay);

        // 일정 시간이 지난 후 박스 오브젝트 삭제
        Destroy(gameObject, destroyDelay);
    }

    private void SpawnMonsters()
    {
        for (int i = 0; i < monsterCount; i++)
        {
            // 몬스터를 설정된 수만큼 생성
            Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : transform.position;
            Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
