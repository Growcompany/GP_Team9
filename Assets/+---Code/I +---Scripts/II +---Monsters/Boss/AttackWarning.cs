using UnityEngine;

public class AttackWarning : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float minAlpha = 0.3f; // 투명도 30%
    public float maxAlpha = 1f;   // 투명도 100%
    public float speed = 2f;      // 변화 속도

    private Color startColor = Color.yellow;
    private Color targetColor = Color.red;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = startColor; // 초기 색상을 노란색으로 설정
    }

    void Update()
    {
        // 투명도를 30~100% 사이로 반복 변화
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(Time.time * speed, 1));

        // 노란색에서 빨간색으로 점차 변화
        Color currentColor = Color.Lerp(startColor, targetColor, Mathf.PingPong(Time.time * speed, 1));

        // 색상과 투명도를 적용
        currentColor.a = alpha;
        spriteRenderer.color = currentColor;
    }
}