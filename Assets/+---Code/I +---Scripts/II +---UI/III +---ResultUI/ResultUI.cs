using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ResultUI : MonoBehaviour
{
    public TMP_Text deathText;
    public TMP_Text timeText;

    public UnityEvent enableEvent;

    private void Awake()
    {
        if(deathText == null)
        {
            deathText = transform.Find("Death").Find("Count").GetComponent<TMP_Text>();
        }

        if(timeText == null)
        {
            timeText = transform.Find("Time").Find("Count").GetComponent<TMP_Text>();
        }
    }

    private void OnEnable()
    {
       deathText.text = SceneTransition.Instance.deathCount.ToString();
       timeText.text = SceneTransition.Instance.elapsedTime.ToString("F2");
    }
}
