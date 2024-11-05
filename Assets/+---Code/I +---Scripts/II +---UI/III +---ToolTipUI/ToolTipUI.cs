using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToolTipUI : MonoBehaviour
{
    Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();

        if (SceneManager.GetActiveScene().name == "SampleScene")
            canvas.enabled = true;
    }
}
