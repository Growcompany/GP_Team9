using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeUI : MonoBehaviour
{
    [SerializeField] List<Transform> lifeImages;

    int previousLifes;

    private void Start()
    {
        previousLifes = GameManager.instance.Player.MovementStats.MaxLife;

        // All SetActive(false)
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            lifeImages.Add(child);
            lifeImages[i].gameObject.SetActive(false);
        }

        // MaxLife SetActive(true)
        for (int i = 0; i < previousLifes; i++)
        {
            Transform child = transform.GetChild(i);
            lifeImages[i].gameObject.SetActive(true);
        }

        GameManager.instance.Player.lifeUpdateUIEvent.AddListener(UpdateLifeImages);
    }

    private void OnDestroy()
    {
        GameManager.instance.Player.lifeUpdateUIEvent.RemoveAllListeners();

    }

    // TODO: Player에서
    // UnityEvent<int> lifeUpdateUIEvent 추가 필요
    // 이후 Level up하면 lifeUpdateUIEvent.Invoke(currentLife: int) 추가 필요
    void UpdateLifeImages(int currentLifes)
    {
        int diff = currentLifes - previousLifes;

        // SetActive(false)
        if (diff < 0)
        {
            for (int i = 0; i < Math.Abs(diff); i++)
            {
                lifeImages[Math.Clamp(previousLifes - 1 - i, 0, 9)].gameObject.SetActive(false);
            }
        }

        // SetActive(true)
        else
        {
            for (int i = 0; i < Math.Abs(diff); i++)
            {
                lifeImages[Math.Clamp(previousLifes + i, 0, 9)].gameObject.SetActive(true);
            }
        }
        previousLifes = currentLifes;
    }
}