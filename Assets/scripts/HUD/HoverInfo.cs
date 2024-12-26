using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverInfo : MonoBehaviour
{
    [SerializeField]
    private GameObject infoPanel; // ������ ����������
    public TextMeshProUGUI infoTitle; // ��������� ������
    public TextMeshProUGUI infoDescription; // �������� � ������
    [SerializeField]
    private float hoverTime = 3f; // ����� �� ������ ������
    private Coroutine hoverCoroutine; // ��� ������������ ��������
    [SerializeField]
    private bool isHovering = false; // ���� ��������� ���������

    // ��� �������� ���������� �� �������
    [Header("Ability Info")]
    public string abilityName; // �������� �����������
    public string abilityDescription; // �������� �����������

    [Header("Character Info")]
    public string characterName; 
    /*public int characterHP; 
    public int characterMinDmg; 
    public int characterMaxDmg; 
    public int characterProtection; 
    public int characterSpeed;*/ 

    private void Start()
    {
        // ����� ������ 
        // �������� ��� ������� ���� GameObject
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // ��������� �� �����
            if (obj.name == "InfoPanel" && !obj.activeInHierarchy)
            {
                infoPanel = obj;
                break;
            }
        }


        
        if (infoPanel == null)
        {
            Debug.LogError("InfoPanel �� �������!.");
            return;
        }
        Transform transTitle = infoPanel.transform.Find("infoTitle");
        Transform transDescrp= infoPanel.transform.Find("infoDescription");
        infoTitle = transTitle.gameObject.GetComponent<TextMeshProUGUI>();
        infoDescription = transDescrp.gameObject.GetComponent<TextMeshProUGUI>();


        // ����� ����������� ������ ������
        //infoTitle = infoPanel.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        //infoDescription = infoPanel.transform.Find("Description").GetComponent<TextMeshProUGUI>();

        if (infoPanel != null)
        {
            infoPanel.SetActive(false); // ������ ������ �� ���������
        }
    }


    private void Update()
    {
        if(infoPanel == null)
        {
            // �������� ��� ������� ���� GameObject
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                Debug.Log($"�������� ������: {obj.name}");
                if (obj.name == "InfoPanel" && !obj.activeInHierarchy)
                {
                    infoPanel = obj;
                    Debug.Log("InfoPanel �������!");
                    break;
                }
            }


            Transform transTitle = infoPanel.transform.Find("infoTitle");
            Transform transDescrp = infoPanel.transform.Find("infoDescription");
            infoTitle = transTitle.gameObject.GetComponent<TextMeshProUGUI>();
            infoDescription = transDescrp.gameObject.GetComponent<TextMeshProUGUI>();
        }
    }

    // ��������� ������ ��� EventTrigger
    public void OnHoverEnter(BaseEventData eventData)
    {
        Debug.Log("��������� ��������!");
        isHovering = true;
        if (hoverCoroutine == null)
        {
            hoverCoroutine = StartCoroutine(ShowInfoAfterDelay());
        }
    }

    public void OnHoverExit(BaseEventData eventData)
    {
        Debug.Log("��������� ���������!");

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
            else if (characterName != "")
            {
                CharacterController charControl = this.GetComponent<CharacterController>();
                infoTitle.text = characterName;
                infoDescription.text = $"��: {charControl.persData.hp} \n " +
                    $"����: {charControl.persData.minDamage} - {charControl.persData.maxDamage} \n" +
                    $"������: {charControl.persData.protection} \n" +
                    $"��������: {charControl.persData.speed} \n";
            }
        }
    }
}
