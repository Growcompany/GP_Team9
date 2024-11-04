using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public int index;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            RespawnPointManager.Instance.Save(index);
        }
    }

}
