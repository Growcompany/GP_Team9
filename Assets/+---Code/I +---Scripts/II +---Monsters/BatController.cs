using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonsterController
{
    protected override void Awake()
    {
        base.Awake(); // 부모 클래스의 Awake 메서드 호출
        experiencePoints = 50; // Bat의 경험치
        FirstAnim = "idletofly";
        attackAnim = "bite"; // BatController의 공격 애니메이션 이름 설정
        idleAnim = "fly";
        runAnim = "fly";
    }

}
