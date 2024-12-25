using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Rendering.Universal;
using Unity.Mathematics;

[Serializable]
public class basePers
{
    public string name;

    public int hp;//текущее хп
    public int max_hp;//макс хп
    public int minDamage;// минимальный базовый урон
    public int maxDamage;// максимальный базовый урон
    public int critChance; // шанс крит попадания
    public int speed;// скорость
    public int protection; // защита
    public int magicProtection; // магическая защита
    
    // сопротивления эффектам 

    //

    public bool hasTurn;  // Показывает, чей это ход
    public bool isPlayer; // показывает что персонаж игрока

    public Light2D persLight;

    public enum enemyType
    {
        None,
        Knight,
        Slave,
        Vuchnik


    }

    public enemyType _enemyType;



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

    public basePers(string name, int hp, int speed, int protection, enemyType _enemyType, bool hasTurn = false,bool isPlayer = false, int magicProtection = 0, int minDamage = 4, int maxDamage = 5, int critChance = 2)
    {
        this.name = name; 
        this.hp = hp; 
        this.max_hp = hp; 
        this.speed = speed; 
        this.protection = protection;
        this.magicProtection = magicProtection;
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
        this.critChance = critChance;

        this.hasTurn = false; 
        this.isPlayer = isPlayer;
        this._enemyType = _enemyType;
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
    //

    // Метод для сброса света к изначальному цвету
    public void ResetLightColor()
    {
        SetLightColor(new Color32(0xFE, 0xFF, 0xCB, 0xFF)); // дефолтный цвет
    }


    // метод подсчета наносимого урона
    public int CalculateDamage()
    {
        bool isCriticalHit = CheckCriticalHit();
        int baseDamage = UnityEngine.Random.Range(minDamage, maxDamage);
        
        if(isCriticalHit)
        {
            Debug.Log("Нанесен крит урон!");
            return baseDamage * 2;
        }
        else
        {
            Debug.Log("Нанесен урон!");
            return baseDamage;
        }
    }
    //проверка что попадание критическое
    private bool CheckCriticalHit()
    {
        int randomValue = UnityEngine.Random.Range(0,100);

        return randomValue <= critChance; 
    }




}
