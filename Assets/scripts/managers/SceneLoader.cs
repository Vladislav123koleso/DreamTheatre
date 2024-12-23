using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public string sceneToLoad; // Название сцены для загрузки
    public Slider loadingBar;  // Слайдер для индикатора загрузки
    public float minLoadingTime = 3f; // Минимальное время, которое должен быть виден экран загрузки (в секундах)
    public float loadSpeed = 1f; // Скорость изменения слайдера

    private void Start()
    {
        // Начинаем асинхронную загрузку сцены
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        // Начинаем асинхронную загрузку сцены
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);

        // Отключаем автоматическую активацию сцены
        asyncLoad.allowSceneActivation = false;

        float elapsedTime = 0f;  // Время загрузки

        // Пока сцена не загружена на 100%
        while (!asyncLoad.isDone)
        {
            // Обновляем индикатор загрузки плавно
            float targetProgress = asyncLoad.progress < 0.9f ? asyncLoad.progress : 1f;

            // Плавное увеличение значения слайдера
            float currentProgress = loadingBar.value;
            loadingBar.value = Mathf.MoveTowards(currentProgress, targetProgress, loadSpeed * Time.deltaTime);

            // Когда прогресс загрузки будет 90%, можно активировать сцену
            if (asyncLoad.progress >= 0.9f)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= minLoadingTime)
                {
                    asyncLoad.allowSceneActivation = true; // Активируем сцену
                }
            }

            yield return null;
        }
    }
}
