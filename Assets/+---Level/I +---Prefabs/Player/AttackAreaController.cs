using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaController : MonoBehaviour
{
    public PlayerController playerController;
    public PlayerMovementStats MovementStats;
    private bool hasAttacked = false; // 공격 실행 여부 플래그

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
            MonsterController monster = collider.GetComponent<MonsterController>();
            monster.Damaged(MovementStats.AttackDamage);
            Debug.Log("Monster damaged by AttackArea with damage: " + MovementStats.AttackDamage);
            // 공격이 한 번 발생했음을 표시
            hasAttacked = true;
            //MonsterController monsterController = collider.gameObject.GetComponent<MonsterController>();
            //monsterController.Damaged(MovementStats.AttackDamage);
            // collider.gameObject.BroadcastMessage("Damaged", MovementStats.AttackDamage);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 트리거를 벗어나면 다시 공격할 수 있도록 플래그 초기화
        if (collision.GetComponent<MonsterController>() != null)
        {
            hasAttacked = false;
        }
    }

}
