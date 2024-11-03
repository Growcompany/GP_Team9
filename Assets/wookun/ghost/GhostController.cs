using System.Collections;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 2.0f;
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1.0f;
    public GameObject explosionEffectPrefab;
    public float explosionRadius = 2.0f;
    public AudioClip explosionSound; // 폭발 사운드 클립 추가
    private Vector3 initialPosition;
    private Animator anim;
    private bool hasExploded = false;
    private CircleCollider2D explosionCollider;
    private AudioSource audioSource; // AudioSource 추가

    void Start()
    {
        anim = GetComponent<Animator>();

        // "Player" 태그를 가진 오브젝트를 찾아 플레이어의 Transform을 할당
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                Debug.LogWarning("Player object with tag 'Player' not found in the scene.");
            }
        }

        initialPosition = transform.position;

        // CircleCollider2D 컴포넌트를 추가하고 초기에는 비활성화
        explosionCollider = gameObject.AddComponent<CircleCollider2D>();
        explosionCollider.isTrigger = false;
        explosionCollider.radius = explosionRadius;
        explosionCollider.enabled = false;

        // AudioSource 컴포넌트를 추가하고 폭발 사운드를 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = explosionSound;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (player != null && !hasExploded)
        {
            transform.position = Vector3.Lerp(transform.position, player.position, followSpeed * Time.deltaTime);
        }

        if (!hasExploded)
        {
            float floatOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
            transform.position += new Vector3(0, floatOffset * Time.deltaTime, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasExploded)
        {
            anim.SetTrigger("isScared");
            Invoke("StartExplosion", 0.5f);
        }
    }

    private void StartExplosion()
    {
        anim.SetTrigger("isExploding");
        anim.ResetTrigger("isScared");
        hasExploded = true;

        // 폭발 사운드 재생
        if (audioSource != null && explosionSound != null)
        {
            audioSource.Play();
        }

        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        explosionCollider.enabled = true;
        StartCoroutine(DisableExplosionCollider());
        StartCoroutine(DestroyAfterExplosion());
    }

    private IEnumerator DisableExplosionCollider()
    {
        yield return new WaitForSeconds(0.1f);
        explosionCollider.enabled = false;
    }

    private IEnumerator DestroyAfterExplosion()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && explosionCollider.enabled)
        {
            Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
            collision.rigidbody.velocity = pushDirection * 10f;
        }
    }
}

