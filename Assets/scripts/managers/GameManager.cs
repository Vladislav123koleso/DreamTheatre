using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
//using UnityEditor.VersionControl;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera mainCamera;
    public FadeInOut fadeInOut;
    public GameObject abilitiesPanel; // Панель с активными способностями
    private float defaultCameraSize; // Для хранения дефолтного размера камеры

    public bool isFight = false; // для отслеживания активности боя

    public TextMeshProUGUI introText;  // текст заставки
    public float typingSpeed = 0.05f;     // Скорость появления текста

    public TextMeshProUGUI dialogueText;  // Для вывода текста диалога
    

    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> players = new List<GameObject>();


    public static GameManager Instance { get; private set; }

    private IEnumerator Start()
    {
        
        abilitiesPanel.SetActive(false);
        // Сохраняем дефолтный размер камеры
        defaultCameraSize = mainCamera.orthographicSize;

        // Ждем ... секунды после спавна
        //yield return new WaitForSeconds(0);
        // Эффект затемнения
        yield return StartCoroutine(fadeInOut.FadeOut());
        


        /*
        string intro = "Голос во тьме: Добро пожаловать на сцену Театра смерти, " +
            "здесь вы познаете отчаяние и смерть. Пьесса о вашей смерти будет сыграна здесь";
        yield return StartCoroutine(TypeText(intro, introText));
        */


        // получаем списки персонажей игрока и врагов
        FindCharacters();


        yield return new WaitForSeconds(1);

        yield return StartCoroutine(fadeInOut.FadeIn());
        //-------------------------------

        //---------------------------
        //здесь диалоговая система
        // Переход к диалогу персонажей
        // yield return StartCoroutine(DialogueSequence());


        //----------------------------------
        //после диалога начинается бой
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(fadeInOut.FadeOut());
        yield return StartCoroutine(TypeText("Голос во тьме: Сценка начинается. Выживи если сможешь", introText));
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(fadeInOut.FadeIn());

        abilitiesPanel.SetActive(true);
        StartCoroutine(FocusCameraOnBattle());
        isFight = true;


        StartCoroutine(CheckBattleOutcome());
    }





    private void FindCharacters()
    {
        // Очищаем текущий список врагов (если нужно)
        enemies.Clear();

        // Находим все объекты с тегом "Enemy"
        GameObject[] foundEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Добавляем их в список
        foreach (GameObject enemy in foundEnemies)
        {
            enemies.Add(enemy);
        }

        // Очищаем текущий список врагов (если нужно)
        players.Clear();

        // Находим все объекты с тегом "Enemy"
        GameObject[] foundPlayers = GameObject.FindGameObjectsWithTag("Player");

        // Добавляем их в список
        foreach (GameObject player in foundPlayers)
        {
            players.Add(player);
        }
    }


    private IEnumerator DialogueSequence()
    {
        // Получаем персонажей для диалога
        GameObject player = players[0]; // Первый игрок
        GameObject enemy = enemies[0];  // Первый враг

        // Получаем канвасы персонажей (второй дочерний объект)
        Transform playerCanvas = player.transform.GetChild(1); // Канвас игрока
        Transform enemyCanvas = enemy.transform.GetChild(1);   // Канвас врага

        // Получаем компоненты TextMeshProUGUI
        TextMeshProUGUI playerDialogueText = playerCanvas.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI enemyDialogueText = enemyCanvas.GetComponentInChildren<TextMeshProUGUI>();


        // Диалог 1 (Игрок и 1 из врагов)
        yield return StartCoroutine(TypeText("Игрок: Привет, кто ты?", playerDialogueText));
        yield return new WaitForSeconds(1); // Пауза между диалогами

        yield return StartCoroutine(TypeText("Враг: Я... голос из темноты. Ты здесь не один.", enemyDialogueText));
        yield return new WaitForSeconds(1);

        yield return StartCoroutine(TypeText("Игрок: Что здесь происходит?", playerDialogueText));
        yield return new WaitForSeconds(1);

        yield return StartCoroutine(TypeText("Враг: Это место для твоей смерти...", enemyDialogueText));
        yield return new WaitForSeconds(1);

        // Пауза после диалога
        yield return new WaitForSeconds(1);
        //dialogueText.text = "";
    }


    

    public IEnumerator CheckBattleOutcome()
    {
        while (isFight)
        {
            yield return new WaitForSeconds(1); // Проверяем каждую секунду, чтобы не нагружать процессор

            // Проверка победы
            if (enemies.Count == 0)
            {
                Debug.Log("Игрок победил!");
                yield return StartCoroutine(TypeText("Поздравляем, вы выиграли!", introText));
                EndFight();
                yield return StartCoroutine(DialogueSequence()); // Начинаем новый диалог
                yield break; // Выходим из проверки
            }

            // Проверка поражения
            if (players.Count == 0)
            {
                Debug.Log("Игрок проиграл!");
                yield return StartCoroutine(TypeText("Вы проиграли... Конец игры.", introText));
                EndFight();
                yield break; // Выходим из проверки
            }
        }
    }

    private void EndFight()
    {
        isFight = false;
        abilitiesPanel.SetActive(false); // Скрываем панель способностей
        ResetCameraToDefault(); // Возвращаем камеру к стандартному состоянию
    }

    // Метод обновления списков врагов и игроков после их действий
    public void UpdateCharacterLists()
    {
        // Удаляем мертвых персонажей из списков
        enemies.RemoveAll(enemy => enemy == null || !enemy.activeInHierarchy);
        players.RemoveAll(player => player == null || !player.activeInHierarchy);
    }

    
    private void Update()
    {
        if (isFight)
        {
            UpdateCharacterLists();
        }
        //CheckBattleOutcome();
        
    }






    // вывод текста по букве
    private IEnumerator TypeText(string message, TextMeshProUGUI targetText)
    {
        targetText.text = "";  // Очищаем текст перед набором

        foreach (char letter in message.ToCharArray())
        {
            targetText.text += letter;  // Добавляем символ
            yield return new WaitForSeconds(typingSpeed);  // Задержка между символами
        }
        yield return new WaitForSeconds(2);
        targetText.text = "";
    }

    // работа с камерой
    IEnumerator FocusCameraOnBattle()
    {
        float targetSize = 5f; // Целевой размер камеры для приближения
        float duration = 0f; // Длительность приближения
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
    // Метод для возврата камеры к дефолтному размеру
    public void ResetCameraToDefault()
    {
        StartCoroutine(ResetCameraSize());
    }

    private IEnumerator ResetCameraSize()
    {
        float duration = 1f; // Время, за которое камера вернется к дефолтному размеру
        float initialSize = mainCamera.orthographicSize;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            mainCamera.orthographicSize = Mathf.Lerp(initialSize, defaultCameraSize, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.orthographicSize = defaultCameraSize; // Убедитесь, что камера установлена на точное значение
    }

}
