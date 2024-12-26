using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class CharacterController : MonoBehaviour
{
    public basePers persData;


    


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
            // ������ ������ ���������
            Debug.Log($"{persData.name} ��������!");
            TurnManager.Instance.RemoveCharacter(persData);
            Destroy(gameObject);

        }
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
