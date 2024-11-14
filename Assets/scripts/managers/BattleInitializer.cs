using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInitializer : MonoBehaviour
{
    //public spawnManager spawnManager;
    public FadeInOut fadeInOut;
    public Camera mainCamera;
    public TurnManager turnManager;
    public GameObject abilitiesPanel; // Панель с активными способностями

    private IEnumerator Start()
    {
        // Спавн персонажей
        //spawnManager.SpawnPlayerCharacters();
        //spawnManager.SpawnEnemyCharacters();

        // Ждем 3 секунды после спавна
        yield return new WaitForSeconds(3);
        //
        
        yield return StartCoroutine(fadeInOut.FadeOut());
        yield return new WaitForSeconds(1);
        abilitiesPanel.SetActive(true);
        yield return StartCoroutine(fadeInOut.FadeIn());

        // Фокусировка камеры
        FocusCameraOnBattle();

        // Включаем TurnManager(начинается бой)
        turnManager.InitializeCharacters();
        turnManager.StartTurn();
    }

    void FocusCameraOnBattle()
    {
        // Логика приближения камеры к бою 
        mainCamera.transform.position = new Vector3(0, 0, -10); // пример позиции
    }
}
