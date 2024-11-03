using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonsterController
{
    protected override void Awake()
    {
        base.Awake(); // 부모 클래스의 Awake 메서드 호출
        FirstAnim = "idletofly";
        attackAnim = "bite"; // BatController의 공격 애니메이션 이름 설정
        idleAnim = "fly";
        runAnim = "fly";
    }
    protected override void Patrol()
    {
        // Bat 클래스는 자동 patrol 기능을 사용하지 않음
        // 이 메서드는 빈 상태로 둡니다.
    }
    //protected override void OnDrawGizmos()
    //{
    //    // Bat 클래스는 자동 patrol 기능을 사용하지 않음
    //    // 이 메서드는 빈 상태로 둡니다.
    //}


}
