using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesPanelController : MonoBehaviour
{
    public List<Button> abilityButtons; // Кнопки способностей
    private List<CharacterController> highlightedEnemies = new List<CharacterController>();
    [SerializeField]
    private List<CharacterController> enemies;

    private bool abilitySelected = false;
    private bool isAbilityHighlighted = false; // Флаг для отслеживания состояния подсветки

    private int selectedAbilityIndex = -1; // Индекс выбранной способности
    private TurnManager turnManager; // Ссылка на TurnManager для активного персонажа

    
    private void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();

        // Привязываем обработчики к кнопкам способностей
        for (int i = 0; i < abilityButtons.Count; i++)
        {
            int index = i; // Сохраняем индекс кнопки
            abilityButtons[i].onClick.AddListener(() => OnAbilitySelected(index));
        }
    }

    // Метод для выбора способности
    public void OnAbilitySelected(int abilityIndex)
    {
        // Проверяем, был ли уже выбран скилл для этой кнопки
        if (abilitySelected && selectedAbilityIndex == abilityIndex)
        {
            // Если скилл уже выбран, проверим, активирована ли подсветка
            if (isAbilityHighlighted)
            {
                // Если подсветка активна, сбрасываем подсветку
                ResetEnemyLights();
            }
            abilitySelected = false;
            return; // Прерываем выполнение метода, чтобы не выполнять повторно
            /*abilitySelected = false;
            isAbilityHighlighted = false; // Сбрасываем состояние подсветки
            return; // Выход из метода, чтобы не выполнять повторно*/
        }
        
        abilitySelected = true;
        selectedAbilityIndex = abilityIndex;
        highlightedEnemies.Clear();

        Debug.Log($"Выбрана способность {abilityIndex + 1}");
        switch (abilityIndex)
        {
            case 0:
                HighlightFirstTwoEnemies();
                break;
            case 1:
                DamageFirstTwoEnemies();
                break;
                // Здесь остальные способности
        }
    }

    // Подсветка двух ближайших врагов
    private void HighlightFirstTwoEnemies()
    {
        if (isAbilityHighlighted) return; // Если подсветка уже активирована, не делаем ничего

        List<CharacterController> enemies = GetEnemyControllers();
        int enemiesCount = enemies.Count;

        // Проверяем, что врагов достаточно для подсветки
        if (enemiesCount == 0) return;

        // Подсвечиваем двух врагов
        for (int i = 2; i < enemiesCount; i++)
        {
            enemies[i].persData.SetLight(true);
            enemies[i].persData.SetLightColor(Color.red);
            highlightedEnemies.Add(enemies[i]);
        }

        isAbilityHighlighted = true;

    }


    // Наносим урон двум первым врагам
    private void DamageFirstTwoEnemies()
    {
        HighlightFirstTwoEnemies();

        /*enemies = GetEnemyControllers();
        basePers activeCharacter = turnManager.GetActiveCharacter(); // Получаем активного персонажа
        int damage1 = activeCharacter.CalculateDamage();
        int damage2 = activeCharacter.CalculateDamage();

        enemies[2].TakeDamage(damage1); // Используем урон активного персонажа
        enemies[3].TakeDamage(damage2); // Используем урон активного персонажа
        Debug.Log($"Нанесено {damage1} урона врагу {enemies[2].persData.name}");
        Debug.Log($"Нанесено {damage2} урона врагу {enemies[3].persData.name}");
        */

        
        abilityButtons[1].interactable = false; // Делаем кнопку неактивной
        StartCoroutine(CooldownAbility(1, 1)); // Кд на 1 ход

        isAbilityHighlighted = false;
    }

    // Сброс подсветки врагов
    public void ResetEnemyLights()
    {
        foreach (var enemy in highlightedEnemies)
        {
            enemy.persData.ResetLightColor();
            enemy.persData.SetLight(false);
        }
        highlightedEnemies.Clear();
        isAbilityHighlighted = false;
    }

    // Метод для обработки клика по врагу
    public void OnEnemyClicked(CharacterController enemy)
    {
        basePers activeCharacter = turnManager.GetActiveCharacter();
        int damage1 = activeCharacter.CalculateDamage();
        int damage2 = activeCharacter.CalculateDamage();

        if (abilitySelected && highlightedEnemies.Contains(enemy))
        {
            if (selectedAbilityIndex == 0) // Если выбрана первая способность
            {
                enemy.TakeDamage(damage1);
                Debug.Log($"Нанесено {damage1} урона врагу {enemy.persData.name}");
                ResetEnemyLights();
            }
            else
            {
                highlightedEnemies[0].TakeDamage(damage2);
                highlightedEnemies[1].TakeDamage(damage1);
                Debug.Log($"Нанесено {damage1} урона врагу {highlightedEnemies[0].persData.name}");
                Debug.Log($"Нанесено {damage2} урона врагу {highlightedEnemies[1].persData.name}");
                // Сбрасываем подсветку после применения способности
                ResetEnemyLights();
                /*foreach (var highlightedEnemy in highlightedEnemies)
                {
                    highlightedEnemy.TakeDamage(damage1);
                    Debug.Log($"Нанесено {damage1} урона врагу {highlightedEnemy.persData.name}");
                }*/

                // Сбрасываем подсветку после применения способности
                ResetEnemyLights();
            }

            abilitySelected = false; 
            isAbilityHighlighted = false;
        }
    }
    

    // Метод для получения списка врагов
    private List<CharacterController> GetEnemyControllers()
    {
        List<CharacterController> enemies = new List<CharacterController>();

        foreach (var character in FindObjectsOfType<CharacterController>())
        {
            if (!character.persData.isPlayer) // Если это враг
            {
                enemies.Add(character);
            }
        }

        return enemies;
    }

    // Метод для обработки кулдауна
    private IEnumerator CooldownAbility(int abilityIndex, int cooldownTurns)
    {
        for (int i = 0; i < cooldownTurns; i++)
        {
            yield return new WaitUntil(() => turnManager.IsNewTurn); // Ждем начала нового хода
        }

        abilityButtons[abilityIndex].interactable = true;
        Debug.Log($"Способность {abilityIndex + 1} снова доступна");
    }
}
