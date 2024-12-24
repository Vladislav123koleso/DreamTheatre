using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour // �������� ����������� �����
{
    public List<basePers> characters = new List<basePers>();
    int currentTurnIndex = 0;

    public static TurnManager Instance { get; private set; }


    private void Awake()
    {
        // ���������, ����� ��������� ��� ������������
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("������������� ��������� TurnManager ������! �����������.");
            Destroy(gameObject);
        }
    }


    public bool IsNewTurn { get; private set; } // ��� ������������ ������ ����

    /*private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f); // �������� ��������, ����� ��������� ������ ������������

        InitializeCharacters();
        StartTurn();
    }*/
    public void InitializeCharacters()
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
    

    public void StartTurn()
    {
        IsNewTurn = false;

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
            if(currentCharacter._enemyType == basePers.enemyType.Knight)
            {
                Debug.Log("��� �����: " + currentCharacter.name + " ��� �����: " + currentCharacter._enemyType);
                // ����� ������ �����
                CharacterController currentCharctContrl = FindControllerByName(currentCharacter.name);
                // ������� ������ ����� (���������� ������)
                List<CharacterController> playerTargets = new List<CharacterController>();

                // �������� �� ���� ����������
                foreach (var playerPersData in characters)
                {
                    if (playerPersData.isPlayer) // ���� ��� �������� ������
                    {
                        CharacterController playerCharctContrl = FindControllerByName(playerPersData.name);
                        if (playerCharctContrl != null)
                        {
                            playerTargets.Add(playerCharctContrl); // ��������� � ������ ��������� �����
                        }
                    }
                }

                // ���� ������ ����� �� ������, �������� ��������� ����
                if (playerTargets.Count > 0)
                {
                    int randomIndex = Random.Range(0, playerTargets.Count); // �������� ��������� ������
                    CharacterController randomTarget = playerTargets[randomIndex]; // �������� ��������� ����

                    // ������� ��������� ����
                    randomTarget.TakeDamage(currentCharctContrl.dmgEnemyKnight());
                    Debug.Log($"{currentCharacter.name} �������� {randomTarget.persData.name}");
                }
                else
                {
                    Debug.LogWarning("������ ����� ����! ���������� ��������� �����.");
                }
            }
            StartCoroutine(CoroTurnEnemy());
            
        }
    }

    public void EndTurn()
    {
        IsNewTurn = true;

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

    // ����� �������� ���� �����
    private IEnumerator CoroTurnEnemy()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("���� �������");
        EndTurn();
    }


    // �������� ��������� ��� ������ ���
    public basePers GetActiveCharacter()
    {
        return characters.FirstOrDefault(c => c.hasTurn); // ������� ���������, � �������� ������� ���� hasTurn
    }






    // ����� ��� ������ CharacterController �� �����
    private CharacterController FindControllerByName(string name)
    {
        // �������� ���� ������ �� �����
        CharacterController[] allControllers = FindObjectsOfType<CharacterController>();

        // ���� ����� � ������ ������
        return allControllers.FirstOrDefault(controller => controller.persData.name == name);
    }


    // �������� ����� ����� ������
    public void RemoveCharacter(basePers character)
    {
        characters.Remove(character);
        Debug.Log($"{character.name} ������ �� ������� �����.");

        // ��������� ������ �������� ���������
        if (characters.Count > 0)
        {
            currentTurnIndex %= characters.Count;
        }
    }
}
