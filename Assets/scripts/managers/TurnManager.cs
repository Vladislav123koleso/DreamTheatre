using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour // менеджер очередности ходов
{
    public List<basePers> characters = new List<basePers>();
    int currentTurnIndex = 0;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f); // Короткая задержка, чтобы персонажи успели заспавниться

        InitializeCharacters();
        StartTurn();
    }
    void InitializeCharacters()
    {
        // Получаем все объекты персонажей, находящихся на сцене
        CharacterController[] allCharacters = FindObjectsOfType<CharacterController>();
        // Добавляем каждого персонажа в список
        foreach (var character in allCharacters)
        {
            characters.Add(character.persData);
        }

        // Сортировка персонажей по скорости
        characters = characters.OrderByDescending(c => c.speed).ToList();
    }
    

    void StartTurn()
    {
        var currentCharacter = characters[currentTurnIndex]; // текущий персонаж
        currentCharacter.hasTurn = true; // активация хода персонажа
        currentCharacter.SetLight(true); // Включаем лампу над головой для текущего персонажа


        if (currentCharacter.isPlayer) // если персонаж игрока
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
        characters[currentTurnIndex].SetLight(false);
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
