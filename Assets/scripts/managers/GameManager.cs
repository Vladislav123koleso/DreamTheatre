using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEditor.VersionControl;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera mainCamera;
    public FadeInOut fadeInOut;
    public GameObject abilitiesPanel; // ������ � ��������� �������������
    private float defaultCameraSize; // ��� �������� ���������� ������� ������

    public GameObject losePanel;

    public bool isFight = false; // ��� ������������ ���������� ���

    public TextMeshProUGUI introText;  // ����� ��������
    public float typingSpeed = /*0.05f*/ 1f;     // �������� ��������� ������

    public TextMeshProUGUI dialogueText;  // ��� ������ ������ �������
    public spawnManager spawnManager;

    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> players = new List<GameObject>();


    
    public int battleCounter = 0; // ������� ����

    public static GameManager Instance { get; private set; }

    private IEnumerator Start()
    {
        
        abilitiesPanel.SetActive(false);
        // ��������� ��������� ������ ������
        defaultCameraSize = mainCamera.orthographicSize;

        // ���� ... ������� ����� ������
        //yield return new WaitForSeconds(0);
        // ������ ����������
        yield return StartCoroutine(fadeInOut.FadeOut());




        /*string intro = "��������� �������� ���� ����� ��������. " +
            "��������� � ������� ������ ��� ������, ���� �� ����, ������. " +
            "������ � �������� ��������� ��� �����, �� �������� ����� ���� ������������ -- " +
            "�� ������� ������ ����� ����������� ���-�� ���������� ��������. �" +
            "��-��, ���� �� ������ ���� ����. ���� ������������ � ����� ����� �������� ����� ���� �� ������� ����������������� �����.";
        yield return StartCoroutine(TypeText(intro, introText));
*/


        // �������� ������ ���������� ������ � ������
        FindCharacters();


        yield return new WaitForSeconds(1);

        yield return StartCoroutine(fadeInOut.FadeIn());
        //-------------------------------

        //---------------------------
        //����� ���������� �������
        // ������� � ������� ����������
        // yield return StartCoroutine(DialogueSequence());


        //----------------------------------
        //����� ������� ���������� ���
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(fadeInOut.FadeOut());
        yield return StartCoroutine(TypeText("����� �� ����: ������ ����������. ������ ���� �������", introText));
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(fadeInOut.FadeIn());

        abilitiesPanel.SetActive(true);
        StartCoroutine(FocusCameraOnBattle());
        isFight = true;


        StartCoroutine(CheckBattleOutcome());
    }





    private void FindCharacters()
    {
        // ������� ������� ������ ������ (���� �����)
        enemies.Clear();

        // ������� ��� ������� � ����� "Enemy"
        GameObject[] foundEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        // ��������� �� � ������
        foreach (GameObject enemy in foundEnemies)
        {
            enemies.Add(enemy);
        }

        // ������� ������� ������ ������ (���� �����)
        players.Clear();

        // ������� ��� ������� � ����� "Enemy"
        GameObject[] foundPlayers = GameObject.FindGameObjectsWithTag("Player");

        // ��������� �� � ������
        foreach (GameObject player in foundPlayers)
        {
            players.Add(player);
        }
    }


    private IEnumerator DialogueSequence()
    {
        // �������� ���������� ��� �������
        GameObject player = players[0]; // ������ �����
        GameObject enemy = enemies[0];  // ������ ����

        // �������� ������� ���������� (������ �������� ������)
        Transform playerCanvas = player.transform.GetChild(1); // ������ ������
        Transform enemyCanvas = enemy.transform.GetChild(1);   // ������ �����

        // �������� ���������� TextMeshProUGUI
        TextMeshProUGUI playerDialogueText = playerCanvas.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI enemyDialogueText = enemyCanvas.GetComponentInChildren<TextMeshProUGUI>();


        StartCoroutine(TypeText("��������: ���� ������ ���������� ��������� � ����������� " +
            "��������. �������� ����������� ������ �����.",introText));
        yield return new WaitForSeconds(2);
        // ������ 1 (����� � 1 �� ������)
        yield return StartCoroutine(TypeText("�����: �������� ����� � ���, �� �� ����� ��������� �����! ������ ����� �� ��������? ������ ����� �� ��������, ��������� ��� ������?", playerDialogueText));
        yield return new WaitForSeconds(1); // ����� ����� ���������

        yield return StartCoroutine(TypeText("�����: � ����������� ��������... �� �������� ��.� ������ ����� ����-��, ��� ������� ���������� �.", playerDialogueText));
        yield return new WaitForSeconds(1); 

        yield return StartCoroutine(TypeText("�����: ��,��������, ��� ����������� ��������� � �������� �����������, �����? � ��� ���� ���� ������������� �������� �����, �� �������? � ����������� ����, ��...", playerDialogueText));
        //yield return new WaitForSeconds(1);

        yield return StartCoroutine(TypeText("����: �� ������ ���-�� ������ ����� ��������?", enemyDialogueText));
        yield return new WaitForSeconds(1);

        yield return StartCoroutine(TypeText("�����: � ���� ��� ����� �����������?", playerDialogueText));
        yield return new WaitForSeconds(1);

        yield return StartCoroutine(TypeText("�����: ��� ��, � �� ���� � ���� ������ ������, �� ������ �������� ��� �����, ����...", playerDialogueText));
        yield return new WaitForSeconds(1);

        yield return StartCoroutine(TypeText("����: ���� ��������!", enemyDialogueText));
        yield return new WaitForSeconds(1);

        // ����� ����� �������
        //yield return new WaitForSeconds(1);
        //dialogueText.text = "";
    }

    private IEnumerator SecondDialogueSequence()
    {
        // �������� ���������� ��� �������
        GameObject player = players[0]; // ������ �����
        GameObject mag = players[0]; // 2�� ����� - ���
        GameObject enemy = enemies[0];  // ������ ����

        // �������� ������� ���������� (������ �������� ������)
        Transform playerCanvas = player.transform.GetChild(1); // ������ ������
        Transform magCanvas = mag.transform.GetChild(1); // ������ ������
        Transform enemyCanvas = enemy.transform.GetChild(1);   // ������ �����

        // �������� ���������� TextMeshProUGUI
        TextMeshProUGUI playerDialogueText = playerCanvas.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI magDialogueText = magCanvas.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI enemyDialogueText = enemyCanvas.GetComponentInChildren<TextMeshProUGUI>();


        // ������ 1 (����� � 1 �� ������)
        yield return StartCoroutine(TypeText("�����: ����� ��� �� ���� ���������", playerDialogueText));
        yield return new WaitForSeconds(1); // ����� ����� ���������

        yield return StartCoroutine(TypeText("���: ��....", magDialogueText));
        yield return new WaitForSeconds(1); // ����� ����� ���������

        yield return StartCoroutine(TypeText("�����: ��� �� ����� ������", playerDialogueText));
        yield return new WaitForSeconds(1);

        

        // ����� ����� �������
        yield return new WaitForSeconds(1);
        //dialogueText.text = "";
    }



    public IEnumerator CheckBattleOutcome()
    {
        while (isFight)
        {
            yield return new WaitForSeconds(1);

            // �������� ������
            if (enemies.Count == 0)
            {
                battleCounter++;
                Debug.Log("����� �������!");
                HealAllPlayers(); // �������� ���� �������
                EndFight();
                if(battleCounter == 1) // ��� ������ � ������ �����
                {
                    spawnManager.spawnMag();
                    yield return StartCoroutine(SecondDialogueSequence()); // �������� ����� ������

                    yield return new WaitForSeconds(1);
                    yield return StartCoroutine(fadeInOut.FadeOut());
                    yield return StartCoroutine(TypeText("����� �� ����: sgjshkqworpoejkdsk", introText));
                    
                    abilitiesPanel.SetActive(true);
                    StartCoroutine(FocusCameraOnBattle());
                    isFight = true;
                    
                    yield return new WaitForSeconds(1);
                    yield return StartCoroutine(fadeInOut.FadeIn());



                    StartCoroutine(CheckBattleOutcome());

                }
                yield break; // ������� �� ��������
            }

            // �������� ���������
            if (players.Count == 0)
            {
                Debug.Log("����� ��������!");
                yield return new WaitForSeconds(2);
                losePanel.SetActive(true);
                EndFight();
                //yield return StartCoroutine(TypeText("�� ���������... ����� ����.", introText));
                yield break; // ������� �� ��������
            }
        }
    }

    private void EndFight()
    {
        isFight = false;
        abilitiesPanel.SetActive(false); // �������� ������ ������������
        ResetCameraToDefault(); // ���������� ������ � ������������ ���������
    }

    // ����� ���������� ������� ������ � ������� ����� �� ��������
    public void UpdateCharacterLists()
    {
        // ������� ������� ���������� �� �������
        enemies.RemoveAll(enemy => enemy == null || !enemy.activeInHierarchy);
        players.RemoveAll(player => player == null || !player.activeInHierarchy);
    }

    
    private void Update()
    {
        if (isFight)
        {
            UpdateCharacterLists();
        }
        //CheckBattleOutcome();
        
    }



    private void HealAllPlayers()
    {
        // ������� ���� ������� �� �����
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            // �������� ������ CharacterController � ������� ������
            CharacterController characterController = player.GetComponent<CharacterController>();
            if (characterController != null)
            {
                // �������� ����� Heal
                characterController.Heal(characterController.persData.max_hp);
            }
        }
    }

    //--------------------------------------------------------------------------
    // ����� ������ �� �����
    private IEnumerator TypeText(string message, TextMeshProUGUI targetText)
    {
        targetText.text = "";  // ������� ����� ����� �������

        foreach (char letter in message.ToCharArray())
        {
            targetText.text += letter;  // ��������� ������
            yield return new WaitForSeconds(typingSpeed);  // �������� ����� ���������
        }
        yield return new WaitForSeconds(2);
        targetText.text = "";
    }

    // ������ � �������
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
    // ����� ��� �������� ������ � ���������� �������
    public void ResetCameraToDefault()
    {
        StartCoroutine(ResetCameraSize());
    }

    private IEnumerator ResetCameraSize()
    {
        float duration = 1f; // �����, �� ������� ������ �������� � ���������� �������
        float initialSize = mainCamera.orthographicSize;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            mainCamera.orthographicSize = Mathf.Lerp(initialSize, defaultCameraSize, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.orthographicSize = defaultCameraSize; // ���������, ��� ������ ����������� �� ������ ��������
    }

}
