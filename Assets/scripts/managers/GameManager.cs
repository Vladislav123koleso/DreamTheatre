using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
//using UnityEditor.VersionControl;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera mainCamera;
    public FadeInOut fadeInOut;
    public GameObject abilitiesPanel; // ������ � ��������� �������������
    private float defaultCameraSize; // ��� �������� ���������� ������� ������

    public GameObject losePanel;
    public GameObject winPanel;

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
        yield return StartCoroutine(TypeText(intro, introText));*/



        // �������� ������ ���������� ������ � ������
        FindCharacters();


        yield return new WaitForSeconds(1);

        yield return StartCoroutine(fadeInOut.FadeIn());
        //-------------------------------

        //---------------------------
        //����� ���������� �������
        // ������� � ������� ����������
        yield return StartCoroutine(DialogueSequence());


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
        //

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
        yield return new WaitForSeconds(3);
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
        GameObject mag = players[1]; // 2�� ����� - ���
        GameObject enemy = enemies[0];  // ������ ����

        // �������� ������� ���������� (������ �������� ������)
        Transform playerCanvas = player.transform.GetChild(1); // ������ ������
        Transform magCanvas = mag.transform.GetChild(1); // ������ ������
        Transform enemyCanvas = enemy.transform.GetChild(1);   // ������ �����

        // �������� ���������� TextMeshProUGUI
        TextMeshProUGUI playerDialogueText = playerCanvas.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI magDialogueText = magCanvas.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI enemyDialogueText = enemyCanvas.GetComponentInChildren<TextMeshProUGUI>();
        
        StartCoroutine(TypeText("", introText));

        // ������ 2
        yield return StartCoroutine(TypeText("���: � �������, ��� �� ������� ������ �������� ������, � ������ ����.", magDialogueText));
        yield return new WaitForSeconds(1); // ����� ����� ���������

        yield return StartCoroutine(TypeText("�����: ������ ����� �����", playerDialogueText));
        yield return new WaitForSeconds(1); // ����� ����� ���������

        yield return StartCoroutine(TypeText("����: ����. ������ �� �� ��������.", enemyDialogueText));
        yield return new WaitForSeconds(1);

        // ����� ����� �������
        yield return new WaitForSeconds(1);
        //dialogueText.text = "";
    }
    
    private IEnumerator ThirdDialogueSequence()
    {
        // �������� ���������� ��� �������
        GameObject player = players[0]; // ������ �����
        GameObject mag = players[1]; // 2�� ����� - ���
        GameObject enemy = enemies[0];  // ������ ����
        GameObject boss = enemies[2];  // ��������

        // �������� ������� ���������� (������ �������� ������)
        Transform playerCanvas = player.transform.GetChild(1); // ������ ������
        Transform magCanvas = mag.transform.GetChild(1); // ������ ������
        Transform enemyCanvas = enemy.transform.GetChild(1);   // ������ �����
        Transform bossCanvas = boss.transform.GetChild(1);   // ������ �����

        // �������� ���������� TextMeshProUGUI
        TextMeshProUGUI playerDialogueText = playerCanvas.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI magDialogueText = magCanvas.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI enemyDialogueText = enemyCanvas.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI bossDialogueText = bossCanvas.GetComponentInChildren<TextMeshProUGUI>();

        yield return StartCoroutine(TypeText("��������: ��� ��� ���. ��� ��� �� ��� ���������?", bossDialogueText));
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(TypeText("��������: �������. ���������� � ������.", bossDialogueText));
        yield return new WaitForSeconds(1);


        yield return new WaitForSeconds(1);
    }

    private IEnumerator FourDialogueSequence()
    {
        GameObject player = players[0]; // ������ �����
        Transform playerCanvas = player.transform.GetChild(1);
        TextMeshProUGUI playerDialogueText = playerCanvas.GetComponentInChildren<TextMeshProUGUI>();

        yield return StartCoroutine(TypeText("�����: ������ ����� ������� �������� ��������", playerDialogueText));
        yield return new WaitForSeconds(1); // ����� ����� ���������


        yield return new WaitForSeconds(1);
    }


        public IEnumerator CheckBattleOutcome()
    {
        while (isFight)
        {
            int aliveEnemies = 0;
            foreach (var enemy in enemies)
            {
                if (enemy != null)  // ���������, ��� �� ����
                {
                    aliveEnemies++;
                }
            }

            yield return new WaitForSeconds(1);

            // �������� ������
            if (aliveEnemies == 0)
            {
                battleCounter++;
                Debug.Log("����� �������!");
                HealAllPlayers(); // �������� ���� �������
                EndFight();

                // ����� ������ ������ �������� ������ ���
                if (battleCounter == 1)
                {
                    spawnManager.spawnMag();

                    yield return new WaitForSeconds(1);
                    yield return StartCoroutine(fadeInOut.FadeOut());
                    yield return StartCoroutine(TypeText("����� �� ����: �� ���� ��������� ������ ����������� �� ����� ����, ����, ����������. �� �������� �� �� ����� ������� � ������ ������� ��������?...", introText));
                    spawnManager.SpawnEnemyCharacters(); // ������� ������
                    yield return new WaitForSeconds(1);
                    yield return StartCoroutine(fadeInOut.FadeIn());

                    // ����� ������ ����� ������ ������
                    yield return StartCoroutine(SecondDialogueSequence());
                    yield return new WaitForSeconds(1);

                    yield return StartCoroutine(fadeInOut.FadeOut());
                    yield return StartCoroutine(fadeInOut.FadeIn());
                    yield return new WaitForSeconds(1);

                    abilitiesPanel.SetActive(true);
                    StartCoroutine(FocusCameraOnBattle());
                    isFight = true;


                    // ����� ������� ��� ����� ��������� �����
                    yield return StartCoroutine(CheckBattleOutcome()); // ��������� �������� ����� ������� ���
                }
                else if(battleCounter == 2) // ���� � ������
                {

                    yield return new WaitForSeconds(1);
                    yield return StartCoroutine(fadeInOut.FadeOut());
                    yield return StartCoroutine(TypeText("����� �� ����: �� ��� ������ � ����� ����...", introText));

                    spawnManager.SpawnEnemyCharacters(); // ������� ������

                    yield return StartCoroutine(fadeInOut.FadeIn());
                    yield return new WaitForSeconds(1);
                    yield return StartCoroutine(ThirdDialogueSequence());

                    abilitiesPanel.SetActive(true);
                    StartCoroutine(FocusCameraOnBattle());
                    isFight = true;


                    // ����� ��� ����� ��������� �����
                    yield return StartCoroutine(CheckBattleOutcome());
                }
                else if(battleCounter == 3)
                {
                    yield return StartCoroutine(FourDialogueSequence());
                    yield return new WaitForSeconds(2);
                    winPanel.SetActive(true);
                    // ������ ������ � ����� ����
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
                yield break; // ������� �� ��������
            }
        }
    }

    private void EndFight()
    {
        isFight = false;
        //�������� �� ������ ������ � ������� �� �� �����
        foreach(var enemy in enemies)
        {
            Destroy(enemy);
        }
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
        FindCharacters();

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
