using UnityEngine;

public class BossEntrance : MonoBehaviour
{
    public LayerMask triggerLayer;
    private bool alreadyTriggered = false;
    //[Header("References")]
    //public PlayerMovementStats MovementStats;
    //public DataHandler dataHandler;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer) == triggerLayer)
        {
            if (!alreadyTriggered)
            {
                // 보스룸에 들어가기 전 데이터를 저장
                //SaveData();

                alreadyTriggered = true;
                SceneTransition.Instance.LoadScene("BossScene");
            }
        }
    }

    //public void SaveData()
    //{
    //    // MovementStats 데이터를 기반으로 PlayerStats 객체 생성
    //    PlayerStats stats = new PlayerStats
    //    {
    //        MaxLife = MovementStats.MaxLife,
    //        Life = MovementStats.Life,
    //        Exp = MovementStats.Exp,
    //        Level = MovementStats.Level,
    //        Strength = MovementStats.Strength,
    //        Dodge = MovementStats.Dodge,
    //        SkillCoolTime = MovementStats.SkillCoolTime
    //    };

    //    // 데이터 저장
    //    dataHandler.SaveData(stats);
    //    Debug.Log("Player data saved before entering the boss room.");
    //}
}
