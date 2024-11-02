using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour // �������� ����������� �����
{
    public List<basePers> characters = new List<basePers>();
    int currentTurnIndex = 0;

    private void Start()
    {

        // ���������� ���������� �� ��������
        characters = characters.OrderByDescending(c => c.speed).ToList();
        
        StartTurn();
    }

    void StartTurn()
    {
        var currentCharacter = characters[currentTurnIndex]; // ������� ��������

        currentCharacter.hasTurn = true; // ��������� ���� ���������

        if(currentCharacter.isPlayer) // ���� �������� ������
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
