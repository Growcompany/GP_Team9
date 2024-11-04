using System.Collections;
using UnityEngine;

public class BoxDestruction : MonoBehaviour
{
    private Animator animator;
    private Collider2D boxCollider;
    private bool isBreaking = false;
    public AudioClip destructionSound; // 파괴 사운드
    private AudioSource audioSource; // AudioSource 컴포넌트

    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<Collider2D>();

        // AudioSource 컴포넌트 추가 및 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = destructionSound;
        audioSource.playOnAwake = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isBreaking)
        {
            BreakBox();
        }
    }

    private void BreakBox()
    {
        isBreaking = true;
        animator.SetTrigger("Break");
        boxCollider.enabled = false;

        // 파괴 사운드 재생
        if (audioSource != null && destructionSound != null)
        {
            audioSource.Play();
        }

        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}
