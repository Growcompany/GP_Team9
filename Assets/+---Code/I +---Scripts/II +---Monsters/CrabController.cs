using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabController : MonsterController
{
    protected override void Awake()
    {
        base.Awake(); // 부모 클래스의 Awake 호출
        experiencePoints = 15; // Crab의 경험치
        FlipSprite = false;
        attackAnim = "attackC"; // Crab의 공격 애니메이션 이름
        idleAnim = "idle";
        runAnim = "run";
    }

}
