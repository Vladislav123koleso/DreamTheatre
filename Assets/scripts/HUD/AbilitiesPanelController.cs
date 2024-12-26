using System.Collections;
using System.Collections.Generic;
using System.Globalization;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

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
    [SerializeField]
    private bool isAbilityUsed = false; // Флаг для отслеживания использования способности за ход

    // слоты для замены иконок абилок
    public Image slot1;
    public Image slot2;

    //-------------------------------------------
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
    //---------------------------------------
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
        basePers activeCharacter = turnManager.GetActiveCharacter();

        if (isAbilityUsed)
        {
            Debug.Log("Вы уже использовали способность за этот ход!");
            return; // Блокируем повторное использование
        }

        //если кулдаун не скинулся и пытаются на него нажать мы выходим
        if (abilityIndex == 1 && activeCharacter.cooldownSecondSkill != 0) { return; }

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

        enemies = GetEnemyControllers();
        Debug.Log($"Выбрана способность {abilityIndex + 1}");

        // Проверка, типа активного персонажа
        if(activeCharacter._enemyType == basePers.enemyType.RoyalKnight)
        {
            // если рыцарь
            switch (abilityIndex)
            {
                case 0:
                    HighlightFirstTwoEnemies();
                    break;
                case 1:

                    DamageFirstTwoEnemies();
                    break;

            }
        }
        else if(activeCharacter._enemyType == basePers.enemyType.Mag)
        {
            // если маг
            switch (abilityIndex)
            {
                case 0:
                    HighlightLastTwoEnemies();
                    break;
                case 1:
                    HighlightAllAllies(); // Подсветка всех союзников для второго скилла

                    break;

            }
        }
        

        
    }

    //
    public void ResetAbilityUsage()
    {
        isAbilityUsed = false; // Разрешаем использование способности
    }

    // Подсветка всех союзников (маг - хил)
    private void HighlightAllAllies()
    {
        if (isAbilityHighlighted) return; // Если подсветка уже активирована, не делаем ничего
        List<CharacterController> allies = GetAlliedControllers();

        // Подсвечиваем всех союзников
        foreach (var ally in allies)
        {
            ally.persData.SetLight(true);
            ally.persData.SetLightColor(Color.green); // Зеленый цвет для подсветки
            highlightedEnemies.Add(ally); // Добавляем союзника в список подсвеченных
        }

        isAbilityHighlighted = true;
    }

    // Метод для получения списка союзников
    private List<CharacterController> GetAlliedControllers()
    {
        List<CharacterController> allies = new List<CharacterController>();
        foreach (var character in FindObjectsOfType<CharacterController>())
        {
            if (character.persData.isPlayer) // Если это союзник (игрок)
            {
                allies.Add(character);
            }
        }
        return allies;
    }


    // Подсветка первого врага (рыцарь)
    private void HighlightFirstTwoEnemies()
    {
        if (isAbilityHighlighted) return; // Если подсветка уже активирована, не делаем ничего
        if(enemies.Count == 0)
        {

        }
        /*List<CharacterController> *//*enemies = GetEnemyControllers();*/
        int enemiesCount = enemies.Count;

        // Проверяем, что врагов достаточно для подсветки
        if (enemiesCount == 0) return;
        if(enemiesCount == 1)
        {
            enemies[0].persData.SetLight(true);
            enemies[0].persData.SetLightColor(Color.red);
            highlightedEnemies.Add(enemies[0]);
        }
        else
        {
            // Подсвечиваем двух врагов
            for (int i = enemiesCount - 2; i < enemiesCount; i++)
            {
                enemies[i].persData.SetLight(true);
                enemies[i].persData.SetLightColor(Color.red);
                highlightedEnemies.Add(enemies[i]);
            }

        }

        isAbilityHighlighted = true;

    }

    // Подсветка последних двух врага (маг)
    private void HighlightLastTwoEnemies()
    {
        if (isAbilityHighlighted) return; // Если подсветка уже активирована, не делаем ничего
        if (enemies.Count == 0)
        {

        }
        /*List<CharacterController> *//*enemies = GetEnemyControllers();*/
        int enemiesCount = enemies.Count;

        // Проверяем, что врагов достаточно для подсветки
        if (enemiesCount == 0) return;
        if (enemiesCount == 1)
        {
            enemies[0].persData.SetLight(true);
            enemies[0].persData.SetLightColor(Color.red);
            highlightedEnemies.Add(enemies[0]);
        }
        else
        {
            // Подсвечиваем двух врагов
            for (int i = 0; i < 2; i++)
            {
                enemies[i].persData.SetLight(true);
                enemies[i].persData.SetLightColor(Color.red);
                highlightedEnemies.Add(enemies[i]);
            }

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

        CharacterController attackerCharacterController = turnManager.FindControllerByName(activeCharacter.name);
        attacker = attackerCharacterController.gameObject;
        
        
        //-----------------------------------------------------------
        int damage1 = activeCharacter.CalculateDamage();
        int damage2 = activeCharacter.CalculateDamage();

        if (abilitySelected && highlightedEnemies.Contains(enemy))
        {
            target = enemy.gameObject;

            // Сохраняем начальные позиции и повороты
            originalAttackerPos = attacker.transform.position;
            originalTargetPos = target.transform.position;
            originalAttackerRotation = attacker.transform.rotation;
            originalTargetRotation = target.transform.rotation;


            if (selectedAbilityIndex == 0) // Если выбрана первая способность
            {
                PlayAttackAnimation();

                if (activeCharacter._enemyType == basePers.enemyType.RoyalKnight) // если абилка рыцаря
                {
                    enemy.TakeDamage(damage1);
                    Debug.Log($"Нанесено {damage1} урона врагу {enemy.persData.name}");
                    ResetEnemyLights();
                }
                else if (activeCharacter._enemyType == basePers.enemyType.Mag) // если абилка мага
                {
                    if (enemies.Count == 1)
                    {
                        highlightedEnemies[0].TakeDamage(damage2);
                        attackerCharacterController.TakeDamage(damage2 - 3);
                        Debug.Log($"Нанесено {damage1} урона врагу {highlightedEnemies[0].persData.name}");
                    }
                    else
                    {
                        foreach(var enm in highlightedEnemies)
                        {
                            enm.TakeDamage(Random.RandomRange(damage1,damage2));
                        }
                        attackerCharacterController.TakeDamage(damage1 - 3);
                        Debug.Log($"Нанесено {damage1} урона врагу ");
                        Debug.Log($"Нанесено {damage2} урона врагу ");

                    }
                    // Сбрасываем подсветку после применения способности
                    ResetEnemyLights();
                }
            }
            else // если выбрана 2ая способность
            {
                PlayAttackAnimation();
                if (activeCharacter._enemyType == basePers.enemyType.RoyalKnight) // если абилка рыцаря
                {
                    // обновляем кулдаун на способность
                    activeCharacter.cooldownSecondSkill = 2;

                    if (enemies.Count == 1)
                    {
                        highlightedEnemies[0].TakeDamage(damage2);
                        Debug.Log($"Нанесено {damage1} урона врагу {highlightedEnemies[0].persData.name}");
                    }
                    else
                    {
                        highlightedEnemies[0].TakeDamage(damage1);
                        highlightedEnemies[1].TakeDamage(damage2);
                        Debug.Log($"Нанесено {damage1} урона врагу {highlightedEnemies[0].persData.name}");
                        Debug.Log($"Нанесено {damage2} урона врагу {highlightedEnemies[1].persData.name}");

                    }
                }
                else
                {
                    target = enemy.gameObject;

                    // Сохраняем начальные позиции и повороты
                    originalAttackerPos = attacker.transform.position;
                    originalTargetPos = target.transform.position;
                    originalAttackerRotation = attacker.transform.rotation;
                    originalTargetRotation = target.transform.rotation;

                    int healAmount = 5;

                    // Лечим выбранного союзника
                    enemy.Heal(healAmount);
                    Debug.Log($"Лечим {healAmount} здоровья союзника {enemy.persData.name}");

                    // Сбрасываем подсветку после применения способности
                    ResetEnemyLights();

                    // Устанавливаем кулдаун на второй скилл
                    activeCharacter.cooldownSecondSkill = 2;

                    // Отмечаем, что способность использована
                    isAbilityUsed = true;
                    abilitySelected = false;
                    isAbilityHighlighted = false;
                }
                    
                // Сбрасываем подсветку после применения способности
                ResetEnemyLights();
                /*foreach (var highlightedEnemy in highlightedEnemies)
                {
                    highlightedEnemy.TakeDamage(damage1);
                    Debug.Log($"Нанесено {damage1} урона врагу {highlightedEnemy.persData.name}");
                }*/

                // Сбрасываем подсветку после применения способности
                //ResetEnemyLights();
            }
            // Отмечаем, что способность использована
            isAbilityUsed = true;
            abilitySelected = false; 
            isAbilityHighlighted = false;
        }
    }


    // Метод для обработки клика по союзнику
    public void OnAllyClicked(CharacterController ally)
    {
        basePers activeCharacter = turnManager.GetActiveCharacter();

        if (abilitySelected && highlightedEnemies.Contains(ally))
        {
            target = ally.gameObject;

            // Сохраняем начальные позиции и повороты
            originalAttackerPos = attacker.transform.position;
            originalTargetPos = target.transform.position;
            originalAttackerRotation = attacker.transform.rotation;
            originalTargetRotation = target.transform.rotation;

            int healAmount = 5;

            // Лечим выбранного союзника
            ally.Heal(healAmount);
            Debug.Log($"Лечим {healAmount} здоровья союзника {ally.persData.name}");

            // Сбрасываем подсветку после применения способности
            ResetEnemyLights();

            // Устанавливаем кулдаун на второй скилл
            activeCharacter.cooldownSecondSkill = 2;

            // Отмечаем, что способность использована
            isAbilityUsed = true;
            abilitySelected = false;
            isAbilityHighlighted = false;
        }
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













    public void UpdateAbilityIcons(basePers activeCharacter)
    {
        // Обновляем иконки способностей в зависимости от активного персонажа

        /*for (int i = 0; i < abilityButtons.Count; i++)
        {
            // Логика смены иконок
            // В данном случае мы используем иконки из данных активного персонажа
            // Вам нужно будет создать механизм для установки новых иконок для каждой способности.
            Sprite abilityIcon = activeCharacter.GetAbilityIcon(i); // Предположим, что есть метод получения иконки способности
            abilityButtons[i].GetComponent<Image>().sprite = abilityIcon;
        }*/
        Sprite abilityIcon1 = activeCharacter.GetAbilityIcon(0);
        Sprite abilityIcon2 = activeCharacter.GetAbilityIcon(1);
        slot1.sprite = abilityIcon1;
        slot2.sprite = abilityIcon2;
    }










    // Метод для получения списка врагов
    private List<CharacterController> GetEnemyControllers()
    {
        /*List<CharacterController> enemies = new List<CharacterController>();*/
        enemies.Clear();
        foreach (var character in FindObjectsOfType<CharacterController>())
        {
            if (!character.persData.isPlayer) // Если это враг
            {
                enemies.Add(character);
            }
        }

        return enemies;
    }

    
}
