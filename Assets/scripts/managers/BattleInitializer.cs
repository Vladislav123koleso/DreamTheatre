using System.Collections;
using UnityEngine;

public class BattleInitializer : MonoBehaviour
{
    public FadeInOut fadeInOut;
    public Camera mainCamera;
    public TurnManager turnManager;
    public GameObject abilitiesPanel; // ������ � ��������� �������������

    private IEnumerator Start()
    {
        // ����� ���������� (���������������� ��� �������������)
        //spawnManager.SpawnPlayerCharacters();
        //spawnManager.SpawnEnemyCharacters();

        // ���� 3 ������� ����� ������
        yield return new WaitForSeconds(3);

        // ������ ����������
        yield return StartCoroutine(fadeInOut.FadeOut());
        yield return new WaitForSeconds(1);

        abilitiesPanel.SetActive(true);

        // ����������� ������
        StartCoroutine(FocusCameraOnBattle());
        
        yield return StartCoroutine(fadeInOut.FadeIn());


        // �������� TurnManager (���������� ���)
        turnManager.InitializeCharacters();
        turnManager.StartTurn();
    }

    IEnumerator FocusCameraOnBattle()
    {
        float targetSize = 5f; // ������� ������ ������ ��� �����������
        float duration = 0f; // ������������ �����������
        float initialSize = mainCamera.orthographicSize;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            mainCamera.orthographicSize = Mathf.Lerp(initialSize, targetSize, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.orthographicSize = targetSize;
    }
}
