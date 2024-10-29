using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CurrentStatusUI : MonoBehaviour
{
    public PointTextUI pointTextUI;
    public UnityEvent<bool> onStatusChanged;

    [SerializeField] TMP_Text changesText;
    [SerializeField] TMP_Text currentStateText;

    int previousValue;
    int currentValue;
    int diff;

    // TODO: Ability 찍는거 제한 및 Save기능
    private void Awake()
    {
        if(changesText == null)
            changesText = transform.Find("Changes").GetComponent<TMP_Text>();

        if(currentStateText == null)
            currentStateText = GetComponent<TMP_Text>();

        if(!int.TryParse(currentStateText.text, out currentValue))
        {
            Debug.LogError("Failed Get currenValue to int");
        }

        Confirm();

        onStatusChanged.AddListener(StatusChange);
    }

    private void OnDestroy()
    {
        onStatusChanged.RemoveListener(StatusChange);
    }

    void StatusChange(bool selectedButton)
    {
        currentValue = selectedButton ? (currentValue + 1) : (currentValue - 1);

        // 기존보다 낮게 되면 값 원래대로 복구하고 return
        if(currentValue <= previousValue)
        {
            currentValue = previousValue;
            changesText.enabled = false;
        }
        else
        {
            diff = currentValue - previousValue;

            // Changes text 바꾸기
            if (!changesText.enabled && diff > 0)
                changesText.enabled = true;

            changesText.text = "(+" + diff.ToString() + ")";
        }

        // Current text 바꾸기
        currentStateText.text = currentValue.ToString();

        // PointTextUI 바꾸기(오른쪽 상단)
        pointTextUI.onChanged.Invoke();
    }

    public void Confirm()
    {
        previousValue = currentValue;
        changesText.enabled = false;
        diff = 0;
    }
}
