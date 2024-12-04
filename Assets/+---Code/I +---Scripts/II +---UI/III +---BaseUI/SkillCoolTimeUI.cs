using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolTimeUI : MonoBehaviour
{
    [SerializeField] Image grayFilledImage;
    [SerializeField] TMP_Text text;
    [SerializeField] PlayerController player;
    public int skillIndex;

    IEnumerator UpdateImage(int skillIndex_input)
    {
        CoolTimeCalculate coolTimeCalculate = player.GetComponent<CoolTimeCalculate>();
        while (coolTimeCalculate.coolTimeRatio[skillIndex] > 0.0f && skillIndex == skillIndex_input)
        {
            grayFilledImage.fillAmount = coolTimeCalculate.coolTimeRatio[skillIndex];
            yield return null;
        }

        grayFilledImage.fillAmount = 0; // 이미지가 보이지 않게 하기 위함
    }

    IEnumerator UpdateText(int skillIndex_input)
    {
        text.enabled = true;
        CoolTimeCalculate coolTimeCalculate = player.GetComponent<CoolTimeCalculate>();
        while (coolTimeCalculate.coolTimeRatio[skillIndex] > 0.0f && skillIndex == skillIndex_input)
        {
            text.text = coolTimeCalculate.remainTime[skillIndex].ToString("F1");
            yield return null;
        }
        text.enabled = false;
    }

    void UpdateCoolTimeUI(int skillIndex)
    {
        grayFilledImage.fillAmount = 1.0f;
        StartCoroutine(UpdateImage(skillIndex));
        StartCoroutine(UpdateText(skillIndex));
    }

    private void Start()
    {
        grayFilledImage.fillAmount = 0;
        text.enabled = false;
        player = GameManager.instance.Player;

        player.skillCoolTimeUIEvent.AddListener(UpdateCoolTimeUI);
    }

    private void OnDestroy()
    {
        player.skillCoolTimeUIEvent.RemoveAllListeners();
    }
}