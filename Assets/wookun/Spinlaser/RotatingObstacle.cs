using UnityEngine;

public class RotatingObstacle : MonoBehaviour
{
    public Transform centerPoint; // 회전 중심점
    public float rotationSpeed = 50f; // 회전 속도
    public float knockbackForce = 5f; // 플레이어를 밀어내는 힘
    public float damageAmount = 1.0f; // 플레이어에게 줄 데미지 양
    public AudioClip proximitySound; // 플레이어가 가까워질 때 재생될 사운드 클립
    public float triggerDistance = 5f; // 사운드가 재생되는 거리
    private AudioSource audioSource; // AudioSource 컴포넌트
    private Transform player; // 플레이어의 Transform

    void Start()
    {
        // AudioSource 컴포넌트를 추가하고 초기 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = proximitySound;
        audioSource.playOnAwake = false; // 자동 재생 비활성화
        audioSource.loop = true; // 사운드 반복 재생

        // 플레이어의 Transform을 찾기
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

    void Update()
    {
        // 장애물 회전 처리
        if (centerPoint != null)
        {
            transform.RotateAround(centerPoint.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        }

        // 플레이어와의 거리 계산
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            // 거리가 triggerDistance 이내일 때 사운드 재생, 그렇지 않으면 정지
            if (distance <= triggerDistance && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
            else if (distance > triggerDistance && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어와 충돌했을 때 밀어내기와 데미지 처리
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // 데미지 코루틴 호출
                playerController.StartCoroutine(playerController.Damaged());

                // 밀려나는 방향을 계산하여 밀어내기
                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                    playerRb.velocity = Vector2.zero;
                    playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }
    }
}

