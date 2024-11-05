using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabController : MonsterController
{
    protected override void Awake()
    {
        base.Awake(); // �θ� Ŭ������ Awake �޼��� ȣ��
        experiencePoints = 15; // Crab�� ����ġ
        FlipSprite = false;
        attackAnim = "attackC"; // CrabController�� ���� ����
        idleAnim = "idle";
        runAnim = "run";
    }

}
