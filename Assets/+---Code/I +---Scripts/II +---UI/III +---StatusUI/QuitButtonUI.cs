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
        button.onClick.AddListener(Quit);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(Quit);
    }

    void Quit()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
