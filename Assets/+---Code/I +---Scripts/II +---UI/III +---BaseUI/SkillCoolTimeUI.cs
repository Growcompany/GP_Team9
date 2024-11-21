using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolTimeUI : MonoBehaviour
{
    [SerializeField] Image grayFilledImage;
    [SerializeField] TMP_Text text;
    [SerializeField] PlayerController player;

    IEnumerator UpdateImage(int skillIndex)
    {
        CoolTimeCalculate coolTimeCalculate = player.GetComponent<CoolTimeCalculate>();
        while (coolTimeCalculate.coolTimeRatio[skillIndex] > 0.0f)
        {
            grayFilledImage.fillAmount = coolTimeCalculate.coolTimeRatio[skillIndex];
            yield return null;
        }
    }

    IEnumerator UpdateText(int skillIndex)
    {
        text.enabled = true;
        CoolTimeCalculate coolTimeCalculate = player.GetComponent<CoolTimeCalculate>();
        while (coolTimeCalculate.coolTimeRatio[skillIndex] > 0.0f)
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
        player = GameManager.instance.player;

        player.skillCoolTimeUIEvent.AddListener(UpdateCoolTimeUI);
    }

    private void OnDestroy()
    {
        player.skillCoolTimeUIEvent.RemoveAllListeners();
    }
}