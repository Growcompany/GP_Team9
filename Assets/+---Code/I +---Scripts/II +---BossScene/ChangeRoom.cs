using UnityEngine;
using UnityEngine.Events;

public class ChangeRoom : MonoBehaviour
{    
    public GameObject RoomEnter; // RoomEnter 오브젝트
    public GameObject RoomPlay;  // RoomPlay
    public GameObject BaseUI;  // BaseUI 오브젝트
    public GameObject StatusUI;  // StatusUI 오브젝트

    public void Start()
    {
        // StatusUI, BaseUI의 Canvas 비활성화
        if (StatusUI != null)
        {
            Canvas statusUICanvas = StatusUI.GetComponent<Canvas>();
            statusUICanvas.enabled = false; // RoomEnter가 활성화될 때만 Canvas를 활성화

        }
        if (BaseUI != null)
        {
            Canvas BaseUICanvas = BaseUI.GetComponent<Canvas>();
            BaseUICanvas.enabled = false; // RoomEnter가 활성화될 때만 Canvas를 활성화
        }
    }
    // BossTalk에서 호출될 때 RoomEnter 비활성화 및 RoomPlay 활성화
    public void AllDialogsEnded()
    {
        Debug.Log("모든 대화가 종료"); // 모든 대화가 종료될 때 로그 출력

        if (RoomEnter != null)
        {
            RoomEnter.SetActive(false);
        }

        if (RoomPlay != null)
        {
            RoomPlay.SetActive(true);
            Canvas statusUICanvas = StatusUI.GetComponent<Canvas>();
            statusUICanvas.enabled = true;
            Canvas BaseUICanvas = BaseUI.GetComponent<Canvas>();
            BaseUICanvas.enabled = true;
        }
    }
}
