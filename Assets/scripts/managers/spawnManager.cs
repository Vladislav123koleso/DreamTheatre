using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class spawnManager : MonoBehaviour // �������� ������ ���������� � ������ ���
{
    // ����� ������
    public Transform[] playerSpawnPoints;
    public Transform[] enemySpawnPoints;

    // �������
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public GameObject mainPers;
    private List<basePers> playerCharacters; // ������ ���������� ������

    private void Start()
    {
        // ��������� ������ ���������� �� ����
        playerCharacters = PlayerTeamSelection.selectedCharacters;
        //���� ������ ����
        if (playerCharacters == null || playerCharacters.Count == 0)
        {
            playerCharacters = new List<basePers>();

            // �������� ������ �������� ��������� ����� ��� CharacterController
            basePers mainCharacterData = mainPers.GetComponent<CharacterController>().persData;
            //deleteme
            basePers newMainPers = new basePers("GG" + Random.Range(1, 100), Random.Range(30, 200), Random.Range(40, 120), Random.Range(5, 15), Random.Range(5, 60), false, true);
            mainPers.GetComponent<CharacterController>().InitializeCharacter(newMainPers);
            //
            playerCharacters.Add(mainCharacterData); // ��������� �������� ��������� � ������
        }
        
        SpawnPlayerCharacters(); //����� ���������� ������ 

        SpawnEnemyCharacters(); // ����� ������
    }

    void SpawnPlayerCharacters()
    {
        for (int i = 0; i < playerCharacters.Count; i++)
        {
            // ������� ������ ������ �� ����� ������
            GameObject player = Instantiate(playerPrefab, playerSpawnPoints[i].position, Quaternion.identity);
            
            // �������� ������ ��������� � ������
            //player.GetComponent<CharacterController>().InitializeCharacter(playerCharacters[i]);
        }
    }

    void SpawnEnemyCharacters()
    {
        for (int i = 0; i < enemySpawnPoints.Length; i++)
        {
            // ������� ������ ����� �� ������ ����� ������
            GameObject enemy = Instantiate(enemyPrefab, enemySpawnPoints[i].position, Quaternion.identity);

            // ����� ������ ���������� ��������� ��� ������
            enemy.GetComponent<CharacterController>().InitializeCharacter(CreateRandomEnemy(i+1));
        }
    }



    // ����� ��� �������� ������ � ���������� ����������� 
    // P.S. �������� ��� ��������
    basePers CreateRandomEnemy(int numberEnemy)
    {
        return new basePers("Enemy" + numberEnemy, Random.Range(30, 200), Random.Range(40, 120), Random.Range(5, 15), Random.Range(5, 60), false, false);
    }

    
}
