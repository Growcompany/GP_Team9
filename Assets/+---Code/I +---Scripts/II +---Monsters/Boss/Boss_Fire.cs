using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Fire : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Destory());
    }

    private IEnumerator Destory()
    {
        yield return new WaitForSeconds(4f); // 충분한 대기 시간 설정

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
