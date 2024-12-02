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

        // Audio 관련 추가
        public AudioSource audioSource;
        public AudioClip[] bossSounds; // 보스가 말하는 소리 배열
        private int currentSoundIndex = 0; // 현재 재생할 사운드 인덱스

        // Start is called before the first frame update
        void Start()
        {
            rpgtalk = GetComponent<RPGTalk>();
            changeRoom = FindObjectOfType<ChangeRoom>(); // TempGameManager 인스턴스를 찾음
            rpgtalk.OnEndAnimating += CanPass;
            rpgtalk.OnPlayNext += Passed;
            // 모든 대화가 완전히 종료될 때 발생하는 이벤트
            rpgtalk.OnEndTalk += AllDialogsEnded;

            if (bossSounds != null && bossSounds.Length > 0)
            {
                audioSource.clip = bossSounds[currentSoundIndex];
                audioSource.Play();
                currentSoundIndex++;
            }
            else
            {
                Debug.LogWarning("bossSounds 배열이 비어 있습니다. AudioClip을 추가하세요.");
            }

            // 대화 중에는 넘어가지 않도록 설정
            rpgtalk.enablePass = false;
            StartCoroutine(Wait(1f));
        }

        private void Update()
        {
            Debug.Log("rpgtalk.enablePass:"+rpgtalk.enablePass);
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
            // 사운드 재생
            if (bossSounds != null && currentSoundIndex < bossSounds.Length)
            {
                audioSource.clip = bossSounds[currentSoundIndex];
                audioSource.Play();
                currentSoundIndex++;
            }
            rpgtalk.enablePass = false;

            // 2.5초 후에 다음 대화로 넘어가도록 설정
            StartCoroutine(Wait(2.5f));
        }

        IEnumerator Wait(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            rpgtalk.enablePass = true;
        }

        void AllDialogsEnded()
        {
            changeRoom.AllDialogsEnded();
        }
    }
}