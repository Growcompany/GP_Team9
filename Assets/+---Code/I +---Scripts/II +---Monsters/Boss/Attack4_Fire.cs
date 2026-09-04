using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack4_Fire : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Collided object name: " + collision.gameObject.name);
            // PlayerController 컴포넌트 가져오기
            PlayerController playerController = collision.GetComponent<PlayerController>();

            if (playerController != null)
            {
                // 플레이어에 데미지 주기
                playerController.StartCoroutine(playerController.Damaged(3));

                Debug.Log("Player damaged by MonsterSkill damage");
            }
            else
            {
                Debug.LogWarning("PlayerController not found on the collided object.");
            }
        }
    }
}

