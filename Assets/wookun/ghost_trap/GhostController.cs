using System.Collections;
using System.Collections;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform을 연결합니다.
    public float followSpeed = 2.0f; // 고스트가 플레이어를 따라가는 속도
    public float floatAmplitude = 0.5f; // 고스트가 떠다니는 높이
    public float floatFrequency = 1.0f; // 고스트가 떠다니는 속도
    public GameObject explosionEffectPrefab; // 폭발 파티클 이펙트 프리팹
    private Vector3 initialPosition; // 초기 위치 저장
    private Animator anim;
    private bool hasExploded = false; // 폭발이 한 번만 실행되도록 설정하는 플래그

    void Start()
    {
        anim = GetComponent<Animator>();

        // 플레이어 오브젝트 찾기 (플레이어는 "Player" 태그가 있다고 가정)
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        initialPosition = transform.position; // 초기 위치를 저장
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

        // 폭발 애니메이션의 길이에 맞춰 일정 시간이 지난 후 오브젝트 제거 및 데미지 적용
        StartCoroutine(DestroyAfterExplosion());
    }

    private IEnumerator DestroyAfterExplosion()
    {
        yield return new WaitForSeconds(1.0f); // 폭발 애니메이션의 재생 시간에 맞춰 설정 (예: 1초)
        Destroy(gameObject); // 오브젝트 제거
    }
}
