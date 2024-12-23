using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera mainCamera;
    public FadeInOut fadeInOut;
    public GameObject abilitiesPanel; // Панель с активными способностями
    private float defaultCameraSize; // Для хранения дефолтного размера камеры

    public bool isFight = false; // для отслеживания активности боя

    public TextMeshProUGUI introText;  // текст заставки
    public float typingSpeed = 0.05f;     // Скорость появления текста

    private IEnumerator Start()
    {
        
        abilitiesPanel.SetActive(false);
        // Сохраняем дефолтный размер камеры
        defaultCameraSize = mainCamera.orthographicSize;

        // Ждем ... секунды после спавна
        //yield return new WaitForSeconds(0);
        // Эффект затемнения
        yield return StartCoroutine(fadeInOut.FadeOut());

        string intro = "Голос во тьме: Добро пожаловать на сцену Театра смерти, " +
            "здесь вы познаете отчаяние и смерть. Пьесса о вашей смерти будет сыграна здесь";
        yield return StartCoroutine(TypeText(intro));


        yield return new WaitForSeconds(1);

        yield return StartCoroutine(fadeInOut.FadeIn());
        //-------------------------------

        //---------------------------
        //здесь диалоговая система

        //----------------------------------
        //после диалога начинается бой
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(fadeInOut.FadeOut());
        yield return StartCoroutine(TypeText("Голос во тьме: Сценка начинается. Выживи если сможешь"));
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(fadeInOut.FadeIn());

        abilitiesPanel.SetActive(true);
        StartCoroutine(FocusCameraOnBattle());
        isFight = true;



    }











    // вывод текста по букве
    private IEnumerator TypeText(string message)
    {
        introText.text = "";  // Очищаем текст перед набором

        foreach (char letter in message.ToCharArray())
        {
            introText.text += letter;  // Добавляем символ
            yield return new WaitForSeconds(typingSpeed);  // Задержка между символами
        }
        yield return new WaitForSeconds(2);
        introText.text = "";
    }

    // работа с камерой
    IEnumerator FocusCameraOnBattle()
    {
        float targetSize = 5f; // Целевой размер камеры для приближения
        float duration = 0f; // Длительность приближения
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
    // Метод для возврата камеры к дефолтному размеру
    public void ResetCameraToDefault()
    {
        StartCoroutine(ResetCameraSize());
    }

    private IEnumerator ResetCameraSize()
    {
        float duration = 1f; // Время, за которое камера вернется к дефолтному размеру
        float initialSize = mainCamera.orthographicSize;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            mainCamera.orthographicSize = Mathf.Lerp(initialSize, defaultCameraSize, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.orthographicSize = defaultCameraSize; // Убедитесь, что камера установлена на точное значение
    }

}
