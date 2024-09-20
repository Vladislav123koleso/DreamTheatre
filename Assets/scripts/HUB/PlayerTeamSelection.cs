using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class PlayerTeamSelection : MonoBehaviour
{
    public static List<basePers> selectedCharacters = new List<basePers>();

    public void SelectedCharacter(basePers character)
    {
        if(selectedCharacters.Count < 4) // ����������� � 4 ��������� � ������
        {
            selectedCharacters.Add(character);
        }
    }

    public void StartMission() // ������ ������ ���
    {
        SceneManager.LoadScene(1);
    }
}
