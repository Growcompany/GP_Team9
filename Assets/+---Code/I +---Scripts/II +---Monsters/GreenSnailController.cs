using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenSnailController : MonsterController
{
    protected override void Awake()
    {
        base.Awake(); // 부모 클래스의 Awake 메서드 호출
        experiencePoints = 50; // GreenSnail의 경험치
        FlipSprite = false;
        attackAnim = "attack"; // GreenSnail의 공격 설정
        idleAnim = "idle";
        runAnim = "run";
    }

    protected override void Patrol()
    {
        // 벽 탐지 레이캐스트
        RaycastHit2D wallHit = Physics2D.Raycast(transform.position, patrolDirection, patrolWallDistance, groundLayer);
        Debug.DrawLine(transform.position, transform.position + (Vector3)patrolDirection * patrolWallDistance, Color.green);

        // GreenSnail에서 땅 체크 로직을 커스터마이징
        Vector2 groundCheckStart = (Vector2)transform.position + Vector2.down * 0.8f + (Vector2)patrolDirection * 3f;
        RaycastHit2D groundHit = Physics2D.Raycast(groundCheckStart, Vector2.down, groundCheckDistance, groundLayer);
        Debug.DrawLine(groundCheckStart, groundCheckStart + Vector2.down * groundCheckDistance, Color.yellow);

        if (wallHit.collider != null || groundHit.collider == null)
        {
            patrolDirection = -patrolDirection; // 방향을 반대로 전환
            spriteRenderer.flipX = patrolDirection.x < 0; // 플립 방향 설정
        }
        else
        {
            Move(patrolDirection);
            TriggerAnimation(runAnim);
        }
    }

}
