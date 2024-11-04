using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipButtonUI : MonoBehaviour
{
    public Canvas toolTipCanvas;

    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        if (toolTipCanvas == null)
        {
            toolTipCanvas = GameObject.Find("---ToolTipUI---").GetComponent<Canvas>();
            toolTipCanvas.enabled = false;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OpenUI);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    private void OpenUI()
    {
        toolTipCanvas.enabled = true;
    }
}
