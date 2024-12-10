using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverInfo : MonoBehaviour
{
    public GameObject infoPanel; // Панель информации
    public TextMeshProUGUI infoTitle; // Заголовок панели
    public TextMeshProUGUI infoDescription; // Описание в панели

    public float hoverTime = 1f; // Время до показа панели
    private Coroutine hoverCoroutine; // Для отслеживания корутины

    private bool isHovering = false; // Флаг состояния наведения

    // Для хранения информации об объекте
    [Header("Ability Info")]
    public string abilityName; // Название способности
    public string abilityDescription; // Описание способности

    [Header("Character Info")]
    public string characterName; 
    public int characterHP; 
    public int characterMinDmg; 
    public int characterMaxDmg; 
    public int characterProtection; 
    public int characterSpeed; 

    private void Start()
    {
        // Поиск панели по тегу или в иерархии (используйте подходящий вариант)
        infoPanel = GameObject.FindWithTag("InfoPanel"); // Убедитесь, что у панели есть тег InfoPanel
        if (infoPanel == null)
        {
            Debug.LogError("InfoPanel не найдена! Убедитесь, что у панели установлен тег 'InfoPanel'.");
            return;
        }

        // Поиск компонентов внутри панели
        infoTitle = infoPanel.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        infoDescription = infoPanel.transform.Find("Description").GetComponent<TextMeshProUGUI>();

        if (infoPanel != null)
        {
            infoPanel.SetActive(false); // Панель скрыта по умолчанию
        }
    }

    // Публичные методы для EventTrigger
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
