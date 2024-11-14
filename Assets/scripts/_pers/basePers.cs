using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Rendering.Universal;

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

    public Light2D persLight;

    // Инициализация источника света
    public void InitializeLight(Transform characterTransform)
    {
        // Ищем компонент Light2D в дочерних объектах персонажа
        persLight = characterTransform.GetComponentInChildren<Light2D>();
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

    // Метод для изменения цвета света над персонажем
    public void SetLightColor(Color color)
    {
        if (persLight != null)
        {
            persLight.color = color;
        }
    }

    // Метод для сброса света к изначальному цвету
    public void ResetLightColor()
    {
        SetLightColor(new Color32(0xFE, 0xFF, 0xCB, 0xFF)); // дефолтный цвет
    }


}
