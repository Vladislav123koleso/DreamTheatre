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

    // ����� ��� ������������� ��������� � ��� �������
    public void InitializeCharacter(basePers character)
    {
        characterName = character.name;
        speed = character.speed;
        damage = character.damage;
        hp = character.hp;
        mp = character.mp;
        isPlayer = character.isPlayer;
        
        Debug.Log($"�������� {characterName} �� ��������� {speed},������ {damage},�� {hp} ��� ���������������.");
    }
}
