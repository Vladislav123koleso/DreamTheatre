using System.Collections;
using UnityEngine;

public class BattleInitializer : MonoBehaviour
{
    public FadeInOut fadeInOut;
    public Camera mainCamera;
    public TurnManager turnManager;
    public GameObject abilitiesPanel; // ѕанель с активными способност€ми

    private IEnumerator Start()
    {
        // —павн персонажей (раскомментируйте при необходимости)
        //spawnManager.SpawnPlayerCharacters();
        //spawnManager.SpawnEnemyCharacters();

        // ∆дем 3 секунды после спавна
        yield return new WaitForSeconds(3);

        // Ёффект затемнени€
        yield return StartCoroutine(fadeInOut.FadeOut());
        yield return new WaitForSeconds(1);

        abilitiesPanel.SetActive(true);

        // ‘окусировка камеры
        StartCoroutine(FocusCameraOnBattle());
        
        yield return StartCoroutine(fadeInOut.FadeIn());


        // ¬ключаем TurnManager (начинаетс€ бой)
        turnManager.InitializeCharacters();
        turnManager.StartTurn();
    }

    IEnumerator FocusCameraOnBattle()
    {
        float targetSize = 5f; // ÷елевой размер камеры дл€ приближени€
        float duration = 0f; // ƒлительность приближени€
        float initialSize = mainCamera.orthographicSize;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            mainCamera.orthographicSize = Mathf.Lerp(initialSize, targetSize, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.orthographicSize = targetSize;
    }
}
