using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance {  get; private set; }

    [SerializeField] private FadeEffect fadeEffect;
    private bool isWaiting = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Already SceneTransition exists");
            Destroy(gameObject);
        }

        Init();
    }
    private void Init()
    {
        if (fadeEffect == null)
        {
            fadeEffect = GameObject.Find("FadeUI").GetComponent<FadeEffect>();
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        fadeEffect.FadeOut();
        yield return new WaitForSeconds(fadeEffect.duration);   // Fade In 효과 보장용
        yield return StartCoroutine(LoadSceneAsync(sceneName));
        fadeEffect.FadeIn();
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        Debug.LogWarning("Load Start");
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        while (!async.isDone)
        {
            yield return null;
        }
        Debug.LogWarning("Load End");

    }
}
