using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInitializer : MonoBehaviour
{
    //public spawnManager spawnManager;
    public FadeInOut fadeInOut;
    public Camera mainCamera;
    public TurnManager turnManager;
    public GameObject abilitiesPanel; // ������ � ��������� �������������

    private IEnumerator Start()
    {
        // ����� ����������
        //spawnManager.SpawnPlayerCharacters();
        //spawnManager.SpawnEnemyCharacters();

        // ���� 3 ������� ����� ������
        yield return new WaitForSeconds(3);
        //
        
        yield return StartCoroutine(fadeInOut.FadeOut());
        yield return new WaitForSeconds(1);
        abilitiesPanel.SetActive(true);
        yield return StartCoroutine(fadeInOut.FadeIn());

        // ����������� ������
        FocusCameraOnBattle();

        // �������� TurnManager(���������� ���)
        turnManager.InitializeCharacters();
        turnManager.StartTurn();
    }

    void FocusCameraOnBattle()
    {
        // ������ ����������� ������ � ��� 
        mainCamera.transform.position = new Vector3(0, 0, -10); // ������ �������
    }
}
