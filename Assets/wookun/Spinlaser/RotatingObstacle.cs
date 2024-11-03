using UnityEngine;

public class RotatingObstacle : MonoBehaviour
{
    public Transform centerPoint; // 회전 중심점
    public float rotationSpeed = 50f; // 회전 속도
    public float knockbackForce = 5f; // 플레이어를 밀어내는 힘
    public float damageAmount = 1.0f; // 플레이어에게 줄 데미지 양

    void Update()
    {
        if (centerPoint != null)
        {
            // 중심점을 기준으로 회전
            transform.RotateAround(centerPoint.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어와 충돌했는지 확인
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player collided with Rotating Obstacle"); // 충돌 확인 로그 출력

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
}
