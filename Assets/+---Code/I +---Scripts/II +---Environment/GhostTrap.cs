using UnityEngine;

public class GhostTrap : MonoBehaviour
{
    public GameObject ghostTrap;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ghostTrap.SetActive(true);
            GhostController[] ghosts = ghostTrap.GetComponentsInChildren<GhostController>();
            foreach (GhostController ghost in ghosts)
            {
                ghost.gameObject.SetActive(true);
            }
        }
    }
}
