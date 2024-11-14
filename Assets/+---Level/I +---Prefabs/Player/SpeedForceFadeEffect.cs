using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpeedForceFadeEffect : MonoBehaviour
{
    public Image fadeImage;
    public float duration = 5.0f;

    private void Awake()
    {
        fadeImage.gameObject.SetActive(true);

        Color colorImage = fadeImage.color;
        colorImage.a = 0;
        fadeImage.color = colorImage;
    }

    public void FadeIn()
    {
        StartCoroutine(Fade(0.02f, 0f));
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(0f, 0.02f));
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0.0f;
        Color colorImage = fadeImage.color;

        fadeImage.gameObject.SetActive(true);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.fixedUnscaledDeltaTime;
            colorImage.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);

            fadeImage.color = colorImage;
            yield return null;
        }

        colorImage.a = endAlpha;
        fadeImage.color = colorImage;
    }
}
