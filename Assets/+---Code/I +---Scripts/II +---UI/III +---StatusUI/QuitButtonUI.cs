using System.Collections;
using System.Collections.Generic;
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
        transform.parent.gameObject.SetActive(false);
    }
}
