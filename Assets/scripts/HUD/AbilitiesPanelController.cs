using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesPanelController : MonoBehaviour
{
    public List<Button> abilityButtons; // ������ ������������
    private List<CharacterController> highlightedEnemies = new List<CharacterController>();
    [SerializeField]
    private List<CharacterController> enemies;

    private bool abilitySelected = false;
    private bool isAbilityHighlighted = false; // ���� ��� ������������ ��������� ���������

    private int selectedAbilityIndex = -1; // ������ ��������� �����������
    private TurnManager turnManager; // ������ �� TurnManager ��� ��������� ���������

    
    private void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();

        // ����������� ����������� � ������� ������������
        for (int i = 0; i < abilityButtons.Count; i++)
        {
            int index = i; // ��������� ������ ������
            abilityButtons[i].onClick.AddListener(() => OnAbilitySelected(index));
        }
    }

    // ����� ��� ������ �����������
    public void OnAbilitySelected(int abilityIndex)
    {
        // ���������, ��� �� ��� ������ ����� ��� ���� ������
        if (abilitySelected && selectedAbilityIndex == abilityIndex)
        {
            // ���� ����� ��� ������, ��������, ������������ �� ���������
            if (isAbilityHighlighted)
            {
                // ���� ��������� �������, ���������� ���������
                ResetEnemyLights();
            }
            abilitySelected = false;
            return; // ��������� ���������� ������, ����� �� ��������� ��������
            /*abilitySelected = false;
            isAbilityHighlighted = false; // ���������� ��������� ���������
            return; // ����� �� ������, ����� �� ��������� ��������*/
        }
        
        abilitySelected = true;
        selectedAbilityIndex = abilityIndex;
        highlightedEnemies.Clear();

        Debug.Log($"������� ����������� {abilityIndex + 1}");
        switch (abilityIndex)
        {
            case 0:
                HighlightFirstTwoEnemies();
                break;
            case 1:
                DamageFirstTwoEnemies();
                break;
                // ����� ��������� �����������
        }
    }

    // ��������� ���� ��������� ������
    private void HighlightFirstTwoEnemies()
    {
        if (isAbilityHighlighted) return; // ���� ��������� ��� ������������, �� ������ ������

        List<CharacterController> enemies = GetEnemyControllers();
        int enemiesCount = enemies.Count;

        // ���������, ��� ������ ���������� ��� ���������
        if (enemiesCount == 0) return;

        // ������������ ���� ������
        for (int i = 2; i < enemiesCount; i++)
        {
            enemies[i].persData.SetLight(true);
            enemies[i].persData.SetLightColor(Color.red);
            highlightedEnemies.Add(enemies[i]);
        }

        isAbilityHighlighted = true;

    }


    // ������� ���� ���� ������ ������
    private void DamageFirstTwoEnemies()
    {
        HighlightFirstTwoEnemies();

        /*enemies = GetEnemyControllers();
        basePers activeCharacter = turnManager.GetActiveCharacter(); // �������� ��������� ���������
        int damage1 = activeCharacter.CalculateDamage();
        int damage2 = activeCharacter.CalculateDamage();

        enemies[2].TakeDamage(damage1); // ���������� ���� ��������� ���������
        enemies[3].TakeDamage(damage2); // ���������� ���� ��������� ���������
        Debug.Log($"�������� {damage1} ����� ����� {enemies[2].persData.name}");
        Debug.Log($"�������� {damage2} ����� ����� {enemies[3].persData.name}");
        */

        
        abilityButtons[1].interactable = false; // ������ ������ ����������
        StartCoroutine(CooldownAbility(1, 1)); // �� �� 1 ���

        isAbilityHighlighted = false;
    }

    // ����� ��������� ������
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

    // ����� ��� ��������� ����� �� �����
    public void OnEnemyClicked(CharacterController enemy)
    {
        basePers activeCharacter = turnManager.GetActiveCharacter();
        int damage1 = activeCharacter.CalculateDamage();
        int damage2 = activeCharacter.CalculateDamage();

        if (abilitySelected && highlightedEnemies.Contains(enemy))
        {
            if (selectedAbilityIndex == 0) // ���� ������� ������ �����������
            {
                enemy.TakeDamage(damage1);
                Debug.Log($"�������� {damage1} ����� ����� {enemy.persData.name}");
                ResetEnemyLights();
            }
            else
            {
                highlightedEnemies[0].TakeDamage(damage2);
                highlightedEnemies[1].TakeDamage(damage1);
                Debug.Log($"�������� {damage1} ����� ����� {highlightedEnemies[0].persData.name}");
                Debug.Log($"�������� {damage2} ����� ����� {highlightedEnemies[1].persData.name}");
                // ���������� ��������� ����� ���������� �����������
                ResetEnemyLights();
                /*foreach (var highlightedEnemy in highlightedEnemies)
                {
                    highlightedEnemy.TakeDamage(damage1);
                    Debug.Log($"�������� {damage1} ����� ����� {highlightedEnemy.persData.name}");
                }*/

                // ���������� ��������� ����� ���������� �����������
                ResetEnemyLights();
            }

            abilitySelected = false; 
            isAbilityHighlighted = false;
        }
    }
    

    // ����� ��� ��������� ������ ������
    private List<CharacterController> GetEnemyControllers()
    {
        List<CharacterController> enemies = new List<CharacterController>();

        foreach (var character in FindObjectsOfType<CharacterController>())
        {
            if (!character.persData.isPlayer) // ���� ��� ����
            {
                enemies.Add(character);
            }
        }

        return enemies;
    }

    // ����� ��� ��������� ��������
    private IEnumerator CooldownAbility(int abilityIndex, int cooldownTurns)
    {
        for (int i = 0; i < cooldownTurns; i++)
        {
            yield return new WaitUntil(() => turnManager.IsNewTurn); // ���� ������ ������ ����
        }

        abilityButtons[abilityIndex].interactable = true;
        Debug.Log($"����������� {abilityIndex + 1} ����� ��������");
    }
}
