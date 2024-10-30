using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack3Warning : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        // 알파값을 0.1에서 0.3 왔다 갔다 하게 설정
        float alpha = Mathf.Lerp(0.2f, 0.7f, Mathf.PingPong(Time.time, 1));

        // 변경된 알파값을 사용하여 색상 설정
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
    }
}
