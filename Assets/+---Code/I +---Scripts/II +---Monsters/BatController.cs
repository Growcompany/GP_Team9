using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonsterController
{
    protected override void Awake()
    {
        base.Awake(); // �θ� Ŭ������ Awake �޼��� ȣ��
        experiencePoints = 7; // Bat�� ����ġ
        FirstAnim = "idletofly";
        attackAnim = "bite"; // BatController�� ���� �ִϸ��̼� �̸� ����
        idleAnim = "fly";
        runAnim = "fly";
    }
    protected override void Patrol()
    {
        // Bat Ŭ������ �ڵ� patrol ����� ������� ����
        // �� �޼���� �� ���·� �Ӵϴ�.
    }


}
