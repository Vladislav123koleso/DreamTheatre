using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyKnightLogic : MonoBehaviour
{
    CharacterController enemy;
    basePers enemyData;
    private void Start()
    {
        enemy = this.GetComponent<CharacterController>();
        enemyData = enemy.persData;
    }


    //����� ����� ����� �� 1��� ��������� ������
    public void attack()
    {

    }
}
