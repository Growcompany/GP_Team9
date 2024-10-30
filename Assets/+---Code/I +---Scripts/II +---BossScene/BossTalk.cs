using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPGTALK.Snippets
{
    [AddComponentMenu("Seize Studios/RPGTalk/Snippets/Can Pass Warning")]
    public class BossTalk : MonoBehaviour
    {

        public UnityEvent OnCanPass, OnPassed;
        private ChangeRoom changeRoom;

        RPGTalk rpgtalk;

        // Start is called before the first frame update
        void Start()
        {
            rpgtalk = GetComponent<RPGTalk>();
            changeRoom = FindObjectOfType<ChangeRoom>(); // TempGameManager 인스턴스를 찾음
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
            changeRoom.AllDialogsEnded();
        }
    }
}