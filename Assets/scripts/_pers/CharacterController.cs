using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    public basePers persData;

    public Animator animator;
    public SpriteRenderer spritePers;
    private bool isDead = false;


    // Метод для инициализации персонажа с его данными
    public void InitializeCharacter(basePers character)
    {
        persData = character;
        persData.InitializeLight(transform);
    }


    public void TakeDamage(int damage)
    {
        persData.hp -= damage;
        if (persData.hp <= 0)
        {
            Die();

        }
    }


            // Логика смерти персонажа
    public void Die()
    {
        if (isDead) return; // Предотвращаем повторный вызов
        isDead = true;

        Color color = spritePers.color; 
        color.a = 0f;             
        spritePers.color = color;

        Debug.Log($"{persData.name} повержен!");
        TurnManager.Instance.RemoveCharacter(persData);
        animator.SetTrigger("isDead"); // Устанавливаем триггер для анимации смерти

        // Опционально: задержка перед удалением объекта
        Destroy(gameObject, 1f);
    }


    private void OnMouseDown()
    {
        // Отправляем сигнал в AbilitiesPanelController о том, что враг был выбран
        AbilitiesPanelController abilitiesPanel = FindObjectOfType<AbilitiesPanelController>();
        if (abilitiesPanel != null)
        {
            abilitiesPanel.OnEnemyClicked(this);
            Debug.Log("Инфа о выбранном враге отправлена");
        }
        Debug.Log("Выбран враг");
    }




    public void Heal(int heal_points/* на сколько захил*/)
    {
        persData.hp = heal_points;
        if (persData.hp > persData.max_hp)
        {
            persData.hp = persData.max_hp;
        }
    }



    public int dmgEnemyKnight()
    {
        return persData.CalculateDamage();
    }

}
