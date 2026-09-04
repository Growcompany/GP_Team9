using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonsterController
{
    protected override void Awake()
    {
        base.Awake(); // 부모 클래스의 Awake 호출
        experiencePoints = 7; // Bat의 경험치
        FirstAnim = "idletofly";
        attackAnim = "bite"; // Bat의 공격 애니메이션 이름
        idleAnim = "fly";
        runAnim = "fly";
    }
    protected override void Patrol()
    {
        // Bat은 순찰 기능을 사용하지 않음
        // 빈 메서드로 둠
    }


}
