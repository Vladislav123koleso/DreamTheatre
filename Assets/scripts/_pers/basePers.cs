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

    public int hp;// имя
    public int speed;// скорость
    public int damage;// базовый урон
    public int protection; // защита

    public bool hasTurn;  // Показывает, чей это ход
    public bool isPlayer; // показывает что персонаж игрока

    public Light2DBase persLight;

    // Инициализация источника света
    public void InitializeLight(Transform characterTransform)
    {
        // Ищем компонент Light2D в дочерних объектах персонажа
        persLight = characterTransform.GetComponentInChildren<Light2DBase>();
        if (persLight != null)
        {
            persLight.enabled = false;
        }
    }

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
