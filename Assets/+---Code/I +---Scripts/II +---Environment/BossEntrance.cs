using UnityEngine;

public class BossEntrance : MonoBehaviour
{
    public LayerMask triggerLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer) == triggerLayer)
        {
            SceneTransition.Instance.LoadScene("BossScene");
        }
    }
}
