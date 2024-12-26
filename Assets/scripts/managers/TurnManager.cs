using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour // �������� ����������� �����
{
    public List<basePers> characters = new List<basePers>();
    int currentTurnIndex = 0;
    
    public static TurnManager Instance { get; private set; }




    [SerializeField]
    private GameObject attacker; // ��������� ��������
    [SerializeField]
    private GameObject target;   // ���� 
    public float animationDuration = 0.5f; // ������������ ��������
    public float moveDistance = 0.5f; // ������� �� ������� ���������� ��������� (����� ���������)
    public float rotationAngle = -10f; // ���� ������� ����������

    private Vector3 originalAttackerPos;
    private Vector3 originalTargetPos;
    private Quaternion originalAttackerRotation;
    private Quaternion originalTargetRotation;





    private void Awake()
    {
        // ���������, ����� ��������� ��� ������������
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("������������� ��������� TurnManager ������! �����������.");
            Destroy(gameObject);
        }
    }


    public bool IsNewTurn { get; private set; } // ��� ������������ ������ ����

    /*private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f); // �������� ��������, ����� ��������� ������ ������������

        InitializeCharacters();
        StartTurn();
    }*/
    public void InitializeCharacters()
    {
        // �������� ��� ������� ����������, ����������� �� �����
        CharacterController[] allCharacters = FindObjectsOfType<CharacterController>();
        // ��������� ������� ��������� � ������
        foreach (var character in allCharacters)
        {
            characters.Add(character.persData);
        }

        // ���������� ���������� �� ��������
        characters = characters.OrderByDescending(c => c.speed).ToList();
    }
    

    public void StartTurn()
    {
        IsNewTurn = false;

        var currentCharacter = characters[currentTurnIndex]; // ������� ��������
        currentCharacter.hasTurn = true; // ��������� ���� ���������
        currentCharacter.SetLight(true); // �������� ����� ��� ������� ��� �������� ���������


        if (currentCharacter.isPlayer) // ���� �������� ������
        {
            FindObjectOfType<AbilitiesPanelController>().ResetAbilityUsage(); // ����� ������������� ������������
            FindObjectOfType<AbilitiesPanelController>().UpdateAbilityIcons(currentCharacter);

            Debug.Log("��� ���������: " + currentCharacter.name);
            Debug.Log("�������� ������� ������ ����� ����");
        }
        else // ������� ����
        {
            if(currentCharacter._enemyType == basePers.enemyType.Knight || 
                currentCharacter._enemyType == basePers.enemyType.Slave || 
                currentCharacter._enemyType == basePers.enemyType.Vuchnik)
            {
                Debug.Log("��� �����: " + currentCharacter.name + " ��� �����: " + currentCharacter._enemyType);
                // ����� ������ �����
                CharacterController currentCharctContrl = FindControllerByName(currentCharacter.name);

                attacker = currentCharctContrl.gameObject;

                // ������� ������ ����� (���������� ������)
                List<CharacterController> playerTargets = new List<CharacterController>();

                // �������� �� ���� ����������
                foreach (var playerPersData in characters)
                {
                    if (playerPersData.isPlayer) // ���� ��� �������� ������
                    {
                        CharacterController playerCharctContrl = FindControllerByName(playerPersData.name);
                        if (playerCharctContrl != null)
                        {
                            playerTargets.Add(playerCharctContrl); // ��������� � ������ ��������� �����
                        }
                    }
                }

                
                
                // ���� ������ ����� �� ������, �������� ��������� ����
                if (playerTargets.Count > 0)
                {
                    int randomIndex = Random.Range(0, playerTargets.Count); // �������� ��������� ������
                    CharacterController randomTarget = playerTargets[randomIndex]; // �������� ��������� ����
                    
                    target = randomTarget.gameObject;
                    
                    // ��������� ��������� ������� � ��������
                    originalAttackerPos = attacker.transform.position;
                    originalTargetPos = target.transform.position;
                    originalAttackerRotation = attacker.transform.rotation;
                    originalTargetRotation = target.transform.rotation;

                    PlayAttackAnimation();

                    // ������� ��������� ����
                    randomTarget.TakeDamage(currentCharctContrl.dmgEnemyKnight());
                    Debug.Log($"{currentCharacter.name} �������� {randomTarget.persData.name}");
                }
                else
                {
                    Debug.LogWarning("������ ����� ����! ���������� ��������� �����.");
                }
            }
            StartCoroutine(CoroTurnEnemy());

        }
    }

    public void EndTurn()
    {
        IsNewTurn = true;

        characters[currentTurnIndex].hasTurn = false; // ����� ���� ���������
        //���� ���� ������� � ����� �� �� �����������
        if(characters[currentTurnIndex].cooldownSecondSkill != 0)
        {
            characters[currentTurnIndex].cooldownSecondSkill--;
        }
        characters[currentTurnIndex].SetLight(false);
        currentTurnIndex = (currentTurnIndex + 1) % characters.Count; // ��������� ������� ���� � ���������� ���������

        //StartCoroutine(GameManager.Instance.CheckBattleOutcome());

        StartTurn();
    }


    // ���������� ���� ��������� (������)
    public void OnEndPlayerTurnButton()
    {
        // ���� ������ ��� ������
        if (characters[currentTurnIndex].isPlayer)
        {
            EndTurn();
        }
        //StartCoroutine(GameManager.Instance.CheckBattleOutcome());
    }

    // ����� �������� ���� �����
    private IEnumerator CoroTurnEnemy()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("���� �������");
        EndTurn();
    }


    // �������� ��������� ��� ������ ���
    public basePers GetActiveCharacter()
    {
        return characters.FirstOrDefault(c => c.hasTurn); // ������� ���������, � �������� ������� ���� hasTurn
    }







    // ����� ��� ������� �������� �����
    public void PlayAttackAnimation()
    {
        StartCoroutine(AttackAnimationCoroutine());
    }
    // �������� ��� �������� �����
    private IEnumerator AttackAnimationCoroutine()
    {
        float elapsedTime = 0f;

        // ��������� �������� � ������� ����������
        Vector3 attackerTargetPos = target.transform.position + new Vector3(-moveDistance, 0, 0);
        Quaternion attackerTargetRotation = Quaternion.Euler(0, 0, rotationAngle);

        // ��������� �������� � ������ ������ ����������
        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;

            // �������� ����������
            attacker.transform.position = Vector3.Lerp(originalAttackerPos, attackerTargetPos, t);

            // ������ ����������
            attacker.transform.rotation = Quaternion.Lerp(originalAttackerRotation, attackerTargetRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ��������� �������� � ���������� ���������� �� �������� �������
        attacker.transform.position = attackerTargetPos;
        attacker.transform.rotation = attackerTargetRotation;

        // ����� �� 0.5 �������, ����� �������� �����������
        yield return new WaitForSeconds(0.5f);

        // ���������� ���������� � ��������� ������� � �������
        elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;

            // ������� ���������� �� �������� �������
            attacker.transform.position = Vector3.Lerp(attackerTargetPos, originalAttackerPos, t);

            // ������� � �������� �������
            attacker.transform.rotation = Quaternion.Lerp(attackerTargetRotation, originalAttackerRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ��������� ����� ���������� �� �������� ������� � ��������
        attacker.transform.position = originalAttackerPos;
        attacker.transform.rotation = originalAttackerRotation;
    }












    // ����� ��� ������ CharacterController �� �����
    public CharacterController FindControllerByName(string name)
    {
        // �������� ���� ������ �� �����
        CharacterController[] allControllers = FindObjectsOfType<CharacterController>();

        // ���� ����� � ������ ������
        return allControllers.FirstOrDefault(controller => controller.persData.name == name);
    }


    // �������� ����� ����� ������
    public void RemoveCharacter(basePers character)
    {
        characters.Remove(character);
        Debug.Log($"{character.name} ������ �� ������� �����.");

        // ��������� ������ �������� ���������
        if (characters.Count > 0)
        {
            currentTurnIndex %= characters.Count;
        }

        
    }
}
