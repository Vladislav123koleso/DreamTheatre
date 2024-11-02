using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class CharacterController : MonoBehaviour
{
    public basePers persData;


    // ћетод дл€ инициализации персонажа с его данными
    public void InitializeCharacter(basePers character)
    {
        persData.name = character.name;
        persData.hp = character.hp;
        persData.speed = character.speed;
        persData.damage = character.damage;
        persData.protection = character.protection;
        persData.hasTurn = character.hasTurn;
        persData.isPlayer = character.isPlayer;
    }
}
