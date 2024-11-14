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
        // Подсвечиваем врагов красным
        abilitySelected = true;
        highlightedEnemies.Clear();

        foreach (var enemy in FindObjectsOfType<CharacterController>())
        {
            if (!enemy.persData.isPlayer) // Проверяем, что это враг
            {
                enemy.persData.SetLight(true);
                enemy.persData.SetLightColor(Color.red); // Подсвечиваем красным
                highlightedEnemies.Add(enemy);
            }
        }
    }

    public void OnEnemyClicked(CharacterController enemy)
    {
        if (abilitySelected && highlightedEnemies.Contains(enemy))
        {
            // Применяем урон и сбрасываем подсветку
            enemy.TakeDamage(abilityDamage);
            ResetEnemyLights();
            abilitySelected = false;
        }
    }

    // Метод для сброса света всех врагов
    private void ResetEnemyLights()
    {
        foreach (var enemy in highlightedEnemies)
        {
            enemy.persData.ResetLightColor();
        }
        highlightedEnemies.Clear();
    }
}
