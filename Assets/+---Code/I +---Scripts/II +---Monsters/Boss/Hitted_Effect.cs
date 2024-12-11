using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitted_Effect : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnAnimationEnd()
    {
        Destroy(gameObject); // 애니메이션이 끝나면 객체 삭제
    }
}
 
