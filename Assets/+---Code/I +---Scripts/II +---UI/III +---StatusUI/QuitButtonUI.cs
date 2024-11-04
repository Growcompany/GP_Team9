using UnityEngine;
using UnityEngine.UI;

public class QuitButtonUI : MonoBehaviour
{
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Quit);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(Quit);
    }

    public void Quit()
    {
        Canvas canvas = transform.parent.parent.gameObject.GetComponent<Canvas>();
        canvas.enabled = !canvas.enabled;
    }
}
