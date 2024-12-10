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
    public GameObject enemyPrefab;

    public GameObject mainPers;
    private List<basePers> playerCharacters; // список персонажей игрока

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
            basePers newMainPers = new basePers("MainPers" + 1, 20, 4, 6, false, true, 8,6,8,8);
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

    void SpawnEnemyCharacters()
    {
        for (int i = 0; i < enemySpawnPoints.Length; i++)
        {
            // Создаем префаб врага на каждой точке спауна
            GameObject enemy = Instantiate(enemyPrefab, enemySpawnPoints[i].position, Quaternion.identity);

            // Здесь задаем уникальные параметры для врагов
            enemy.GetComponent<CharacterController>().InitializeCharacter(CreateRandomEnemy(i+1));
        }
    }



    // Метод для создания врагов с случайными параметрами 
    // P.S. временно для блокинга
    basePers CreateRandomEnemy(int numberEnemy)
    {
        return new basePers("Enemy" + numberEnemy, Random.Range(30, 200), Random.Range(40, 120), Random.Range(5, 15), false, false, Random.Range(5, 60));
    }

    
}
