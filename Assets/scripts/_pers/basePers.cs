using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

[Serializable]
public class basePers
{
    public string name;

    public int hp;// ���
    public int speed;// ��������
    public int damage;// ������� ����
    public int protection; // ������

    public bool hasTurn;  // ����������, ��� ��� ���
    public bool isPlayer; // ���������� ��� �������� ������

    public Light2DBase persLight;

    public basePers(string name, int hp, int speed, int damage, int protection, bool hasTurn = false,bool isPlayer = false)
    {
        this.name = name; 
        this.hp = hp; 
        this.speed = speed; 
        this.damage = damage; 
        this.protection = protection;

        this.hasTurn = false; 
        this.isPlayer = isPlayer; 
    }


    public void SetLight(bool isActive)
    {
        if (persLight != null)
        {
            persLight.enabled = isActive;
        }
    }
}
