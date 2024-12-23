using System.Collections;
using UnityEngine;

public class BattleInitializer : MonoBehaviour
{
    
    public TurnManager turnManager;
    [SerializeField]
    public GameManager gameManager;
    // Флаг, чтобы проверить, был ли уже запущен бой
    private bool hasStartedFight = false;

    private void Start()
    {

        // Спавн персонажей 
        //spawnManager.SpawnPlayerCharacters();
        //spawnManager.SpawnEnemyCharacters();

        //GameManager gameManager = FindObjectOfType<GameManager>();
    }
    private void Update()
    {
        if(gameManager.isFight == true && !hasStartedFight)
        {
        // Включаем TurnManager (начинается бой)
            turnManager.InitializeCharacters();
            turnManager.StartTurn();

            hasStartedFight = true;
        }



        if (!gameManager.isFight && hasStartedFight)
        {
            hasStartedFight = false;
        }
    }

    
}
