using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointManager : MonoBehaviour
{
    public static RespawnPointManager Instance { get; private set; }

    [SerializeField] private int currentRespawnPointIndex;
    [SerializeField] private List<RespawnPoint> respawnPoints;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Already RespawnManager exists");
            Destroy(gameObject);
        }

        RespawnPoint[] temp = GetComponentsInChildren<RespawnPoint>();
        int n = temp.Length;

        for (int i = 0; i < n; i++)
        {
            respawnPoints.Add(temp[i]);
        }
    }

    // RespawnPoint -> RespawnPointManager
    public void Save(int index)
    {
        currentRespawnPointIndex = index;
    }

    // 외부에서 사용
    public void Respawn(PlayerController player)
    {
        StartCoroutine(RespawnPlayerAfterDeathUI(player));
    }

    private IEnumerator RespawnPlayerAfterDeathUI(PlayerController player)
    {
        // 대충 4초로 잡음
        yield return new WaitForSeconds(4);
        player.fadeEffectUI.fadeImage.gameObject.SetActive(false);
        player.transform.position = respawnPoints[currentRespawnPointIndex].transform.position;
    }
}
