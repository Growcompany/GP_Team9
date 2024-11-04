using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPGTALK.Snippets
{
    [AddComponentMenu("Seize Studios/RPGTalk/Snippets/Can Pass Warning")]
    public class RPGTalkCanPassWarning : MonoBehaviour
    {

        public UnityEvent OnCanPass, OnPassed;

        RPGTalk rpgtalk;

        // Start is called before the first frame update
        void Start()
        {
            rpgtalk = GetComponent<RPGTalk>();
            rpgtalk.OnEndAnimating += CanPass;
            rpgtalk.OnPlayNext += Passed;
            // 모든 대화가 완전히 종료될 때 발생하는 이벤트
            rpgtalk.OnEndTalk += AllDialogsEnded;
        }

        void CanPass()
        {
            if (rpgtalk.enablePass)
            {
                OnCanPass.Invoke();
            }
        }

        void Passed()
        {
            OnPassed.Invoke();
        }

        void AllDialogsEnded()
        {
            Debug.Log("모든 대화가 종료되었습니다."); // 모든 대화가 종료될 때 로그 출력
        }
    }
}