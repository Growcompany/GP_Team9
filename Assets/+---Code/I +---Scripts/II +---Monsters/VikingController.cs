using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingController : MonsterController
{
    protected override void Awake()
    {
        base.Awake(); // �θ� Ŭ������ Awake �޼��� ȣ��
        experiencePoints = 10; // Bat�� ����ġ
        FlipSprite = false;
        attackAnim = "attackA"; // VikingController ���� ����
        idleAnim = "idle";
        runAnim = "walk";
    }

}
