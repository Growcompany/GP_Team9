using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayFrame : MonoBehaviour
{
    private float m_fps;
    private TMP_Text m_text;

    private void Start()
    {
        m_text = GetComponent<TMP_Text>();
        InvokeRepeating("GetFPS", 1, 1);
    }

    private void GetFPS()
    {
        m_fps = 1 / Time.unscaledDeltaTime;
        m_text.text = "FPS: " + m_fps.ToString("F0");
    }

    private void OnDestroy()
    {
        CancelInvoke();
    }
}