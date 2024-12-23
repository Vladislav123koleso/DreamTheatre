using System.Collections;
using UnityEngine;

public class BattleInitializer : MonoBehaviour
{
    
    public TurnManager turnManager;
    [SerializeField]
    public GameManager gameManager;
    // ����, ����� ���������, ��� �� ��� ������� ���
    private bool hasStartedFight = false;

    private void Start()
    {

        // ����� ���������� 
        //spawnManager.SpawnPlayerCharacters();
        //spawnManager.SpawnEnemyCharacters();

        //GameManager gameManager = FindObjectOfType<GameManager>();
    }
    private void Update()
    {
        if(gameManager.isFight == true && !hasStartedFight)
        {
        // �������� TurnManager (���������� ���)
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
