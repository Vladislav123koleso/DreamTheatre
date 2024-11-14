using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesPanelController : MonoBehaviour
{
    public List<Button> abilityButtons;
    public int abilityDamage = 5;
    private List<CharacterController> highlightedEnemies = new List<CharacterController>();
    private bool abilitySelected = false;

    public void OnAbilitySelected()
    {
        // ������������ ������ �������
        abilitySelected = true;
        highlightedEnemies.Clear();

        foreach (var enemy in FindObjectsOfType<CharacterController>())
        {
            if (!enemy.persData.isPlayer) // ���������, ��� ��� ����
            {
                enemy.persData.SetLight(true);
                enemy.persData.SetLightColor(Color.red); // ������������ �������
                highlightedEnemies.Add(enemy);
            }
        }
    }

    public void OnEnemyClicked(CharacterController enemy)
    {
        if (abilitySelected && highlightedEnemies.Contains(enemy))
        {
            // ��������� ���� � ���������� ���������
            enemy.TakeDamage(abilityDamage);
            ResetEnemyLights();
            abilitySelected = false;
        }
    }

    // ����� ��� ������ ����� ���� ������
    private void ResetEnemyLights()
    {
        foreach (var enemy in highlightedEnemies)
        {
            enemy.persData.ResetLightColor();
        }
        highlightedEnemies.Clear();
    }
}
