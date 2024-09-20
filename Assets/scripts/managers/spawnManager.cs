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

    private List<basePers> playerCharacters; // список персонажей игрока

    private void Start()
    {
        // Загружаем список персонажей из хаба
        playerCharacters = PlayerTeamSelection.selectedCharacters;
        
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
            player.GetComponent<CharacterController>().InitializeCharacter(playerCharacters[i]);
        }
    }

    void SpawnEnemyCharacters()
    {
        for (int i = 0; i < enemySpawnPoints.Length; i++)
        {
            // Создаем префаб врага на каждой точке спауна
            GameObject enemy = Instantiate(enemyPrefab, enemySpawnPoints[i].position, Quaternion.identity);

            // Здесь задаем уникальные параметры для врагов
            enemy.GetComponent<CharacterController>().InitializeCharacter(CreateRandomEnemy());
        }
    }



    // Метод для создания врагов с случайными параметрами 
    // P.S. временно для блокинга
    basePers CreateRandomEnemy()
    {
        return new basePers("Enemy" + Random.Range(1, 100), Random.Range(40, 120), Random.Range(20, 300), Random.Range(5, 15), Random.Range(5, 60), false);
    }
}
