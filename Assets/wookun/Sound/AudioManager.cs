using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    private AudioSource audioSource;

    // AudioManager 인스턴스가 없으면 자동으로 생성되도록 합니다.
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                // 새로운 GameObject를 만들어 AudioManager를 추가합니다.
                GameObject audioManagerObject = new GameObject("AudioManager");
                instance = audioManagerObject.AddComponent<AudioManager>();
                DontDestroyOnLoad(audioManagerObject);

                // AudioSource를 추가하고 초기화합니다.
                instance.audioSource = audioManagerObject.AddComponent<AudioSource>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        // 싱글톤 패턴 적용
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // AudioSource 초기화
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static void PlayProximitySound(AudioClip clip)
    {
        // 인스턴스를 강제로 초기화하여 null 상태 방지
        AudioManager manager = Instance;

        // AudioSource가 null 상태인지 다시 확인합니다.
        if (manager.audioSource == null)
        {
            Debug.LogWarning("AudioSource component could not be initialized.");
            return;
        }

        // 동일한 사운드가 이미 재생 중인지 확인
        if (manager.audioSource.isPlaying && manager.audioSource.clip == clip)
        {
            return; // 이미 재생 중이면 다시 재생하지 않음
        }

        // 사운드를 재생
        manager.audioSource.clip = clip;
        manager.audioSource.loop = true;
        manager.audioSource.Play();
    }

    public static void StopProximitySound()
    {
        if (instance != null && instance.audioSource != null && instance.audioSource.isPlaying)
        {
            instance.audioSource.Stop();
        }
    }
}
