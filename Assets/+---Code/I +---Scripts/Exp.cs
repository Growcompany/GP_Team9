using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Exp : MonoBehaviour
{
    PlayerController m_player;

    private void Start()
    {
        m_player = GameManager.instance.Player;

        transform.DOMove(m_player.transform.position, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Debug.Log("Complete");
        });
    }
}
