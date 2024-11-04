using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolTimeUI : MonoBehaviour
{
    [SerializeField] Image grayFilledImage;
    [SerializeField] TMP_Text text;
    IEnumerator UpdateImage()
    {
        while (GameManager.instance.coolTimeRatio >= 0.0f)
        {
            grayFilledImage.fillAmount = GameManager.instance.coolTimeRatio;
            yield return null;
        }
    }

    IEnumerator UpdateText()
    {
        text.enabled = true;
        while (GameManager.instance.currentCoolTime >= 0.0f)
        {
            text.text = GameManager.instance.currentCoolTime.ToString("F1");
            yield return null;
        }
        text.enabled = false;
    }

    void UpdateCoolTimeUI()
    {
        grayFilledImage.fillAmount = 1.0f;
        StartCoroutine(UpdateImage());
        StartCoroutine(UpdateText());
    }

    private void Start()
    {
        grayFilledImage.fillAmount = 0;
        text.enabled = false;

        GameManager.instance.player.skillCoolTimeUIEvent.AddListener(UpdateCoolTimeUI);
    }

    private void OnDestroy()
    {
        GameManager.instance.player.skillCoolTimeUIEvent.RemoveAllListeners();
    }
}