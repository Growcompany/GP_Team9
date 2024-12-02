using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusOpenButton : MonoBehaviour
{
    public GameObject statusUI;
    private Button m_button;

    private void Awake()
    {
        m_button = GetComponent<Button>();
        m_button.onClick.RemoveAllListeners();
        m_button.onClick.AddListener(OpenStatusUI);
    }

    private void OnDestroy()
    {
        m_button.onClick.RemoveAllListeners();
    }

    private void OpenStatusUI()
    {
        statusUI.GetComponent<Canvas>().enabled = true;
    }
}
