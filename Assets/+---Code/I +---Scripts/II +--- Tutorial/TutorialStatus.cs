using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStatus : MonoBehaviour
{
    private Canvas canvas;
    private bool m_isActive = false;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    void Update()
    {
        if(GameManager.instance.Player.MovementStats.Level == 2 && !m_isActive)
        {
            m_isActive = true;

            canvas.enabled = true;
            GameManager.instance.Pause(true);
        }
    }
}
