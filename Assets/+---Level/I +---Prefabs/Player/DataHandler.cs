using System.IO;
using UnityEngine;
public class DataHandler : MonoBehaviour
{
    private string filePath;

    private void Awake()
    {
        // JSON 파일 경로 설정
        filePath = Application.persistentDataPath + "/playerStats.json";
    }

    public void SaveData(PlayerStats stats)
    {
        // 객체를 JSON으로 직렬화하여 파일로 저장
        string json = JsonUtility.ToJson(stats, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Data saved to " + filePath);
    }

    public PlayerStats LoadData()
    {
        if (File.Exists(filePath))
        {
            // JSON 파일을 읽어 객체로 역직렬화
            string json = File.ReadAllText(filePath);
            PlayerStats stats = JsonUtility.FromJson<PlayerStats>(json);
            Debug.Log("Data loaded from " + filePath);
            return stats;
        }
        else
        {
            Debug.LogWarning("Save file not found.");
            return null;
        }
    }
}

[System.Serializable]
public class PlayerStats
{
    public int MaxLife;
    public int Life;
    public int Exp;
    public int Level;
    public int Strength;
    public int Dodge;
    public int SkillCoolTime;
}

