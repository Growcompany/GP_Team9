using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public int index;

    private bool isPlayerIn = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isPlayerIn)
        {
            isPlayerIn = true;
            RespawnPointManager.Instance.Save(index);
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isPlayerIn)
        {
            isPlayerIn = true;
            RespawnPointManager.Instance.Save(index);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isPlayerIn)
        {
            isPlayerIn = false;
        }
    }
}
