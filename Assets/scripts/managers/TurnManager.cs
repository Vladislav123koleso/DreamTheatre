using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour // менеджер очередности ходов
{
    public List<basePers> characters = new List<basePers>();
    int currentTurnIndex = 0;

    private void Start()
    {

        // сортировка персонажей по скорости
        characters = characters.OrderByDescending(c => c.speed).ToList();
        
        StartTurn();
    }

    void StartTurn()
    {
        var currentCharacter = characters[currentTurnIndex]; // текущий персонаж

        currentCharacter.hasTurn = true; // активация хода персонажа

        if(currentCharacter.isPlayer) // если персонаж игрока
        {
            Debug.Log("ход персонажа: " + currentCharacter.name);
            Debug.Log("ожидания нажатия кнопки конца хода");
        }
        else
        {
            Debug.Log("ход врага: " + currentCharacter.name);
            Debug.Log("Враг походил");
            EndTurn();
        }
    }

    public void EndTurn()
    {
        characters[currentTurnIndex].hasTurn = false; // конец хода персонажа

        currentTurnIndex = (currentTurnIndex + 1) % characters.Count; // цикличный переход хода к следующему персонажу

        StartTurn();
    }


    // завершения хода персонажа (кнопка)
    public void OnEndPlayerTurnButton()
    {
        // Если сейчас ход игрока
        if (characters[currentTurnIndex].isPlayer)
        {
            EndTurn();
        }
    }

}
