using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class StartSceneController : MonoBehaviour
{
    public Image darkScreen;                 // Canvas - DarkScreen
    public GameObject character;            // Character
    public float duration = 5.0f;          // Speed of the character & dark screen

    private AudioSource m_audioSource;       // Audio Source
    private bool m_isPressed = false;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && !m_isPressed)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                m_isPressed = true;
                m_audioSource.Play();

                // 120.0f 까지 character 이동
                character.transform.DOMoveX(120.0f, duration).SetEase(Ease.Linear);
                character.GetComponent<Animator>().Play("Player Move");

                // fillAmount 1.0f 까지 darkScreen 채우기
                darkScreen.DOFillAmount(1.0f, duration).SetEase(Ease.Linear);

                // Loading SampleScene Mobile
                SceneTransition.Instance.LoadScene("SampleScene Mobile");
            }
        }
    }
}
