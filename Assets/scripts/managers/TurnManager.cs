using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour // менеджер очередности ходов
{
    public List<basePers> characters = new List<basePers>();
    int currentTurnIndex = 0;

    public static TurnManager Instance { get; private set; }


    private void Awake()
    {
        // Проверяем, чтобы экземпляр был единственным
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Дублирующийся экземпляр TurnManager найден! Уничтожение.");
            Destroy(gameObject);
        }
    }


    public bool IsNewTurn { get; private set; } // для отслеживания нового хода

    /*private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f); // Короткая задержка, чтобы персонажи успели заспавниться

        InitializeCharacters();
        StartTurn();
    }*/
    public void InitializeCharacters()
    {
        // Получаем все объекты персонажей, находящихся на сцене
        CharacterController[] allCharacters = FindObjectsOfType<CharacterController>();
        // Добавляем каждого персонажа в список
        foreach (var character in allCharacters)
        {
            characters.Add(character.persData);
        }

        // Сортировка персонажей по скорости
        characters = characters.OrderByDescending(c => c.speed).ToList();
    }
    

    public void StartTurn()
    {
        IsNewTurn = false;

        var currentCharacter = characters[currentTurnIndex]; // текущий персонаж
        currentCharacter.hasTurn = true; // активация хода персонажа
        currentCharacter.SetLight(true); // Включаем лампу над головой для текущего персонажа


        if (currentCharacter.isPlayer) // если персонаж игрока
        {
            Debug.Log("ход персонажа: " + currentCharacter.name);
            Debug.Log("ожидания нажатия кнопки конца хода");
        }
        else
        {
            if(currentCharacter._enemyType == basePers.enemyType.Knight)
            {
                Debug.Log("ход врага: " + currentCharacter.name + " тип врага: " + currentCharacter._enemyType);
                // атака рыцаря врага
                CharacterController currentCharctContrl = FindControllerByName(currentCharacter.name);
                // Создаем список целей (персонажей игрока)
                List<CharacterController> playerTargets = new List<CharacterController>();

                // Проходим по всем персонажам
                foreach (var playerPersData in characters)
                {
                    if (playerPersData.isPlayer) // Если это персонаж игрока
                    {
                        CharacterController playerCharctContrl = FindControllerByName(playerPersData.name);
                        if (playerCharctContrl != null)
                        {
                            playerTargets.Add(playerCharctContrl); // Добавляем в список возможных целей
                        }
                    }
                }

                // Если список целей не пустой, выбираем случайную цель
                if (playerTargets.Count > 0)
                {
                    int randomIndex = Random.Range(0, playerTargets.Count); // Выбираем случайный индекс
                    CharacterController randomTarget = playerTargets[randomIndex]; // Получаем случайную цель

                    // Атакуем выбранную цель
                    randomTarget.TakeDamage(currentCharctContrl.dmgEnemyKnight());
                    Debug.Log($"{currentCharacter.name} атаковал {randomTarget.persData.name}");
                }
                else
                {
                    Debug.LogWarning("Список целей пуст! Невозможно выполнить атаку.");
                }
            }
            StartCoroutine(CoroTurnEnemy());
            
        }
    }

    public void EndTurn()
    {
        IsNewTurn = true;

        characters[currentTurnIndex].hasTurn = false; // конец хода персонажа
        characters[currentTurnIndex].SetLight(false);
        currentTurnIndex = (currentTurnIndex + 1) % characters.Count; // цикличный переход хода к следующему персонажу

        StartTurn();
    }


    // завершения хода персонажа (кнопка)
    public void OnEndPlayerTurnButton()
    {
        // Если сейчас ход игрока
        if (characters[currentTurnIndex].isPlayer)
        {
            EndTurn();
        }
    }

    // метол имитации хода врага
    private IEnumerator CoroTurnEnemy()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("Враг походил");
        EndTurn();
    }


    // получаем персонажа чей сейчас ход
    public basePers GetActiveCharacter()
    {
        return characters.FirstOrDefault(c => c.hasTurn); // Находим персонажа, у которого включен флаг hasTurn
    }






    // Метод для поиска CharacterController по имени
    private CharacterController FindControllerByName(string name)
    {
        // Получаем всех врагов на сцене
        CharacterController[] allControllers = FindObjectsOfType<CharacterController>();

        // Ищем врага с нужным именем
        return allControllers.FirstOrDefault(controller => controller.persData.name == name);
    }


    // удаления перса после смерти
    public void RemoveCharacter(basePers character)
    {
        characters.Remove(character);
        Debug.Log($"{character.name} удален из очереди ходов.");

        // Обновляем индекс текущего персонажа
        if (characters.Count > 0)
        {
            currentTurnIndex %= characters.Count;
        }
    }
}
