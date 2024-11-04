using UnityEngine;

public class ChangeRoom : MonoBehaviour
{    
    public GameObject RoomEnter; // RoomEnter 오브젝트
    public GameObject RoomPlay;  // RoomPlay
    public GameObject BaseUI;  // BaseUI 오브젝트
    public GameObject StatusUI;  // StatusUI 오브젝트
    //public DataHandler dataHandler;
    //public PlayerMovementStats playerMovementStats; 

    public void Start()
    {
        BaseUI = GameObject.Find("---BaseUI---");
        StatusUI = GameObject.Find("---StatusUI---");

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

        
        // 데이터 불러오기
        //PlayerStats loadedStats = dataHandler.LoadData();

        //if (loadedStats != null)
        //{
        //    // 불러온 데이터를 PlayerMovementStats에 적용
        //    ApplyDataToPlayerMovementStats(loadedStats, playerMovementStats);
        //    Debug.Log("Player data loaded and applied in the Boss Scene.");
        //}
        //else
        //{
        //    Debug.LogWarning("No data found to load in the Boss Scene.");
        //}
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

    //private void ApplyDataToPlayerMovementStats(PlayerStats loadedStats, PlayerMovementStats movementStats)
    //{
    //    movementStats.MaxLife = loadedStats.MaxLife;
    //    movementStats.Life = loadedStats.Life;
    //    movementStats.Exp = loadedStats.Exp;
    //    movementStats.Level = loadedStats.Level;
    //    movementStats.Strength = loadedStats.Strength;
    //    movementStats.Dodge = loadedStats.Dodge;
    //    movementStats.SkillCoolTime = loadedStats.SkillCoolTime;

    //    // 로드된 정보 로그 출력
    //    Debug.Log("Loaded Player Stats:");
    //    Debug.Log("MaxLife: " + loadedStats.MaxLife);
    //    Debug.Log("Life: " + loadedStats.Life);
    //    Debug.Log("Exp: " + loadedStats.Exp);
    //    Debug.Log("Level: " + loadedStats.Level);
    //    Debug.Log("Strength: " + loadedStats.Strength);
    //    Debug.Log("Dodge: " + loadedStats.Dodge);
    //    Debug.Log("SkillCoolTime: " + loadedStats.SkillCoolTime);
    //}

}
