using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpecialSpace : MonoBehaviour
{
    public float fadeDuration = 5.0f;

    [SerializeField] private GameObject lightEffect;
    private bool isPlayerInside = false;
    private SpriteRenderer m_spriteRenderer;
    private ParticleSystem m_particleSystem;

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_particleSystem = GetComponent<ParticleSystem>();
        lightEffect = transform.Find("Light").gameObject;
        lightEffect.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isPlayerInside)
        {
            isPlayerInside = true;
            // Fade out the sprite
            m_spriteRenderer.DOFade(0.6f, fadeDuration);
            m_particleSystem.Play();
            lightEffect.SetActive(true);

            Debug.Log("Player entered special space");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isPlayerInside)
        {
            isPlayerInside = false;
            // Fade in the sprite
            m_spriteRenderer.DOFade(1.0f, fadeDuration);
            m_particleSystem.Stop();
            lightEffect.SetActive(false);

            Debug.Log("Player exited special space");
        }
    }
}
