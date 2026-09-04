using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpManager : MonoBehaviour
{
    public int Exp = 5;
    private SpriteRenderer spriteRenderer;
    protected Transform player_pos;
    public float MoveSpeed = 30.0f;
    public float CollisionDistance = 1.0f; // 충돌로 간주할 거리
    private bool canMove = false; // 이동 및 충돌 활성화 여부

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player_pos = playerObject.transform; // Transform 참조 저장
        }

        // 이동 및 충돌 활성화
        StartCoroutine(EnableMoveAfterDelay(1f));
    }

    // 이동 및 충돌 활성화 코루틴
    private IEnumerator EnableMoveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canMove = true; // 이동 및 충돌 활성화
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // R 값을 0 ~ 1로 반복
        float rValue = Mathf.PingPong(Time.time, 1f);

        Color color = spriteRenderer.color;
        color.r = rValue; // R 값 변경
        spriteRenderer.color = color;

        if (!canMove) return;

        // 플레이어 방향으로 이동
        if (player_pos != null)
        {
            Vector3 directionToPlayer = (player_pos.position - transform.position).normalized;
            Move(directionToPlayer);

            // 일정 거리 이내면 충돌 처리
            if (Vector3.Distance(transform.position, player_pos.position) <= CollisionDistance)
            {
                HandleCollision();
            }
        }
    }
    public void Move(Vector3 direction)
    {
        transform.Translate(direction * MoveSpeed * Time.deltaTime);
    }
    private void HandleCollision()
    {
        PlayerController player = Object.FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            player.ExpUp(Exp); // 경험치 전달
            //Debug.Log("경험치주기");
        }

        Destroy(gameObject); // ExpManager 오브젝트 제거
    }
}
