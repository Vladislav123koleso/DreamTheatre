using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class spawnManager : MonoBehaviour // менеджер спавна персонажей в начале боя
{
    // точки спавна
    public Transform[] playerSpawnPoints;
    public Transform[] enemySpawnPoints;

    // префабы
    public GameObject playerPrefab;
    public GameObject magPrefab;
    public GameObject enemyKnightPrefab;
    public GameObject enemyTraderPrefab;
    public GameObject enemyVuchnikPrefab;

    public GameObject mainPers;
    private List<basePers> playerCharacters; // список персонажей игрока

    public GameManager gameManager;

    public static spawnManager Instance { get; private set; }

    // иконки персонажей
    [Header("icons knight")]
    public Sprite[] abilityIconsKnight;
    [Header("icons mag")]
    public Sprite[] abilityIconsMag;
    private void Start()
    {
        // Загружаем список персонажей из хаба
        playerCharacters = PlayerTeamSelection.selectedCharacters;
        //если список пуст
        if (playerCharacters == null || playerCharacters.Count == 0)
        {
            playerCharacters = new List<basePers>();

            // Получаем данные главного персонажа через его CharacterController
            basePers mainCharacterData = mainPers.GetComponent<CharacterController>().persData;
            //deleteme
            basePers newMainPers = new basePers("MainPers" + 1, 40, 4, 6,basePers.enemyType.None, false, true, 8,7,12,8);
            mainPers.GetComponent<CharacterController>().InitializeCharacter(newMainPers);
            //
            playerCharacters.Add(mainCharacterData); // Добавляем главного персонажа в список
        }
        
        SpawnPlayerCharacters(); //Спавн персонажей игрока 

        SpawnEnemyCharacters(); // Сравн врагов
    }

    void SpawnPlayerCharacters()
    {
        for (int i = 0; i < playerCharacters.Count; i++)
        {
            // Создаем префаб игрока на точке спауна
            GameObject player = Instantiate(playerPrefab, playerSpawnPoints[i].position, Quaternion.identity);
            
            // Передаем данные персонажа в скрипт
            //player.GetComponent<CharacterController>().InitializeCharacter(playerCharacters[i]);
        }
    }

    public void SpawnEnemyCharacters()
    {
        StartCoroutine(waitwait());
        if(gameManager.battleCounter == 0)
        {
            
                // Создаем префаб врага 
                GameObject enemy = Instantiate(enemyKnightPrefab, enemySpawnPoints[0].position, Quaternion.identity);
                GameObject enemy1 = Instantiate(enemyTraderPrefab, enemySpawnPoints[1].position, Quaternion.identity);

                // задаем уникальные параметры для врагов
                enemy.GetComponent<CharacterController>().InitializeCharacter(CreateRandomKnightEnemy(0 + 1));
                enemy1.GetComponent<CharacterController>().InitializeCharacter(CreateRandomTraderEnemy(1 + 1));
            
        }
        else if(gameManager.battleCounter == 1)
        {
            // Создаем префаб врага 
            GameObject enemy = Instantiate(enemyKnightPrefab, enemySpawnPoints[0].position, Quaternion.identity);
            GameObject enemy1 = Instantiate(enemyTraderPrefab, enemySpawnPoints[1].position, Quaternion.identity);
            GameObject enemy2 = Instantiate(enemyVuchnikPrefab, enemySpawnPoints[2].position, Quaternion.identity);

            // задаем уникальные параметры для врагов
            enemy.GetComponent<CharacterController>().InitializeCharacter(CreateRandomKnightEnemy(0 + 1));
            enemy1.GetComponent<CharacterController>().InitializeCharacter(CreateRandomTraderEnemy(1 + 1));
            enemy2.GetComponent<CharacterController>().InitializeCharacter(CreateRandomVuchnikEnemy(2 + 1));

        }

        /*for (int i = 0; i < enemySpawnPoints.Length; i++)
        {
            // Создаем префаб врага на каждой точке спауна
            GameObject enemy = Instantiate(enemyPrefab, enemySpawnPoints[i].position, Quaternion.identity);

            // Здесь задаем уникальные параметры для врагов
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
