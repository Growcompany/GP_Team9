using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance {  get; private set; }

    public int deathCount = 0;
    public float elapsedTime = 0;
    [SerializeField] private FadeEffect fadeEffect;

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

    private void Update()
    {
        elapsedTime += Time.deltaTime;
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
        // TODO: player dontdestroy되면 그냥 가져오기
        if (sceneName == "BossScene")
        {
            deathCount += GameManager.instance.Player.dieCount;
        }
        else
        {
            deathCount = 0;
            elapsedTime = 0;
        }

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
