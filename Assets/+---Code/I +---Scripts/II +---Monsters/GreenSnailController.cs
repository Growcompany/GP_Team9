using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenSnailController : MonsterController
{
    protected override void Awake()
    {
        base.Awake(); // �θ� Ŭ������ Awake �޼��� ȣ��
        experiencePoints = 15; // GreenSnail�� ����ġ
        FlipSprite = false;
        attackAnim = "attack"; // GreenSnail�� ���� ����
        idleAnim = "idle";
        runAnim = "run";
    }

    protected override void Patrol()
    {
        // �� Ž�� ����ĳ��Ʈ
        RaycastHit2D wallHit = Physics2D.Raycast(transform.position, patrolDirection, patrolWallDistance, groundLayer);
        Debug.DrawLine(transform.position, transform.position + (Vector3)patrolDirection * patrolWallDistance, Color.green);

        // GreenSnail���� �� üũ ������ Ŀ���͸���¡
        Vector2 groundCheckStart = (Vector2)transform.position + Vector2.down * 0.8f + (Vector2)patrolDirection * 3f;
        RaycastHit2D groundHit = Physics2D.Raycast(groundCheckStart, Vector2.down, groundCheckDistance, groundLayer);
        Debug.DrawLine(groundCheckStart, groundCheckStart + Vector2.down * groundCheckDistance, Color.yellow);

        if (wallHit.collider != null || groundHit.collider == null)
        {
            patrolDirection = -patrolDirection; // ������ �ݴ�� ��ȯ
            spriteRenderer.flipX = patrolDirection.x < 0; // �ø� ���� ����
        }
        else
        {
            Move(patrolDirection);
            TriggerAnimation(runAnim);
        }
    }

}
