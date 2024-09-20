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

    private List<basePers> playerCharacters; // ������ ���������� ������

    private void Start()
    {
        // ��������� ������ ���������� �� ����
        playerCharacters = PlayerTeamSelection.selectedCharacters;
        
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
            player.GetComponent<CharacterController>().InitializeCharacter(playerCharacters[i]);
        }
    }

    void SpawnEnemyCharacters()
    {
        for (int i = 0; i < enemySpawnPoints.Length; i++)
        {
            // ������� ������ ����� �� ������ ����� ������
            GameObject enemy = Instantiate(enemyPrefab, enemySpawnPoints[i].position, Quaternion.identity);

            // ����� ������ ���������� ��������� ��� ������
            enemy.GetComponent<CharacterController>().InitializeCharacter(CreateRandomEnemy());
        }
    }



    // ����� ��� �������� ������ � ���������� ����������� 
    // P.S. �������� ��� ��������
    basePers CreateRandomEnemy()
    {
        return new basePers("Enemy" + Random.Range(1, 100), Random.Range(40, 120), Random.Range(20, 300), Random.Range(5, 15), Random.Range(5, 60), false);
    }
}
