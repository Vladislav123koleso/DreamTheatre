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


    // ����� ��� ������������� ��������� � ��� �������
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


            // ������ ������ ���������
    public void Die()
    {
        if (isDead) return; // ������������� ��������� �����
        isDead = true;

        Color color = spritePers.color; 
        color.a = 0f;             
        spritePers.color = color;

        Debug.Log($"{persData.name} ��������!");
        TurnManager.Instance.RemoveCharacter(persData);
        animator.SetTrigger("isDead"); // ������������� ������� ��� �������� ������

        // �����������: �������� ����� ��������� �������
        Destroy(gameObject, 1f);
    }


    private void OnMouseDown()
    {
        // ���������� ������ � AbilitiesPanelController � ���, ��� ���� ��� ������
        AbilitiesPanelController abilitiesPanel = FindObjectOfType<AbilitiesPanelController>();
        if (abilitiesPanel != null)
        {
            abilitiesPanel.OnEnemyClicked(this);
            Debug.Log("���� � ��������� ����� ����������");
        }
        Debug.Log("������ ����");
    }




    public void Heal(int heal_points/* �� ������� �����*/)
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
