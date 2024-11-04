using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PointTextUI : MonoBehaviour
{
    TMP_Text text;

    public UnityEvent onChanged;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        onChanged.AddListener(UpdateText);

        UpdateText();
    }

    private void OnDestroy()
    {
        onChanged.RemoveListener(UpdateText);
    }

    void UpdateText()
    {
        Debug.LogWarning(GameManager.instance.availablePoint);
        text.text = (GameManager.instance.availablePoint - GameManager.instance.currentUsedStatusPoint).ToString();
    }
}
