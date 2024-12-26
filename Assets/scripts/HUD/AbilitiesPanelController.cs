using System.Collections;
using System.Collections.Generic;
using System.Globalization;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

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
    [SerializeField]
    private bool isAbilityUsed = false; // ���� ��� ������������ ������������� ����������� �� ���

    // ����� ��� ������ ������ ������
    public Image slot1;
    public Image slot2;

    //-------------------------------------------
    [SerializeField]
    private GameObject attacker; // ��������� ��������
    [SerializeField]
    private GameObject target;   // ���� 
    public float animationDuration = 0.5f; // ������������ ��������
    public float moveDistance = 0.5f; // ������� �� ������� ���������� ��������� (����� ���������)
    public float rotationAngle = -10f; // ���� ������� ����������

    private Vector3 originalAttackerPos;
    private Vector3 originalTargetPos;
    private Quaternion originalAttackerRotation;
    private Quaternion originalTargetRotation;
    //---------------------------------------
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
        basePers activeCharacter = turnManager.GetActiveCharacter();

        if (isAbilityUsed)
        {
            Debug.Log("�� ��� ������������ ����������� �� ���� ���!");
            return; // ��������� ��������� �������������
        }

        //���� ������� �� �������� � �������� �� ���� ������ �� �������
        if (abilityIndex == 1 && activeCharacter.cooldownSecondSkill != 0) { return; }

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

        enemies = GetEnemyControllers();
        Debug.Log($"������� ����������� {abilityIndex + 1}");

        // ��������, ���� ��������� ���������
        if(activeCharacter._enemyType == basePers.enemyType.RoyalKnight)
        {
            // ���� ������
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
            // ���� ���
            switch (abilityIndex)
            {
                case 0:
                    HighlightLastTwoEnemies();
                    break;
                case 1:
                    HighlightAllAllies(); // ��������� ���� ��������� ��� ������� ������

                    break;

            }
        }
        

        
    }

    //
    public void ResetAbilityUsage()
    {
        isAbilityUsed = false; // ��������� ������������� �����������
    }

    // ��������� ���� ��������� (��� - ���)
    private void HighlightAllAllies()
    {
        if (isAbilityHighlighted) return; // ���� ��������� ��� ������������, �� ������ ������
        List<CharacterController> allies = GetAlliedControllers();

        // ������������ ���� ���������
        foreach (var ally in allies)
        {
            ally.persData.SetLight(true);
            ally.persData.SetLightColor(Color.green); // ������� ���� ��� ���������
            highlightedEnemies.Add(ally); // ��������� �������� � ������ ������������
        }

        isAbilityHighlighted = true;
    }

    // ����� ��� ��������� ������ ���������
    private List<CharacterController> GetAlliedControllers()
    {
        List<CharacterController> allies = new List<CharacterController>();
        foreach (var character in FindObjectsOfType<CharacterController>())
        {
            if (character.persData.isPlayer) // ���� ��� ������� (�����)
            {
                allies.Add(character);
            }
        }
        return allies;
    }


    // ��������� ������� ����� (������)
    private void HighlightFirstTwoEnemies()
    {
        if (isAbilityHighlighted) return; // ���� ��������� ��� ������������, �� ������ ������
        if(enemies.Count == 0)
        {

        }
        /*List<CharacterController> *//*enemies = GetEnemyControllers();*/
        int enemiesCount = enemies.Count;

        // ���������, ��� ������ ���������� ��� ���������
        if (enemiesCount == 0) return;
        if(enemiesCount == 1)
        {
            enemies[0].persData.SetLight(true);
            enemies[0].persData.SetLightColor(Color.red);
            highlightedEnemies.Add(enemies[0]);
        }
        else
        {
            // ������������ ���� ������
            for (int i = enemiesCount - 2; i < enemiesCount; i++)
            {
                enemies[i].persData.SetLight(true);
                enemies[i].persData.SetLightColor(Color.red);
                highlightedEnemies.Add(enemies[i]);
            }

        }

        isAbilityHighlighted = true;

    }

    // ��������� ��������� ���� ����� (���)
    private void HighlightLastTwoEnemies()
    {
        if (isAbilityHighlighted) return; // ���� ��������� ��� ������������, �� ������ ������
        if (enemies.Count == 0)
        {

        }
        /*List<CharacterController> *//*enemies = GetEnemyControllers();*/
        int enemiesCount = enemies.Count;

        // ���������, ��� ������ ���������� ��� ���������
        if (enemiesCount == 0) return;
        if (enemiesCount == 1)
        {
            enemies[0].persData.SetLight(true);
            enemies[0].persData.SetLightColor(Color.red);
            highlightedEnemies.Add(enemies[0]);
        }
        else
        {
            // ������������ ���� ������
            for (int i = 0; i < 2; i++)
            {
                enemies[i].persData.SetLight(true);
                enemies[i].persData.SetLightColor(Color.red);
                highlightedEnemies.Add(enemies[i]);
            }

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

        CharacterController attackerCharacterController = turnManager.FindControllerByName(activeCharacter.name);
        attacker = attackerCharacterController.gameObject;
        
        
        //-----------------------------------------------------------
        int damage1 = activeCharacter.CalculateDamage();
        int damage2 = activeCharacter.CalculateDamage();

        if (abilitySelected && highlightedEnemies.Contains(enemy))
        {
            target = enemy.gameObject;

            // ��������� ��������� ������� � ��������
            originalAttackerPos = attacker.transform.position;
            originalTargetPos = target.transform.position;
            originalAttackerRotation = attacker.transform.rotation;
            originalTargetRotation = target.transform.rotation;


            if (selectedAbilityIndex == 0) // ���� ������� ������ �����������
            {
                PlayAttackAnimation();

                if (activeCharacter._enemyType == basePers.enemyType.RoyalKnight) // ���� ������ ������
                {
                    enemy.TakeDamage(damage1);
                    Debug.Log($"�������� {damage1} ����� ����� {enemy.persData.name}");
                    ResetEnemyLights();
                }
                else if (activeCharacter._enemyType == basePers.enemyType.Mag) // ���� ������ ����
                {
                    if (enemies.Count == 1)
                    {
                        highlightedEnemies[0].TakeDamage(damage2);
                        attackerCharacterController.TakeDamage(damage2 - 3);
                        Debug.Log($"�������� {damage1} ����� ����� {highlightedEnemies[0].persData.name}");
                    }
                    else
                    {
                        foreach(var enm in highlightedEnemies)
                        {
                            enm.TakeDamage(Random.RandomRange(damage1,damage2));
                        }
                        attackerCharacterController.TakeDamage(damage1 - 3);
                        Debug.Log($"�������� {damage1} ����� ����� ");
                        Debug.Log($"�������� {damage2} ����� ����� ");

                    }
                    // ���������� ��������� ����� ���������� �����������
                    ResetEnemyLights();
                }
            }
            else // ���� ������� 2�� �����������
            {
                PlayAttackAnimation();
                if (activeCharacter._enemyType == basePers.enemyType.RoyalKnight) // ���� ������ ������
                {
                    // ��������� ������� �� �����������
                    activeCharacter.cooldownSecondSkill = 2;

                    if (enemies.Count == 1)
                    {
                        highlightedEnemies[0].TakeDamage(damage2);
                        Debug.Log($"�������� {damage1} ����� ����� {highlightedEnemies[0].persData.name}");
                    }
                    else
                    {
                        highlightedEnemies[0].TakeDamage(damage1);
                        highlightedEnemies[1].TakeDamage(damage2);
                        Debug.Log($"�������� {damage1} ����� ����� {highlightedEnemies[0].persData.name}");
                        Debug.Log($"�������� {damage2} ����� ����� {highlightedEnemies[1].persData.name}");

                    }
                }
                else
                {
                    target = enemy.gameObject;

                    // ��������� ��������� ������� � ��������
                    originalAttackerPos = attacker.transform.position;
                    originalTargetPos = target.transform.position;
                    originalAttackerRotation = attacker.transform.rotation;
                    originalTargetRotation = target.transform.rotation;

                    int healAmount = 5;

                    // ����� ���������� ��������
                    enemy.Heal(healAmount);
                    Debug.Log($"����� {healAmount} �������� �������� {enemy.persData.name}");

                    // ���������� ��������� ����� ���������� �����������
                    ResetEnemyLights();

                    // ������������� ������� �� ������ �����
                    activeCharacter.cooldownSecondSkill = 2;

                    // ��������, ��� ����������� ������������
                    isAbilityUsed = true;
                    abilitySelected = false;
                    isAbilityHighlighted = false;
                }
                    
                // ���������� ��������� ����� ���������� �����������
                ResetEnemyLights();
                /*foreach (var highlightedEnemy in highlightedEnemies)
                {
                    highlightedEnemy.TakeDamage(damage1);
                    Debug.Log($"�������� {damage1} ����� ����� {highlightedEnemy.persData.name}");
                }*/

                // ���������� ��������� ����� ���������� �����������
                //ResetEnemyLights();
            }
            // ��������, ��� ����������� ������������
            isAbilityUsed = true;
            abilitySelected = false; 
            isAbilityHighlighted = false;
        }
    }


    // ����� ��� ��������� ����� �� ��������
    public void OnAllyClicked(CharacterController ally)
    {
        basePers activeCharacter = turnManager.GetActiveCharacter();

        if (abilitySelected && highlightedEnemies.Contains(ally))
        {
            target = ally.gameObject;

            // ��������� ��������� ������� � ��������
            originalAttackerPos = attacker.transform.position;
            originalTargetPos = target.transform.position;
            originalAttackerRotation = attacker.transform.rotation;
            originalTargetRotation = target.transform.rotation;

            int healAmount = 5;

            // ����� ���������� ��������
            ally.Heal(healAmount);
            Debug.Log($"����� {healAmount} �������� �������� {ally.persData.name}");

            // ���������� ��������� ����� ���������� �����������
            ResetEnemyLights();

            // ������������� ������� �� ������ �����
            activeCharacter.cooldownSecondSkill = 2;

            // ��������, ��� ����������� ������������
            isAbilityUsed = true;
            abilitySelected = false;
            isAbilityHighlighted = false;
        }
    }


    // ����� ��� ������� �������� �����
    public void PlayAttackAnimation()
    {
        StartCoroutine(AttackAnimationCoroutine());
    }
    // �������� ��� �������� �����
    private IEnumerator AttackAnimationCoroutine()
    {
        float elapsedTime = 0f;

        // ��������� �������� � ������� ����������
        Vector3 attackerTargetPos = target.transform.position + new Vector3(-moveDistance, 0, 0);
        Quaternion attackerTargetRotation = Quaternion.Euler(0, 0, rotationAngle);

        // ��������� �������� � ������ ������ ����������
        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;

            // �������� ����������
            attacker.transform.position = Vector3.Lerp(originalAttackerPos, attackerTargetPos, t);

            // ������ ����������
            attacker.transform.rotation = Quaternion.Lerp(originalAttackerRotation, attackerTargetRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ��������� �������� � ���������� ���������� �� �������� �������
        attacker.transform.position = attackerTargetPos;
        attacker.transform.rotation = attackerTargetRotation;

        // ����� �� 0.5 �������, ����� �������� �����������
        yield return new WaitForSeconds(0.5f);

        // ���������� ���������� � ��������� ������� � �������
        elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;

            // ������� ���������� �� �������� �������
            attacker.transform.position = Vector3.Lerp(attackerTargetPos, originalAttackerPos, t);

            // ������� � �������� �������
            attacker.transform.rotation = Quaternion.Lerp(attackerTargetRotation, originalAttackerRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ��������� ����� ���������� �� �������� ������� � ��������
        attacker.transform.position = originalAttackerPos;
        attacker.transform.rotation = originalAttackerRotation;
    }













    public void UpdateAbilityIcons(basePers activeCharacter)
    {
        // ��������� ������ ������������ � ����������� �� ��������� ���������

        /*for (int i = 0; i < abilityButtons.Count; i++)
        {
            // ������ ����� ������
            // � ������ ������ �� ���������� ������ �� ������ ��������� ���������
            // ��� ����� ����� ������� �������� ��� ��������� ����� ������ ��� ������ �����������.
            Sprite abilityIcon = activeCharacter.GetAbilityIcon(i); // �����������, ��� ���� ����� ��������� ������ �����������
            abilityButtons[i].GetComponent<Image>().sprite = abilityIcon;
        }*/
        Sprite abilityIcon1 = activeCharacter.GetAbilityIcon(0);
        Sprite abilityIcon2 = activeCharacter.GetAbilityIcon(1);
        slot1.sprite = abilityIcon1;
        slot2.sprite = abilityIcon2;
    }










    // ����� ��� ��������� ������ ������
    private List<CharacterController> GetEnemyControllers()
    {
        /*List<CharacterController> enemies = new List<CharacterController>();*/
        enemies.Clear();
        foreach (var character in FindObjectsOfType<CharacterController>())
        {
            if (!character.persData.isPlayer) // ���� ��� ����
            {
                enemies.Add(character);
            }
        }

        return enemies;
    }

    
}
