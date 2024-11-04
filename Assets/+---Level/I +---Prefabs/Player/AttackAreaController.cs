using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaController : MonoBehaviour
{
    public PlayerController playerController;
    public PlayerMovementStats MovementStats;
    private bool hasAttacked = false; // ���� ���� ���� �÷���

    // Start is called before the first frame update
    void Start()
    {
        playerController = Object.FindFirstObjectByType<PlayerController>();
        MovementStats = playerController.MovementStats;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
            return;

        else if (collider.name == "Ground")
        {
            // Hit the ground
        }

        else if (collider.tag == "Monster")
        {
            // ���� MonsterController�� �õ��Ͽ� ã��
            MonsterController monster = collider.GetComponent<MonsterController>();

            if (monster != null)
            {
                monster.Damaged(MovementStats.AttackDamage);
                Debug.Log("Monster damaged by AttackArea with damage: " + MovementStats.AttackDamage);
                hasAttacked = true;
            }
            else
            {
                // MonsterController�� ������ BossController �õ�
                BossController boss = collider.GetComponent<BossController>();
                if (boss != null)
                {
                    boss.Damaged(MovementStats.AttackDamage);
                    Debug.Log("Boss damaged by AttackArea with damage: " + MovementStats.AttackDamage);
                    hasAttacked = true;
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Ʈ���Ÿ� ����� �ٽ� ������ �� �ֵ��� �÷��� �ʱ�ȭ
        if (collision.GetComponent<MonsterController>() != null)
        {
            hasAttacked = false;
        }
    }

}
