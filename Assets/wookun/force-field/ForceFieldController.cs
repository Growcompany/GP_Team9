using System.Collections;
using UnityEngine;

public class ForceFieldController : MonoBehaviour
{
    public float damageAmount = 1.0f; // 벽에 닿았을 때 플레이어에게 줄 데미지
    public float knockbackForce = 5.0f; // 플레이어가 밀려나는 힘
    public float onDuration = 2.0f; // 전기 오브젝트가 켜진 상태로 유지되는 시간
    public float offDuration = 2.0f; // 전기 오브젝트가 꺼진 상태로 유지되는 시간
    public float soundTriggerDistance = 5.0f; // 경고음이 재생될 거리
    public AudioClip warningSound; // 경고음 오디오 클립

    private bool isActive = true; // 전기 오브젝트의 현재 활성화 상태
    private SpriteRenderer spriteRenderer;
    private Collider2D collider2D;
    private AudioSource audioSource;
    private GameObject player; // 플레이어 오브젝트

    private void Start()
    {
        // SpriteRenderer와 Collider2D 컴포넌트 가져오기
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();

        // AudioSource 컴포넌트 추가 및 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = warningSound;
        audioSource.loop = false;
        audioSource.playOnAwake = false;

        // 플레이어 오브젝트 찾기
        player = GameObject.FindGameObjectWithTag("Player");

        // 전기 오브젝트의 켜짐/꺼짐 상태를 주기적으로 변경하는 코루틴 시작
        StartCoroutine(ToggleElectricity());
    }

    private void Update()
    {
        if (player != null)
        {
            // 장애물과 플레이어 간의 거리 계산
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            // 플레이어가 지정된 거리 이내로 접근했을 때
            if (distanceToPlayer <= soundTriggerDistance)
            {
                // 경고음이 재생 중이 아닐 경우 재생
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                    Debug.Log("Warning sound played.");
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 전기 오브젝트가 활성 상태일 때만 충돌 처리
        if (isActive && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player collided with Force Field"); // 충돌 확인 로그 출력

            // PlayerController 컴포넌트에서 Damaged 메서드를 호출하여 데미지를 줌
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // 데미지 처리
                playerController.Damaged();

                // 플레이어를 뒤로 밀어내는 처리
                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    // 밀려나는 방향을 계산하여 밀어내기
                    Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                    playerRb.velocity = Vector2.zero; // 이전 속도 초기화
                    playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

                    Debug.Log("Knockback applied: " + knockbackDirection * knockbackForce); // 밀려나는 방향 및 힘 로그 출력
                }
            }
        }
    }

    // 전기 오브젝트의 켜짐/꺼짐 상태를 주기적으로 변경하는 코루틴
    private IEnumerator ToggleElectricity()
    {
        while (true)
        {
            // 활성화 상태 설정
            isActive = true;
            spriteRenderer.enabled = true; // 전기 오브젝트의 모습 활성화
            collider2D.enabled = true; // 전기 오브젝트 충돌 활성화
            Debug.Log("Electricity ON");
            yield return new WaitForSeconds(onDuration);

            // 비활성화 상태 설정
            isActive = false;
            spriteRenderer.enabled = false; // 전기 오브젝트의 모습 비활성화
            collider2D.enabled = false; // 전기 오브젝트 충돌 비활성화
            Debug.Log("Electricity OFF");
            yield return new WaitForSeconds(offDuration);
        }
    }
}
