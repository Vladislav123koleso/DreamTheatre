using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverInfo : MonoBehaviour
{
    [SerializeField]
    private GameObject infoPanel; // Панель информации
    public TextMeshProUGUI infoTitle; // Заголовок панели
    public TextMeshProUGUI infoDescription; // Описание в панели
    [SerializeField]
    private float hoverTime = 3f; // Время до показа панели
    private Coroutine hoverCoroutine; // Для отслеживания корутины
    [SerializeField]
    private bool isHovering = false; // Флаг состояния наведения

    // Для хранения информации об объекте
    [Header("Ability Info")]
    public string abilityName; // Название способности
    public string abilityDescription; // Описание способности

    [Header("Character Info")]
    public string characterName; 
    /*public int characterHP; 
    public int characterMinDmg; 
    public int characterMaxDmg; 
    public int characterProtection; 
    public int characterSpeed;*/ 

    private void Start()
    {
        // Поиск панели 
        // Получаем все объекты типа GameObject
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Проверяем по имени
            if (obj.name == "InfoPanel" && !obj.activeInHierarchy)
            {
                infoPanel = obj;
                break;
            }
        }


        
        if (infoPanel == null)
        {
            Debug.LogError("InfoPanel не найдена!.");
            return;
        }
        Transform transTitle = infoPanel.transform.Find("infoTitle");
        Transform transDescrp= infoPanel.transform.Find("infoDescription");
        infoTitle = transTitle.gameObject.GetComponent<TextMeshProUGUI>();
        infoDescription = transDescrp.gameObject.GetComponent<TextMeshProUGUI>();


        // Поиск компонентов внутри панели
        //infoTitle = infoPanel.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        //infoDescription = infoPanel.transform.Find("Description").GetComponent<TextMeshProUGUI>();

        if (infoPanel != null)
        {
            infoPanel.SetActive(false); // Панель скрыта по умолчанию
        }
    }


    private void Update()
    {
        if(infoPanel == null)
        {
            // Получаем все объекты типа GameObject
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                Debug.Log($"Проверяю объект: {obj.name}");
                if (obj.name == "InfoPanel" && !obj.activeInHierarchy)
                {
                    infoPanel = obj;
                    Debug.Log("InfoPanel найдена!");
                    break;
                }
            }


            Transform transTitle = infoPanel.transform.Find("infoTitle");
            Transform transDescrp = infoPanel.transform.Find("infoDescription");
            infoTitle = transTitle.gameObject.GetComponent<TextMeshProUGUI>();
            infoDescription = transDescrp.gameObject.GetComponent<TextMeshProUGUI>();
        }
    }

    // Публичные методы для EventTrigger
    public void OnHoverEnter(BaseEventData eventData)
    {
        Debug.Log("Наведение началось!");
        isHovering = true;
        if (hoverCoroutine == null)
        {
            hoverCoroutine = StartCoroutine(ShowInfoAfterDelay());
        }
    }

    public void OnHoverExit(BaseEventData eventData)
    {
        Debug.Log("Наведение завершено!");

        isHovering = false;
        if (hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine);
            hoverCoroutine = null;
        }

        if (infoPanel != null)
        {
            infoPanel.SetActive(false); // Скрываем панель
        }
    }

    // Показ панели после задержки
    private IEnumerator ShowInfoAfterDelay()
    {
        yield return new WaitForSeconds(hoverTime);

        if (isHovering && infoPanel != null)
        {
            UpdateInfoPanel();
            infoPanel.SetActive(true); // Показываем панель
        }
    }

    // Обновление содержимого панели
    private void UpdateInfoPanel()
    {
        if (infoTitle != null && infoDescription != null)
        {
            // Если у объекта есть информация о способности
            if (!string.IsNullOrEmpty(abilityName))
            {
                infoTitle.text = abilityName;
                infoDescription.text = abilityDescription;
            }
            // Если у объекта есть информация о персонаже
            else if (characterName != "")
            {
                CharacterController charControl = this.GetComponent<CharacterController>();
                infoTitle.text = characterName;
                infoDescription.text = $"ХП: {charControl.persData.hp} \n " +
                    $"Урон: {charControl.persData.minDamage} - {charControl.persData.maxDamage} \n" +
                    $"Защита: {charControl.persData.protection} \n" +
                    $"Скорость: {charControl.persData.speed} \n";
            }
        }
    }
}
