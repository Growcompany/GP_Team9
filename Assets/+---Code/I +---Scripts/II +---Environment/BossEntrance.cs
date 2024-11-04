using UnityEngine;

public class BossEntrance : MonoBehaviour
{
    public LayerMask triggerLayer;
    private bool alreadyTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer) == triggerLayer)
        {
            if (!alreadyTriggered)
            {
                alreadyTriggered = true;
                SceneTransition.Instance.LoadScene("BossScene");
            }
        }
    }
}
