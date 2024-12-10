using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverInfo : MonoBehaviour
{
    public GameObject infoPanel; // ������ ����������
    public TextMeshProUGUI infoTitle; // ��������� ������
    public TextMeshProUGUI infoDescription; // �������� � ������

    public float hoverTime = 1f; // ����� �� ������ ������
    private Coroutine hoverCoroutine; // ��� ������������ ��������

    private bool isHovering = false; // ���� ��������� ���������

    // ��� �������� ���������� �� �������
    [Header("Ability Info")]
    public string abilityName; // �������� �����������
    public string abilityDescription; // �������� �����������

    [Header("Character Info")]
    public string characterName; 
    public int characterHP; 
    public int characterMinDmg; 
    public int characterMaxDmg; 
    public int characterProtection; 
    public int characterSpeed; 

    private void Start()
    {
        // ����� ������ �� ���� ��� � �������� (����������� ���������� �������)
        infoPanel = GameObject.FindWithTag("InfoPanel"); // ���������, ��� � ������ ���� ��� InfoPanel
        if (infoPanel == null)
        {
            Debug.LogError("InfoPanel �� �������! ���������, ��� � ������ ���������� ��� 'InfoPanel'.");
            return;
        }

        // ����� ����������� ������ ������
        infoTitle = infoPanel.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        infoDescription = infoPanel.transform.Find("Description").GetComponent<TextMeshProUGUI>();

        if (infoPanel != null)
        {
            infoPanel.SetActive(false); // ������ ������ �� ���������
        }
    }

    // ��������� ������ ��� EventTrigger
    public void OnHoverEnter(BaseEventData eventData)
    {
        isHovering = true;
        if (hoverCoroutine == null)
        {
            hoverCoroutine = StartCoroutine(ShowInfoAfterDelay());
        }
    }

    public void OnHoverExit(BaseEventData eventData)
    {
        isHovering = false;
        if (hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine);
            hoverCoroutine = null;
        }

        if (infoPanel != null)
        {
            infoPanel.SetActive(false); // �������� ������
        }
    }

    // ����� ������ ����� ��������
    private IEnumerator ShowInfoAfterDelay()
    {
        yield return new WaitForSeconds(hoverTime);

        if (isHovering && infoPanel != null)
        {
            UpdateInfoPanel();
            infoPanel.SetActive(true); // ���������� ������
        }
    }

    // ���������� ����������� ������
    private void UpdateInfoPanel()
    {
        if (infoTitle != null && infoDescription != null)
        {
            // ���� � ������� ���� ���������� � �����������
            if (!string.IsNullOrEmpty(abilityName))
            {
                infoTitle.text = abilityName;
                infoDescription.text = abilityDescription;
            }
            // ���� � ������� ���� ���������� � ���������
            else if (!string.IsNullOrEmpty(characterName))
            {
                infoTitle.text = characterName;
                infoDescription.text = $"HP: {characterHP} \n " +
                    $"Damage: {characterMinDmg} - {characterMaxDmg} \n" +
                    $"Protection: {characterProtection} \n" +
                    $"Speed: {characterSpeed} \n";
            }
        }
    }
}
