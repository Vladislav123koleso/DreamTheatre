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
        if(selectedCharacters.Count < 4) // ограничение в 4 персонажа в отр€де
        {
            selectedCharacters.Add(character);
        }
    }

    public void StartMission() // кнопка начала бо€
    {
        SceneManager.LoadScene(1);
    }
}
