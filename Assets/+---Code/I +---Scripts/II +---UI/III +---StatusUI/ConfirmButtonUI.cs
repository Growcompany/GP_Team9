using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StatusType
{
    Life,
    Strength,
    Dodge,
    SkillCoolTime
}

public class ConfirmButtonUI : MonoBehaviour
{
    Button button;

    [SerializeField] private List<CurrentStatusUI> currentStatusUIs;
    StatusType[] statusTypes = (StatusType[])Enum.GetValues(typeof(StatusType));

    AudioSource m_audioSource;

    private void Start()
    {

        m_audioSource = GetComponent<AudioSource>();

        button = GetComponent<Button>();
        button.onClick.AddListener(GameManager.instance.ConfirmPoints);
        button.onClick.AddListener(ConfirmCurrentStatus);

        LoadCurrentStatus();
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    private void LoadCurrentStatus()
    {
        for (int i = 0; i < currentStatusUIs.Count; i++)
        {
            currentStatusUIs[i].Load(statusTypes[i]);
        }
    }

    private void ConfirmCurrentStatus()
    {
        for(int i = 0; i < currentStatusUIs.Count; i++)
        {
            currentStatusUIs[i].Confirm(statusTypes[i]);
        }

        m_audioSource.Play();
    }
}
