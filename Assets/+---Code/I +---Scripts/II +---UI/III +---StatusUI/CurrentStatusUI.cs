using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Loading;
using UnityEngine;
using UnityEngine.Events;

public class CurrentStatusUI : MonoBehaviour
{
    public PointTextUI pointTextUI;
    public UnityEvent<bool> onStatusChanged;

    [SerializeField] TMP_Text changesText;
    [SerializeField] TMP_Text currentStateText;

    public int currentValue;            // PlusMinusButtonUI에서 사용
    int previousValue;
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

    public void Load(StatusType type)
    {
        switch (type)
        {
            case StatusType.Life:               currentValue = GameManager.instance.player.MovementStats.MaxLife;        break;
            case StatusType.Strength:           currentValue = GameManager.instance.player.MovementStats.Strength;       break;
            case StatusType.Dodge:              currentValue = GameManager.instance.player.MovementStats.Dodge;          break;
            case StatusType.SkillCoolTime:      currentValue = GameManager.instance.player.MovementStats.SkillCoolTime;  break;
        }

        currentStateText.text = currentValue.ToString();
        previousValue = currentValue;
    }

    // 내부 초기화용
    private void Confirm()
    {
        previousValue = currentValue;
        changesText.enabled = false;
        diff = 0;
    }

    public void Confirm(StatusType type)
    {
        switch(type)
        {
            case StatusType.Life:           GameManager.instance.player.MovementStats.MaxLife = currentValue;       break;
            case StatusType.Strength:       GameManager.instance.player.MovementStats.Strength = currentValue;      break;
            case StatusType.Dodge:          GameManager.instance.player.MovementStats.Dodge = currentValue;         break;
            case StatusType.SkillCoolTime:  GameManager.instance.player.MovementStats.SkillCoolTime = currentValue; break;
        }

        Confirm();
    }
}
