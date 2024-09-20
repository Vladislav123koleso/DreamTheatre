using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class basePers
{
    public string name;

    public int hp;
    public int mp;
    public int speed;
    public int damage;

    public bool hasTurn;  // Показывает, чей это ход
    public bool isPlayer; // показывает что персонаж игрока

    public basePers(string name, int hp, int mp, int speed, int damage, bool hasTurn)
    {
        this.name = name;
        this.hp = hp;
        this.mp = mp;
        this.speed = speed;
        this.damage = damage;
        this.hasTurn = false;
        this.isPlayer = isPlayer;
    }
}
