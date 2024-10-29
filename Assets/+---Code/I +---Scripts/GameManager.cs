using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerController player;

    public int totalUsedStatusPoint;                    // ConfirmButtonUI에서 조절
    public int availablePoint;                          // CalculateAvailableStatusPoint 이벤트로 계산됨(ex. Level up, Confirm button)
    public int currentUsedStatusPoint;                  // PlusMinusButtonUI에서 증감, ConfirmButtonUI에서 초기화
                                                        // PointTextUI에서 availablePoint - currentUsedStatusPoint값 사용

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.LogError("GameManager alread exists");
            return;
        }

        CalculateAvailableStatusPoint();
    }

    public void CalculateAvailableStatusPoint()
    {
        availablePoint = player.MovementStats.Level - totalUsedStatusPoint - 1;
    }

    public void ConfirmPoints()
    {
        totalUsedStatusPoint += currentUsedStatusPoint;
        currentUsedStatusPoint = 0;
        CalculateAvailableStatusPoint();
    }
}
