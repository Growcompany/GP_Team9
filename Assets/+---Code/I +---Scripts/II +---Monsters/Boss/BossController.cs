using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private float hp;
    private float maxHp; // 최대 HP 값, Start에서 초기화
    [SerializeField] private float speed;
    [SerializeField] private float AttackRange;
    [SerializeField] private int experiencePoints;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private Transform attack2point; // attack2_razer가 보스 앞에 놓일 위치
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attack3Sound; // 공격 소리
    [SerializeField] private Transform hpBarTransform; // HP 바 오브젝트의 Transform
    public Transform[] attackPoints; // 공격 위치 4개의 Transform 배열로 설정
    public Transform[] TeleportPoints; // 자식 Transform들을 저장할 배열
    public GameObject attackPrefab; // 경고+공격 이펙트 프리팹 (하나의 프리팹 안에 경고/공격 이펙트 있음)
    public GameObject attack2Prefab; // Attack2_razer 프리팹
    public GameObject attack3Prefab;  // Attack3 프리팹을 할당
    public GameObject Warning_Attack;
    public GameObject attack4_Fire;
    public GameObject Boss_Move_Effect;
    public float warningDuration = 3f; // 경고 시간 (3초)
    private HashSet<Transform> activeAttackPoints = new HashSet<Transform>(); // 이미 생성된 위치 저장
    public float approachSpeed = 10f; // 공격 위치로 이동할 때 가속도
    private Vector3 originalPosition; // 보스의 원래 위치 저장
    protected Transform player;
    protected Animator anim;
    private bool isDying = false; // 몬스터가 이미 죽음을 처리 중인지 확인
    private bool isAttacking = false; // 현재 공격 중인지 확인
    private bool isInvincible = false; // 데미지 중복 방지
    public GameObject Hitted_Effect;
    public ResultUI resultUI;
    private int Teleport_pos;
    private int attack4_cnt;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        maxHp = hp;
        originalPosition = transform.position; // 시작 시 보스 위치 저장
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player with tag 'Player' not found!");
        }

        StartCoroutine(AttackRoutine());
        StartCoroutine(RandomAttackRoutine());
        //StartCoroutine(RandomTeleportRoutine());
        //InvokeRepeating("StartPerformAttack2Razer", 0f, 10f); // 10초마다 공격 시퀀스를 반복 실행

    }

    private void StartPerformAttack2Razer()
    {
        StartCoroutine(PerformAttack2Razer());
    }

    protected void Update()
    {

        if (isDying) // 죽음 상태일 때 나머지 애니메이션 건너뜀
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("death") && stateInfo.normalizedTime >= 1.0f)
            {
                Destroy(gameObject);
            }
            return; // 더 이상 애니메이션 트리거나 업데이트 작업을 하지 않음
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        //Debug.Log("Player detected at position: " + player.position + ", distanceToPlayer: " + distanceToPlayer);

        if (distanceToPlayer <= detectionRange)
        {
            if (distanceToPlayer <= AttackRange)
            {
            }
            else
            {
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
            }
        }
        else
        {
        }
    }

    #region Attack1
    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            int attackCount = Random.Range(1, 4); // 1~3개중에 생성

            List<Transform> selectedPoints = new List<Transform>();

            // 위치 선택
            for (int i = 0; i < attackCount; i++)
            {
                Transform chosenAttackPoint = null;
                int attempts = 0;

                while (chosenAttackPoint == null && attempts < 10) // 최대 시도 횟수 설정
                {
                    attempts++;
                    int randomIndex = Random.Range(0, attackPoints.Length);
                    Transform potentialPoint = attackPoints[randomIndex];

                    if (!activeAttackPoints.Contains(potentialPoint) && !selectedPoints.Contains(potentialPoint))
                    {
                        chosenAttackPoint = potentialPoint;
                    }
                }

                if (chosenAttackPoint != null)
                {
                    selectedPoints.Add(chosenAttackPoint);
                }
            }

            foreach (Transform attackPoint in selectedPoints)
            {
                GameObject attackInstance = Instantiate(attackPrefab, attackPoint.position, Quaternion.identity);
                GameObject warningEffect = attackInstance.transform.Find("Attack1Warning")?.gameObject;
                GameObject attackEffect = attackInstance.transform.Find("Attack1")?.gameObject;

                activeAttackPoints.Add(attackPoint);

                if (warningEffect != null) warningEffect.SetActive(true);
                if (attackEffect != null) attackEffect.SetActive(false);

                StartCoroutine(ActivateAttackEffect(attackInstance, attackPoint));
            }

            yield return new WaitForSeconds(5f); // 충분한 대기 시간 설정
        }
    }

    private IEnumerator ActivateAttackEffect(GameObject attackInstance, Transform attackPoint)
    {
        // 경고 시간 대기
        yield return new WaitForSeconds(warningDuration);

        GameObject warningEffect = attackInstance.transform.Find("Attack1Warning")?.gameObject;
        GameObject attackEffect = attackInstance.transform.Find("Attack1")?.gameObject;

        if (warningEffect != null) Destroy(warningEffect);
        if (attackEffect != null) attackEffect.SetActive(true);

        //Debug.Log("Attack effect active at " + attackPoint.position);

        // 공격 지속 시간 후 정리
        yield return new WaitForSeconds(1f); // 공격 이펙트 유지 시간 (1초)
        Destroy(attackInstance);
        activeAttackPoints.Remove(attackPoint);
    }

    #endregion

    #region Random_Attack2&Attack3&Teleport
    private IEnumerator RandomAttackRoutine()
    {
        while (true)
        {
            // 공격 중이 아니면 랜덤하게 공격 선택
            if (!isAttacking)
            {
                isAttacking = true;

                Random.InitState(System.DateTime.Now.Millisecond); // 현재 시간을 시드로 설정
                int attackType = Random.Range(0, 7); // 0이면 Attack2_razer, 1이면 Attack3 선택, 2면 teleport 

                Debug.Log("AttackType: " + attackType + "  -0은 레이저 1은 물기 2는 텔포 3은 연속물기");

                if (attackType > -1 && attackType < 2) // 0,1
                {
                    yield return StartCoroutine(PerformAttack2Razer());
                }
                else if(attackType > 1 && attackType < 5) // 2,3,4
                {
                    yield return StartCoroutine(StartAttack3Sequence()); // 변경된 부분
                }
                else if (attackType > 4 && attackType < 6) // 5
                {
                    yield return StartCoroutine(RandomTeleportRoutine()); // 변경된 부분
                }
                else if (attackType == 6)
                {
                    attack4_cnt = 0;
                    yield return StartCoroutine(StartAttack4Sequence()); // 변경된 부분
                }
            }

            yield return new WaitForSeconds(1.5f); // 두 공격 사이의 대기 시간
        }
    }
    #endregion

    #region Attack2
    private IEnumerator PerformAttack2Razer()
    {
        // Attack2_razer 애니메이션 트리거
        anim.SetTrigger("attack1");

        // 2초 대기 후 Attack2_razer 프리팹 생성
        yield return new WaitForSeconds(2f);
        GameObject attack2Instance = Instantiate(attack2Prefab, attack2point.position, Quaternion.Euler(0, 0, 90));
        Debug.Log("attack2 boss");

        // 1.5초 뒤 idle 상태로 전환
        yield return new WaitForSeconds(1.5f);
        anim.SetTrigger("idle"); // idle 애니메이션 트리거
        Destroy(attack2Instance); // 공격 이펙트 제거
        isAttacking = false;
    }

    #endregion

    #region Attack3
    private IEnumerator StartAttack3Sequence()
    {
        // 공격 위치 변수
        Vector3 attackPosition;
        GameObject attackInstance;

        // 보스 위치를 기준으로 공격 위치 결정 및 Attack3 경고 생성
        if (Teleport_pos == 2 || Teleport_pos == 3)
        {
            attackPosition = new Vector3(transform.position.x + 5f, transform.position.y, transform.position.z);
            attackInstance = Instantiate(attack3Prefab, attackPosition, Quaternion.Euler(0f, 180f, 0f));
            //attackInstance.transform.eulerAngles = new Vector3(attackInstance.transform.eulerAngles.x, 180f, attackInstance.transform.eulerAngles.z);
        }
        else
        {
            attackPosition = new Vector3(transform.position.x - 5f, transform.position.y, transform.position.z);
            attackInstance = Instantiate(attack3Prefab, attackPosition, Quaternion.Euler(0f, 0f, 0f));
        }

        yield return StartCoroutine(Attack3Sequence(attackInstance, attackPosition));
    }

    private IEnumerator Attack3Sequence(GameObject attack3Instance, Vector3 attackPosition)
    {
        // 3초 기다리기
        yield return new WaitForSeconds(2f);

        // Attack3경고 제거
        Destroy(attack3Instance);

        anim.SetTrigger("attack2"); // 공격모션 실행
        audioSource.PlayOneShot(attack3Sound); // 공격 효과음 실행

        //이동 이펙트
        Vector3 Move_effect_pos = new Vector3(transform.position.x, transform.position.y - 2f, transform.position.z);
        Instantiate(Boss_Move_Effect, Move_effect_pos, Quaternion.identity);

        // 보스를 Attack3 위치로 빠르게 이동
        float step = 0f;
        while (Vector3.Distance(transform.position, attackPosition) > 0.1f)
        {
            step += approachSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, attackPosition, step);
            yield return null;
        }

        // 다시 원래 위치로 이동
        step = 0f;
        while (Vector3.Distance(transform.position, originalPosition) > 0.1f)
        {
            step += approachSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, step);
            isAttacking = false;
            yield return null;
        }
    }
    #endregion

    #region Attack4
    private IEnumerator StartAttack4Sequence()
    {
        // 공격 위치 변수
        Vector3 attackPosition;
        GameObject attackInstance;
        GameObject Warning_Mark;

        if (attack4_cnt == 0) // Warning_Mark띄우기
        {
            if (Teleport_pos == 2 || Teleport_pos == 3)
            {
                attackPosition = new Vector3(transform.position.x + 7f, transform.position.y + 3.6f, transform.position.z);
                Warning_Mark = Instantiate(Warning_Attack, attackPosition, Quaternion.Euler(0f, 0f, 0f));
            }
            else
            {
                attackPosition = new Vector3(transform.position.x - 7f, transform.position.y + 3.6f, transform.position.z);
                Warning_Mark = Instantiate(Warning_Attack, attackPosition, Quaternion.Euler(0f, 0f, 0f));
            }
        }


        // 보스 위치를 기준으로 공격 위치 결정 및 Attack3 경고 생성
        if (Teleport_pos == 2 || Teleport_pos == 3)
        {
            attackPosition = new Vector3(transform.position.x + 5f, transform.position.y, transform.position.z);
            attackInstance = Instantiate(attack3Prefab, attackPosition, Quaternion.Euler(0f, 180f, 0f));
            //attackInstance.transform.eulerAngles = new Vector3(attackInstance.transform.eulerAngles.x, 180f, attackInstance.transform.eulerAngles.z);
        }
        else
        {
            attackPosition = new Vector3(transform.position.x - 5f, transform.position.y, transform.position.z);
            attackInstance = Instantiate(attack3Prefab, attackPosition, Quaternion.Euler(0f, 0f, 0f));
        }

        yield return StartCoroutine(Attack4Sequence(attackInstance, attackPosition));
    }

    private IEnumerator Attack4Sequence(GameObject attack3Instance, Vector3 attackPosition)
    {
        // 1초 기다리기
        if (attack4_cnt == 0)
        {
            yield return new WaitForSeconds(1.5f);
        }
        else { yield return new WaitForSeconds(0.1f); }

        // Attack3경고 제거
        Destroy(attack3Instance);

        anim.SetTrigger("attack2"); // 공격모션 실행
        audioSource.PlayOneShot(attack3Sound); // 공격 효과음 실행

        //이동 이펙트
        Instantiate(Boss_Move_Effect, transform.position, Quaternion.identity);

        // 보스를 Attack3 위치로 빠르게 이동
        float step = 0f;
        while (Vector3.Distance(transform.position, attackPosition) > 0.1f)
        {
            step += approachSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, attackPosition, step);
            yield return null;
        }

        // 불 효과 남기기
        Vector3 Fire_Pos;

        Fire_Pos = new Vector3(transform.position.x + 7f, transform.position.y - 3f, transform.position.z);
        Instantiate(attack4_Fire, Fire_Pos, Quaternion.Euler(0f, 0f, 0f));
        Fire_Pos = new Vector3(transform.position.x + 4.5f, transform.position.y - 3f, transform.position.z);
        Instantiate(attack4_Fire, Fire_Pos, Quaternion.Euler(0f, 0f, 0f));
        Fire_Pos = new Vector3(transform.position.x + 2f, transform.position.y - 3f, transform.position.z);
        Instantiate(attack4_Fire, Fire_Pos, Quaternion.Euler(0f, 0f, 0f));
        Fire_Pos = new Vector3(transform.position.x - 0.5f, transform.position.y - 3f, transform.position.z);
        Instantiate(attack4_Fire, Fire_Pos, Quaternion.Euler(0f, 0f, 0f));
        Fire_Pos = new Vector3(transform.position.x - 3f, transform.position.y - 3f, transform.position.z);
        Instantiate(attack4_Fire, Fire_Pos, Quaternion.Euler(0f, 0f, 0f));
        Fire_Pos = new Vector3(transform.position.x - 5.5f, transform.position.y - 3f, transform.position.z);
        Instantiate(attack4_Fire, Fire_Pos, Quaternion.Euler(0f, 0f, 0f));

        if (attack4_cnt < 4)
        {
            attack4_cnt++;
            StartCoroutine(StartAttack4Sequence());
            yield return null;
        }
        else
        {
            // 다시 원래 위치로 이동
            step = 0f;
            while (Vector3.Distance(transform.position, originalPosition) > 0.1f)
            {
                step += approachSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, originalPosition, step);
                isAttacking = false;
                yield return null;
            }
        }
    }
    #endregion

    #region Random_Teleport
    private IEnumerator RandomTeleportRoutine()
    {
        Random.InitState(System.DateTime.Now.Millisecond); // 현재 시간을 시드로 설정
        Teleport_pos = Random.Range(0, TeleportPoints.Length);

        if (Teleport_pos == 2 || Teleport_pos == 3)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180f, transform.eulerAngles.z);
        }
        else { transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0f, transform.eulerAngles.z); }

        transform.position = TeleportPoints[Teleport_pos].position;
        originalPosition = TeleportPoints[Teleport_pos].position;
        isAttacking = false;

        Debug.Log("teleport_Num:" + Teleport_pos);

        yield return new WaitForSeconds(1f); // 대기 시간
    }
    #endregion

    // Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
    public virtual void UpdateHPBar()
    {
        if (hpBarTransform != null)
        {
            float hpPercentage = hp / maxHp; // 현재 HP를 최대 HP로 나눈 비율

            if (hpPercentage < 0)
            {
                hpPercentage = 0;
            }
            hpBarTransform.localScale = new Vector3(hpPercentage, 1f, 1f); // X 스케일 조정
        }
    }

    public virtual void Damaged(float amount)
    {
        if (isInvincible) return; // 무적 상태일 때는 데미지를 받지 않음

        Debug.Log("Monster is taking damage: " + amount);
        anim.SetTrigger("hit"); // 히트 애니메이션 실행

        Vector3 spawnPosition = transform.position + new Vector3(5f, -2.5f, 0);
        if (Teleport_pos == 2 || Teleport_pos == 3)
        {
            spawnPosition = transform.position + new Vector3(-5f, -2.5f, 0);
            // Y축을 180도로 회전하여 생성
            Instantiate(Hitted_Effect, spawnPosition, Quaternion.Euler(0f, 180f, -90f));
        }
        else
        {

            // 기본 회전으로 생성
            Instantiate(Hitted_Effect, spawnPosition, Quaternion.Euler(0f, 0f, -90f));
        }

        hp -= amount;

        UpdateHPBar();

        if (hp <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine(0.1f)); // 0.1초 동안 데미지 안받음 중복방지
        }

    }
    private IEnumerator InvincibilityCoroutine(float duration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }

    private void Die()
    {
        if (isDying) return; // 이미 죽음 상태라면 중복 호출 방지

        Debug.Log("Monster is dying");
        isDying = true; // 죽음 상태 설정
        anim.SetTrigger("death"); // 죽음 애니메이션 시작
        PlayerController player = Object.FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            player.ExpUp(experiencePoints); // 경험치 전달
        }

        // ResultUI
        resultUI.gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name);

        // 충돌한 오브젝트가 "AttackedRange"인지 확인
        if (collision.gameObject.name == "AttackedRange")
        {
            // 부모 오브젝트에서 PlayerController를 가져오기
            PlayerController playerController = collision.transform.parent.GetComponent<PlayerController>();

            if (playerController != null)
            {
                // 데미지 처리
                playerController.Damaged();
                Debug.Log("Player damaged by AttackedRange");
            }
            else
            {
                Debug.LogWarning("PlayerController not found on the parent object.");
            }
        }
    }
}