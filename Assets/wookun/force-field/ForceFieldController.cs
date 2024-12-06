using System.Collections;
using UnityEngine;

public class ForceFieldController : MonoBehaviour
{
    public int damageAmount = 1; // 벽에 닿았을 때 플레이어에게 줄 데미지
    public float knockbackForce = 5.0f; // 플레이어가 밀려나는 힘
    public float onDuration = 2.0f; // 전기 오브젝트가 켜진 상태로 유지되는 시간
    public float offDuration = 2.0f; // 전기 오브젝트가 꺼진 상태로 유지되는 시간
    public AudioClip proximitySound; // 플레이어가 가까이 접근할 때 재생할 소리
    public float triggerDistance = 20.0f; // 사운드가 재생되는 거리

    private bool isActive = true; // 전기 오브젝트의 현재 활성화 상태
    private SpriteRenderer spriteRenderer;
    private Collider2D collider2D;
    private Transform player; // 플레이어의 Transform
    private bool hasTriggeredSound = false; // 소리가 이미 재생되었는지 확인

    private void Start()
    {
        // SpriteRenderer와 Collider2D 컴포넌트 가져오기
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();

        // "Player" 태그를 가진 오브젝트를 찾아 Transform 할당
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player object with tag 'Player' not found in the scene.");
        }

        // 전기 오브젝트의 켜짐/꺼짐 상태를 주기적으로 변경하는 코루틴 시작
        StartCoroutine(ToggleElectricity());
    }

    private void Update()
    {
        // 플레이어와의 거리 계산
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            // 플레이어가 일정 거리 이내로 접근할 때만 사운드를 한 번만 재생
            if (distance <= triggerDistance && !hasTriggeredSound)
            {
                AudioManager.PlayProximitySound(proximitySound); // AudioManager를 통해 소리 재생
                hasTriggeredSound = true; // 소리 재생 여부를 기록
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isActive && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player collided with Force Field");

            // PlayerController 컴포넌트에서 Damaged 메서드를 호출하여 데미지를 줌
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // 데미지 코루틴 호출
                playerController.StartCoroutine(playerController.Damaged(damageAmount));

                // 플레이어를 뒤로 밀어내는 처리
                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                    playerRb.velocity = Vector2.zero; // 이전 속도 초기화
                    playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

                    Debug.Log("Knockback applied: " + knockbackDirection * knockbackForce);
                }
            }
        }
    }

    private IEnumerator ToggleElectricity()
    {
        while (true)
        {
            isActive = true;
            spriteRenderer.enabled = true;
            collider2D.enabled = true;
            // Debug.Log("Electricity ON");
            yield return new WaitForSeconds(onDuration);

            isActive = false;
            spriteRenderer.enabled = false;
            collider2D.enabled = false;
            // Debug.Log("Electricity OFF");
            yield return new WaitForSeconds(offDuration);
        }
    }
}
