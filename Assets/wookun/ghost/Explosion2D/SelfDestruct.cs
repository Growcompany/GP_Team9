using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSelfDestruct : MonoBehaviour
{
    public float lifeTime = 2.0f; // 파티클 재생 시간에 맞게 설정

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
