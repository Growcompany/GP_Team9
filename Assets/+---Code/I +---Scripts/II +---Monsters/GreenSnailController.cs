using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenSnailController : MonsterController
{
    protected override void Awake()
    {
        base.Awake(); // 부모 클래스의 Awake 메서드 호출
        experiencePoints = 50; // GreenSnail의 경험치
        FlipSprite = false;
        attackAnim = "attack"; // GreenSnail의 공격 설정
        idleAnim = "idle";
        runAnim = "run";
    }


}
