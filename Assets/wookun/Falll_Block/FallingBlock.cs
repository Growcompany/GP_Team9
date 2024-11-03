using System.Collections;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    public GameObject explosionEffectPrefab;
    public AudioClip destructionSound; // 파괴 사운드
    private Animator animator;
    private Rigidbody2D rb;
    private bool isFalling = false;
    private bool isDestroyed = false;
    private AudioSource audioSource; // AudioSource 컴포넌트

    // 초기 위치와 회전 상태를 저장할 변수
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // 초기 상태를 Kinematic으로 설정하여 중력 영향을 받지 않음
        rb.bodyType = RigidbodyType2D.Kinematic;

        // AudioSource 컴포넌트 추가 및 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = destructionSound;
        audioSource.playOnAwake = false;

        // 초기 위치와 회전 저장
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isFalling && other.CompareTag("Player"))
        {
            isFalling = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDestroyed && isFalling && (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Player")))
        {
            isDestroyed = true;
            TriggerDestruction();
        }
    }

    private void TriggerDestruction()
    {
        animator.SetTrigger("fall");
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector2.zero;

        // 파괴 사운드 재생
        if (audioSource != null && destructionSound != null)
        {
            audioSource.Play();
        }

        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        gameObject.SetActive(false); // 오브젝트를 비활성화하여 삭제 효과 제공
    }

    // revive 후 오브젝트를 원상복구하는 메서드
    public void ResetBlock()
    {
        // 초기 위치와 회전으로 복구
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // 상태 초기화
        isFalling = false;
        isDestroyed = false;

        // Rigidbody 상태 초기화
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector2.zero;

        // 애니메이션 상태 초기화
        animator.ResetTrigger("fall");

        // 오브젝트 활성화
        gameObject.SetActive(true);
    }
}
