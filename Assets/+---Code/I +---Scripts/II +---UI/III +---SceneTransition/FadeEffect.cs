using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    public Image fadeImage;

    public bool isText = false;
    public TMP_Text fadeText;
    public float duration = 1.0f;

    private void Awake()
    {
        // 기본 상태가 false라서 켜줌
        fadeImage.gameObject.SetActive(true);

        Color colorImage = fadeImage.color;
        colorImage.a = 0;
        fadeImage.color = colorImage;

        if (isText)
        {
            Color colorText = fadeText.color;
            colorText.a = 0;
            fadeText.color = colorText;
        }
    }

    public void FadeIn(AudioClip audioClip = null)
    {
        StartCoroutine(Fade(1, 0, audioClip));
    }

    public void FadeOut(AudioClip audioClip = null)
    {
        StartCoroutine(Fade(0, 1, audioClip));
    }

    IEnumerator Fade(float startAlpha, float endAlpha, AudioClip audioClip = null)
    {
        if(!audioClip.IsUnityNull())
        {
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioClip);
        }    

        float elapsedTime = 0.0f;
        Color colorImage = fadeImage.color;
        Color colorText = new Color();

        fadeImage.gameObject.SetActive(true);
        if(isText)
        {
            colorText = fadeText.color;

        }

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            colorImage.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);

            fadeImage.color = colorImage;
            if(isText)
            {
                colorText.a = colorImage.a;
                fadeText.color = colorText;
            }
            yield return null;
        }

        colorImage.a = endAlpha;
        fadeImage.color = colorImage;

        if (isText)
        {
            colorText.a = endAlpha;
            fadeText.color = colorText;
        }
    }
}
