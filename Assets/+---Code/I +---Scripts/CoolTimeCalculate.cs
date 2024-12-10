using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoolTimeCalculate : MonoBehaviour
{
    public float[] coolTimeRatio;                   // size = player's skill count
                                                    // 0 = skill1, 1 = reduce                                          
    public float[] remainTime;
    private void Start()
    {
        PlayerController player = GetComponent<PlayerController>();
        coolTimeRatio = new float[2];               // player's skill count = 2
        remainTime = new float[2];                  // player's skill count = 2
    }

    // Ratio is for image.fillAmount ==> 1.0f ~ 0.0f reduce
    public IEnumerator CalculateCoolTime(float skillCoolTime, int skillIndex)
    {
        float per = 1 / skillCoolTime;

        coolTimeRatio[skillIndex] = 0.0f;
        remainTime[skillIndex] = skillCoolTime;

        while (remainTime[skillIndex] >= 0.0f)
        {
            remainTime[skillIndex] -= Time.deltaTime;
            coolTimeRatio[skillIndex] = Mathf.Clamp(remainTime[skillIndex] * per, 0.0f, 1.0f);

            yield return null;
        }

        // Initialize
        coolTimeRatio[skillIndex] = 0.0f;
        remainTime[skillIndex] = 0.0f;
    }
}
