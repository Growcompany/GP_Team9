using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject baseUI;
    public GameObject statusUI;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Debug.LogError("UIManager already exists");
            return;
        }

        // Manager
        DontDestroyOnLoad(gameObject);

        // UIs
        DontDestroyOnLoad(baseUI);
        DontDestroyOnLoad(statusUI);
    }
}
