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

    public static spawnManager Instance { get; private set; }

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
            basePers newMainPers = new basePers("MainPers" + 1, 20, 4, 6,basePers.enemyType.None, false, true, 8,6,8,8);
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
        return new basePers("Enemy" + numberEnemy, Random.Range(12, 15), Random.Range(2, 5), Random.Range(5, 6), basePers.enemyType.Knight, false, false,0, 5,7, Random.Range(1, 8));
    }

    
}
