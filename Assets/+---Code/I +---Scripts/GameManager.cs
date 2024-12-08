using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private PlayerController m_player;
    public PlayerController Player
    {
        get 
        { 
            if(m_player == null)
                m_player = Object.FindFirstObjectByType<PlayerController>(); 
            return m_player;
        }
        private set 
        {
            m_player = value; 
        }
    }

    [SerializeField] private ParticleSystem m_levelUpEffect;

    public int totalUsedStatusPoint;                    // ConfirmButtonUI에서 조절
    public int availablePoint;                          // CalculateAvailableStatusPoint 이벤트로 계산됨(ex. Level up, Confirm button)
    public int currentUsedStatusPoint;                  // PlusMinusButtonUI에서 증감, ConfirmButtonUI에서 초기화
                                                        // PointTextUI에서 availablePoint - currentUsedStatusPoint값 사용
    public float coolTimeRatio;                         // SkillCoolTimeUI에서 사용
    public float currentCoolTime;                       // SkillCoolTimeUI에서 사용

    public bool IsPaused { get; private set; }


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.LogError("GameManager already exists");
            Destroy(gameObject);
            return;
        }

        if(Player == null)
            Player = Object.FindFirstObjectByType<PlayerController>();

        
        // DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "SampleScene" || currentScene.name == "SampleScene Mobile")
        {
            CalculateAvailableStatusPoint();
        }
    }

    public void CalculateAvailableStatusPoint()
    {
        availablePoint = Player.MovementStats.Level - totalUsedStatusPoint - 1;
    }

    public void ConfirmPoints()
    {
        totalUsedStatusPoint += currentUsedStatusPoint;
        currentUsedStatusPoint = 0;
        CalculateAvailableStatusPoint();
    }

    public IEnumerator CalculateCoolTime(float skillCoolTime)
    {
        currentCoolTime = skillCoolTime;
        float per = 1 / skillCoolTime;

        while (currentCoolTime >= 0.0f)
        {
            currentCoolTime -= Time.deltaTime;
            coolTimeRatio = Mathf.Clamp(currentCoolTime * per, 0.0f, 1.0f);

            yield return null;
        }
    }

    public void LevelUpEffect()
    {
        StartCoroutine(FollowPlayer(m_levelUpEffect.gameObject));
        m_levelUpEffect.Play();
    }

    private IEnumerator FollowPlayer(GameObject go)
    {
        // 2.5초간 플레이어를 따라다님
        float time = 2.5f;
        while (time > 0)
        {
            time -= Time.deltaTime;
            go.transform.position = Player.transform.position + new Vector3(0, 0.3f, 0);
            yield return null;
        }
    }

    public void Pause(bool isPaused)
    {
        IsPaused = isPaused;
        float timeScale = IsPaused ? 0 : 1;
        Time.timeScale = timeScale;
    }


    public void Update()
    {
        //if (!IsPaused)
        //    Pause(true);
    }
}
