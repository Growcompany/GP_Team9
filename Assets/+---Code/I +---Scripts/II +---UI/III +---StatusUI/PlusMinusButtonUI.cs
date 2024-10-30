using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlusMinusButtonUI : MonoBehaviour
{
    [SerializeField] Button minus;
    [SerializeField] Button plus;

    public CurrentStatusUI currentStatus;
    private void Awake()
    {
        if(minus == null)
            minus = transform.Find("Minus").GetComponent<Button>();

        if(plus == null)
            plus = transform.Find("Plus").GetComponent<Button>();

        if(currentStatus == null)
            currentStatus = transform.parent.Find("CurrentState").gameObject.GetComponent<CurrentStatusUI>();

        minus.onClick.AddListener(Minus);
        plus.onClick.AddListener(Plus);
    }

    private void OnDestroy()
    {
        minus.onClick.RemoveListener(Minus);
        plus.onClick.RemoveListener(Plus);
    }

    void Minus()
    {
        if (GameManager.instance.currentUsedStatusPoint > 0)
        {
            GameManager.instance.currentUsedStatusPoint--;
            currentStatus.onStatusChanged.Invoke(false);
        }
    }

    void Plus()
    {
        if (GameManager.instance.availablePoint > GameManager.instance.currentUsedStatusPoint)
        {
            GameManager.instance.currentUsedStatusPoint++;
            currentStatus.onStatusChanged.Invoke(true);
        }
    }
}
