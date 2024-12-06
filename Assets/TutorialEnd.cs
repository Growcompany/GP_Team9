using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnd : MonoBehaviour
{
    public FadeEffect fadeEffect;       // FadeUI에 있는 FadeEffect를 가져옴
    public float duration = 2.0f;

    [SerializeField] private GameObject spawnPoint;
    private bool isPlayerIn = false;


    private void Awake()
    {
        if(spawnPoint == null)
            spawnPoint = transform.Find("spawnPoint").gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isPlayerIn)
        {
            isPlayerIn = true;
            StartCoroutine(Fade());
            GameManager.instance.Player.gameObject.SetActive(false);
        }
    }

    private IEnumerator Fade()
    {
        Debug.Log("Fade");
        fadeEffect.FadeOut(null, duration);
        yield return new WaitForSeconds(duration);
        GameManager.instance.Player.gameObject.SetActive(true);
        GameManager.instance.Player.transform.position = spawnPoint.transform.position;
        fadeEffect.FadeIn(null, duration);
    }
}
