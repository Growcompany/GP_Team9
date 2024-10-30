using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelUI : MonoBehaviour
{
    [SerializeField] TMP_Text levelText;
    [SerializeField] Image filledImage;
    private void Start()
    {
        if(levelText == null)
            levelText = transform.Find("LevelText").GetComponent<TMP_Text>();

        if(filledImage == null)
            filledImage = transform.Find("LevelFilled").GetComponent<Image>();

        levelText.text = GameManager.instance.player.MovementStats.Level.ToString();
        filledImage.fillAmount = GameManager.instance.player.MovementStats.Exp / 100.0f;    // 100은 임의 설정

        GameManager.instance.player.levelUpUIEvent.AddListener(UpdateLevelTextUI);
        GameManager.instance.player.expUpUIEvent.AddListener(UpdateExpUI);
    }

    private void OnDestroy()
    {
        GameManager.instance.player.levelUpUIEvent.RemoveAllListeners();
        GameManager.instance.player.expUpUIEvent.RemoveAllListeners();
    }

    // TODO: Player에서
    // UnityEvent<int> levelUpUIEvent 추가 필요
    // 이후 Level up하면 levelUpUIEvent.Invoke(level: int) 추가 필요
    void UpdateLevelTextUI(int level)
    {
        levelText.text = level.ToString();
        GameManager.instance.CalculateAvailableStatusPoint();
    }

    // TODO: Player에서
    // UnityEvent<int, int> expUpUIEvent 추가 필요
    // 이후 Level up하면 expUpUIEvent.Invoke(totalExp: int, currentExp: int) 추가 필요
    void UpdateExpUI(int totalExp, int currentExp)
    {
        float amount = (float)currentExp / totalExp;
        filledImage.fillAmount = amount;
    }
}
