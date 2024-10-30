using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmButtonUI : MonoBehaviour
{
    Button button;

    [SerializeField] private List<CurrentStatusUI> currentStatusUIs;


    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(GameManager.instance.ConfirmPoints);
        button.onClick.AddListener(ConfirmCurrentStatus);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    private void ConfirmCurrentStatus()
    {
        for(int i = 0; i < currentStatusUIs.Count; i++)
        {
            currentStatusUIs[i].Confirm();
        }
    }
}
