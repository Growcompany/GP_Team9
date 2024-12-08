using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class TutorialEnd : MonoBehaviour
{
    public FadeEffect fadeEffect;       // FadeUI에 있는 FadeEffect를 가져옴
    public float duration = 2.0f;

    [Header("Confiner")]
    public CinemachineConfiner2D confiner;
    public PolygonCollider2D tutorial;
    public PolygonCollider2D main;

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

        confiner.BoundingShape2D = main;

        fadeEffect.FadeIn(null, duration);
    }
}
