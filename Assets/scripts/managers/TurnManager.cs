using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour // �������� ����������� �����
{
    public List<basePers> characters = new List<basePers>();
    int currentTurnIndex = 0;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f); // �������� ��������, ����� ��������� ������ ������������

        InitializeCharacters();
        StartTurn();
    }
    void InitializeCharacters()
    {
        // �������� ��� ������� ����������, ����������� �� �����
        CharacterController[] allCharacters = FindObjectsOfType<CharacterController>();
        // ��������� ������� ��������� � ������
        foreach (var character in allCharacters)
        {
            characters.Add(character.persData);
        }

        // ���������� ���������� �� ��������
        characters = characters.OrderByDescending(c => c.speed).ToList();
    }
    

    void StartTurn()
    {
        var currentCharacter = characters[currentTurnIndex]; // ������� ��������
        currentCharacter.hasTurn = true; // ��������� ���� ���������
        currentCharacter.SetLight(true); // �������� ����� ��� ������� ��� �������� ���������


        if (currentCharacter.isPlayer) // ���� �������� ������
        {
            Debug.Log("��� ���������: " + currentCharacter.name);
            Debug.Log("�������� ������� ������ ����� ����");
        }
        else
        {
            Debug.Log("��� �����: " + currentCharacter.name);
            Debug.Log("���� �������");
            EndTurn();
        }
    }

    public void EndTurn()
    {
        characters[currentTurnIndex].hasTurn = false; // ����� ���� ���������
        characters[currentTurnIndex].SetLight(false);
        currentTurnIndex = (currentTurnIndex + 1) % characters.Count; // ��������� ������� ���� � ���������� ���������

        StartTurn();
    }


    // ���������� ���� ��������� (������)
    public void OnEndPlayerTurnButton()
    {
        // ���� ������ ��� ������
        if (characters[currentTurnIndex].isPlayer)
        {
            EndTurn();
        }
    }

}
