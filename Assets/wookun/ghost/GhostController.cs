using System.Collections;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform을 연결합니다.
    public float followSpeed = 2.0f; // 고스트가 플레이어를 따라가는 속도
    public float floatAmplitude = 0.5f; // 고스트가 떠다니는 높이
    public float floatFrequency = 1.0f; // 고스트가 떠다니는 속도
    public GameObject explosionEffectPrefab; // 폭발 파티클 이펙트 프리팹
    public float explosionRadius = 2.0f; // 폭발 범위 (Collider의 반지름)
    private Vector3 initialPosition; // 초기 위치 저장
    private Animator anim;
    private bool hasExploded = false; // 폭발이 한 번만 실행되도록 설정하는 플래그
    private CircleCollider2D explosionCollider; // 폭발에 사용될 원형 콜라이더

    void Start()
    {
        anim = GetComponent<Animator>();

        // 씬에서 "Player" 태그를 가진 오브젝트를 찾아 플레이어의 Transform을 할당
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                Debug.LogWarning("Player object with tag 'Player' not found in the scene.");
            }
        }

        initialPosition = transform.position; // 초기 위치를 저장

        // CircleCollider2D 컴포넌트를 추가하고 초기에는 비활성화
        explosionCollider = gameObject.AddComponent<CircleCollider2D>();
        explosionCollider.isTrigger = false; // 물리적 충돌을 위해 Trigger 비활성화
        explosionCollider.radius = explosionRadius;
        explosionCollider.enabled = false; // 초기에는 비활성화
    }

    void Update()
    {
        // 플레이어를 따라다니기 (폭발 후에는 멈춤)
        if (player != null && !hasExploded)
        {
            transform.position = Vector3.Lerp(transform.position, player.position, followSpeed * Time.deltaTime);
        }

        // 고스트가 둥둥 떠다니는 효과 (폭발 후에는 멈춤)
        if (!hasExploded)
        {
            float floatOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
            transform.position += new Vector3(0, floatOffset * Time.deltaTime, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어와 충돌했을 때 폭발 애니메이션 실행 (데미지는 폭발 후에 줄 예정)
        if (other.CompareTag("Player") && !hasExploded)
        {
            anim.SetTrigger("isScared"); // 놀라는 애니메이션 실행
            Invoke("StartExplosion", 0.5f); // 0.5초 후에 폭발 애니메이션 실행
        }
    }

    private void StartExplosion()
    {
        anim.SetTrigger("isExploding"); // 폭발 애니메이션 실행
        anim.ResetTrigger("isScared"); // 놀라는 모션 해제

        // 폭발이 한 번만 실행되도록 설정
        hasExploded = true;

        // 폭발 파티클 이펙트 생성
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // 폭발 콜라이더 활성화 및 일정 시간 후 비활성화
        explosionCollider.enabled = true;
        StartCoroutine(DisableExplosionCollider());

        // 폭발 애니메이션의 길이에 맞춰 일정 시간이 지난 후 오브젝트 제거
        StartCoroutine(DestroyAfterExplosion());
    }

    private IEnumerator DisableExplosionCollider()
    {
        yield return new WaitForSeconds(0.1f); // 폭발 콜라이더가 활성화될 시간 설정 (0.1초)
        explosionCollider.enabled = false; // 콜라이더 비활성화
    }

    private IEnumerator DestroyAfterExplosion()
    {
        yield return new WaitForSeconds(1.0f); // 폭발 애니메이션의 재생 시간에 맞춰 설정 (예: 1초)
        Destroy(gameObject); // 오브젝트 제거
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 폭발 콜라이더가 활성화된 상태에서만 충돌 처리
        if (collision.collider.CompareTag("Player") && explosionCollider.enabled)
        {
            // 플레이어를 반대 방향으로 밀어내기
            Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
            collision.rigidbody.velocity = pushDirection * 3f; // 강한 반대 방향의 속도 적용
        }
    }
}
