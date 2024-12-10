using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegameButtonUI : MonoBehaviour
{
    Button button;
    bool m_isClicked = false;
    AudioSource m_audioSource;

    private void Awake()
    {
        m_isClicked = false;

        button = GetComponent<Button>();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Regame);

        m_audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    void Regame()
    {
        if(!m_isClicked)
        {
            SceneTransition.Instance.LoadScene("MainMenu");
            m_isClicked = true;
            m_audioSource.Play();
        }
    }
}
