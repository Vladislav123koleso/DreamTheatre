using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class CharacterController : MonoBehaviour
{
    public basePers persData;



    // Метод для инициализации персонажа с его данными
    public void InitializeCharacter(basePers character)
    {
        persData = character;
        persData.InitializeLight(transform);
    }


    public void TakeDamage(int damage)
    {
        persData.hp -= damage;
        if (persData.hp <= 0)
        {
            // Логика смерти персонажа
            Debug.Log($"{persData.name} повержен!");
        }
    }
}
