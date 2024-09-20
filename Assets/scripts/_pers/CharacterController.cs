using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class CharacterController : MonoBehaviour
{
    //public basePers persData;

    public string characterName;
    public int hp;
    public int mp;
    public int damage;
    public int speed;
    public bool isPlayer;

    // Метод для инициализации персонажа с его данными
    public void InitializeCharacter(basePers character)
    {
        characterName = character.name;
        speed = character.speed;
        damage = character.damage;
        hp = character.hp;
        mp = character.mp;
        isPlayer = character.isPlayer;
        
        Debug.Log($"Персонаж {characterName} со скоростью {speed},уроном {damage},хп {hp} был инициализирован.");
    }
}
