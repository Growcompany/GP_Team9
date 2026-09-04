using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Exp : MonoBehaviour
{
    public float duration = 1f;
    public int exp = 5;

    private PlayerController m_player;
    private AudioSource m_audioSource;
    private SpriteRenderer m_spriteRenderer;
    private Collider2D m_collider2D;
    private Tweener m_Xtween;
    private Tweener m_Ytween;
    private bool m_isCollected = false;
    private bool m_startTween = false;

    private void Start()
    {
        m_player = GameManager.instance.Player;
        m_audioSource = GetComponent<AudioSource>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_collider2D = GetComponent<Collider2D>();

        StartCoroutine(TweenStartCoroutine());
    }
    
    IEnumerator TweenStartCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
        m_startTween = true;

        m_Xtween = DOTween.To(() => transform.position.x,
                      x =>
                      {
                          Vector3 pos = transform.position;
                          pos.x = x;
                          transform.position = pos;
                      },
                      m_player.transform.position.x,
                      duration)
                .SetEase(Ease.OutCubic);

        m_Ytween = DOTween.To(() => transform.position.y,
                                 y =>
                                 {
                                     Vector3 pos = transform.position;
                                     pos.y = y;
                                     transform.position = pos;
                                 },
                                 m_player.transform.position.y,
                                 duration)
                .SetEase(Ease.OutCubic);
    }

    private void Update()
    {
        float distance = Vector2.Distance(transform.position, m_player.transform.position);
        if(distance <= 2.0f)
        {
            HandleCollsion();
        }
        // R 값을 0 ~ 1로 반복
        float rValue = Mathf.PingPong(Time.time, 1f);

        Color color = m_spriteRenderer.color;
        color.r = rValue; // R 값 변경
        m_spriteRenderer.color = color;

        if (m_startTween)
        {
            m_Xtween.ChangeEndValue(m_player.transform.position.x, true).Restart();
            m_Ytween.ChangeEndValue(m_player.transform.position.y, true).Restart();
        }
    }

    private void HandleCollsion()
    {
        if(!m_isCollected)
        {
            m_isCollected = true;

            m_collider2D.enabled = false;
            Debug.LogWarning("Exp Collision");
            if (m_player != null)
            {
                m_player.ExpUp(exp);
            }

            m_audioSource.Play();
            m_spriteRenderer.enabled = false;


            Destroy(gameObject, 2);
        }   
    }

    private void OnDestroy()
    {
        m_Xtween.Kill();
        m_Ytween.Kill();
    }
}

