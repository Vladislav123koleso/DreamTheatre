using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour // менеджер очередности ходов
{
    public List<basePers> characters = new List<basePers>();
    int currentTurnIndex = 0;
    
    public static TurnManager Instance { get; private set; }




    [SerializeField]
    private GameObject attacker; // Атакующий персонаж
    [SerializeField]
    private GameObject target;   // Цель 
    public float animationDuration = 0.5f; // Длительность анимации
    public float moveDistance = 0.5f; // Сколько на сколько сближаются персонажи (можно настроить)
    public float rotationAngle = -10f; // Угол наклона персонажей

    private Vector3 originalAttackerPos;
    private Vector3 originalTargetPos;
    private Quaternion originalAttackerRotation;
    private Quaternion originalTargetRotation;





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
            FindObjectOfType<AbilitiesPanelController>().ResetAbilityUsage(); // Сброс использования способностей
            FindObjectOfType<AbilitiesPanelController>().UpdateAbilityIcons(currentCharacter);

            Debug.Log("ход персонажа: " + currentCharacter.name);
            Debug.Log("ожидания нажатия кнопки конца хода");
        }
        else // атакует враг
        {
            if(currentCharacter._enemyType == basePers.enemyType.Knight || 
                currentCharacter._enemyType == basePers.enemyType.Slave || 
                currentCharacter._enemyType == basePers.enemyType.Vuchnik)
            {
                Debug.Log("ход врага: " + currentCharacter.name + " тип врага: " + currentCharacter._enemyType);
                // атака рыцаря врага
                CharacterController currentCharctContrl = FindControllerByName(currentCharacter.name);

                attacker = currentCharctContrl.gameObject;

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
                    
                    target = randomTarget.gameObject;
                    
                    // Сохраняем начальные позиции и повороты
                    originalAttackerPos = attacker.transform.position;
                    originalTargetPos = target.transform.position;
                    originalAttackerRotation = attacker.transform.rotation;
                    originalTargetRotation = target.transform.rotation;

                    PlayAttackAnimation();

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
        //если есть кулдаун у перса то он уменьшается
        if(characters[currentTurnIndex].cooldownSecondSkill != 0)
        {
            characters[currentTurnIndex].cooldownSecondSkill--;
        }
        characters[currentTurnIndex].SetLight(false);
        currentTurnIndex = (currentTurnIndex + 1) % characters.Count; // цикличный переход хода к следующему персонажу

        //StartCoroutine(GameManager.Instance.CheckBattleOutcome());

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
        //StartCoroutine(GameManager.Instance.CheckBattleOutcome());
    }

    // метол имитации хода врага
    private IEnumerator CoroTurnEnemy()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("Враг походил");
        EndTurn();
    }


    // получаем персонажа чей сейчас ход
    public basePers GetActiveCharacter()
    {
        return characters.FirstOrDefault(c => c.hasTurn); // Находим персонажа, у которого включен флаг hasTurn
    }







    // Метод для запуска анимации атаки
    public void PlayAttackAnimation()
    {
        StartCoroutine(AttackAnimationCoroutine());
    }
    // Корутина для анимации атаки
    private IEnumerator AttackAnimationCoroutine()
    {
        float elapsedTime = 0f;

        // Анимируем движение и поворот атакующего
        Vector3 attackerTargetPos = target.transform.position + new Vector3(-moveDistance, 0, 0);
        Quaternion attackerTargetRotation = Quaternion.Euler(0, 0, rotationAngle);

        // Анимируем движение и наклон только атакующего
        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;

            // Движение атакующего
            attacker.transform.position = Vector3.Lerp(originalAttackerPos, attackerTargetPos, t);

            // Наклон атакующего
            attacker.transform.rotation = Quaternion.Lerp(originalAttackerRotation, attackerTargetRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Завершаем анимацию и возвращаем атакующего на исходную позицию
        attacker.transform.position = attackerTargetPos;
        attacker.transform.rotation = attackerTargetRotation;

        // Пауза на 0.5 секунды, чтобы анимация закончилась
        yield return new WaitForSeconds(0.5f);

        // Возвращаем атакующего в начальную позицию и поворот
        elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;

            // Возврат атакующего на исходную позицию
            attacker.transform.position = Vector3.Lerp(attackerTargetPos, originalAttackerPos, t);

            // Возврат в исходный поворот
            attacker.transform.rotation = Quaternion.Lerp(attackerTargetRotation, originalAttackerRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Финальный сброс атакующего на исходные позиции и повороты
        attacker.transform.position = originalAttackerPos;
        attacker.transform.rotation = originalAttackerRotation;
    }












    // Метод для поиска CharacterController по имени
    public CharacterController FindControllerByName(string name)
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
