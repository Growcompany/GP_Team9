using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegameButtonUI : MonoBehaviour
{
    Button button;
    bool m_isClicked = false;

    private void Awake()
    {
        button = GetComponent<Button>();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Regame);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    void Regame()
    {
        if(!m_isClicked)
        {
            SceneTransition.Instance.LoadScene("SampleScene Mobile");
            m_isClicked = true;
        }
    }
}
