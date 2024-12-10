using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
    Button button;
    bool m_isClicked = false;
    AudioSource m_audioSource;

    private void Awake()
    {
        m_isClicked = false;

        button = GetComponent<Button>();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Resume);

        m_audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    void Resume()
    {
        if (!m_isClicked)
        {
            m_isClicked = true;
            GameManager.instance.Pause(false);
            transform.parent.parent.GetComponent<Canvas>().enabled = false;
            m_audioSource.Play();
        }

        m_isClicked = false;
    }
}
