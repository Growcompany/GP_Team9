using UnityEngine;
using UnityEngine.UI;

public class QuitButtonUI : MonoBehaviour
{
    Button button;
    AudioSource m_audioSource;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Quit);

        m_audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(Quit);
    }

    public void Quit()
    {
        Canvas canvas = transform.parent.parent.gameObject.GetComponent<Canvas>();
        canvas.enabled = !canvas.enabled;

        m_audioSource.Play();

        GameManager.instance.Pause(false);
    }
}
