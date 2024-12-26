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
    public GameObject magPrefab;
    public GameObject enemyKnightPrefab;
    public GameObject enemyTraderPrefab;
    public GameObject enemyVuchnikPrefab;

    public GameObject mainPers;
    private List<basePers> playerCharacters; // ������ ���������� ������

    public GameManager gameManager;

    public static spawnManager Instance { get; private set; }

    // ������ ����������
    [Header("icons knight")]
    public Sprite[] abilityIconsKnight;
    [Header("icons mag")]
    public Sprite[] abilityIconsMag;
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
            basePers newMainPers = new basePers("MainPers" + 1, 40, 4, 6,basePers.enemyType.None, false, true, 8,7,12,8);
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

    public void SpawnEnemyCharacters()
    {
        StartCoroutine(waitwait());
        if(gameManager.battleCounter == 0)
        {
            
                // ������� ������ ����� 
                GameObject enemy = Instantiate(enemyKnightPrefab, enemySpawnPoints[0].position, Quaternion.identity);
                GameObject enemy1 = Instantiate(enemyTraderPrefab, enemySpawnPoints[1].position, Quaternion.identity);

                // ������ ���������� ��������� ��� ������
                enemy.GetComponent<CharacterController>().InitializeCharacter(CreateRandomKnightEnemy(0 + 1));
                enemy1.GetComponent<CharacterController>().InitializeCharacter(CreateRandomTraderEnemy(1 + 1));
            
        }
        else if(gameManager.battleCounter == 1)
        {
            // ������� ������ ����� 
            GameObject enemy = Instantiate(enemyKnightPrefab, enemySpawnPoints[0].position, Quaternion.identity);
            GameObject enemy1 = Instantiate(enemyTraderPrefab, enemySpawnPoints[1].position, Quaternion.identity);
            GameObject enemy2 = Instantiate(enemyVuchnikPrefab, enemySpawnPoints[2].position, Quaternion.identity);

            // ������ ���������� ��������� ��� ������
            enemy.GetComponent<CharacterController>().InitializeCharacter(CreateRandomKnightEnemy(0 + 1));
            enemy1.GetComponent<CharacterController>().InitializeCharacter(CreateRandomTraderEnemy(1 + 1));
            enemy2.GetComponent<CharacterController>().InitializeCharacter(CreateRandomVuchnikEnemy(2 + 1));

        }

        /*for (int i = 0; i < enemySpawnPoints.Length; i++)
        {
            // ������� ������ ����� �� ������ ����� ������
            GameObject enemy = Instantiate(enemyPrefab, enemySpawnPoints[i].position, Quaternion.identity);

            // ����� ������ ���������� ��������� ��� ������
            enemy.GetComponent<CharacterController>().InitializeCharacter(CreateRandomEnemy(i + 1));
        }*/
    }


    private IEnumerator waitwait()
    {
        yield return new WaitForSeconds(1);
    }


    
    basePers CreateRandomKnightEnemy(int numberEnemy)
    {
        return new basePers("Enemy" + numberEnemy, Random.Range(25, 30), Random.Range(2, 5), Random.Range(5, 6), basePers.enemyType.Knight, false, false,0, 1,5, Random.Range(1, 8));
    }
    basePers CreateRandomTraderEnemy(int numberEnemy)
    {
        return new basePers("EnemyTrader" + numberEnemy, Random.Range(15, 20), Random.Range(2, 5), Random.Range(5, 6), basePers.enemyType.Slave, false, false, 0, 3, 7, Random.Range(1, 4));
    }
    basePers CreateRandomMag(int number)
    {
        return new basePers("Mag" + number, 50, 7, 2, basePers.enemyType.Mag, false, true, 14, 5, 20, 5, abilityIconsMag);
    }
    basePers CreateRandomVuchnikEnemy(int numberEnemy)
    {
        return new basePers("EnemyVuchnik" + numberEnemy, Random.Range(15, 17), Random.Range(10, 15), Random.Range(2, 4), basePers.enemyType.Vuchnik, false, false, 2, 7, 13, Random.Range(6, 10));
    }

    public void spawnMag()
    {
        GameObject mag = Instantiate(magPrefab, playerSpawnPoints[2].position, Quaternion.identity);

        
        mag.GetComponent<CharacterController>().InitializeCharacter(CreateRandomMag(0 + 1));
    }
}
