using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeUI : MonoBehaviour
{
    [SerializeField] List<Transform> lifeImages;

    int previousLifes;

    [Tooltip("player에서 life 만들면 삭제")]
    public int currentLifes;

    private void Start()
    {
        previousLifes = GameManager.instance.player.MovementStats.MaxLife;

        // All SetActive(false)
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.gameObject.SetActive(false);
        }

        // MaxLife SetActive(true)
        for (int i = 0; i < previousLifes; i++)
        {
            Transform child = transform.GetChild(i);
            lifeImages.Add(child);
            lifeImages[i].gameObject.SetActive(true);
        }

        GameManager.instance.player.lifeUpdateUIEvent.AddListener(UpdateLifeImages);
    }

    private void OnDestroy()
    {
        GameManager.instance.player.lifeUpdateUIEvent.RemoveListener(UpdateLifeImages);

    }

    // TODO: Player에서
    // UnityEvent<int> lifeUpdateUIEvent 추가 필요
    // 이후 Level up하면 lifeUpdateUIEvent.Invoke(currentLife: int) 추가 필요
    void UpdateLifeImages(int currentLifes)
    {
        int diff = currentLifes - previousLifes;

        // SetActive(false)
        if(diff < 0)
        {
            for(int i = 0; i < Math.Abs(diff); i++)
            {
                lifeImages[previousLifes - 1 - i].gameObject.SetActive(false);
            }
        }

        // SetActive(true)
        else
        {
            for(int i = 0; i < Math.Abs(diff); i++)
            {
                lifeImages[previousLifes + i].gameObject.SetActive(true);
            }
        }

        previousLifes = currentLifes;
    }
}