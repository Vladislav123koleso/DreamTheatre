using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseLogic : MonoBehaviour
{
    public GameObject pausePanel;
    [SerializeField]
    private bool isPaused = false;


    private void Start()
    {
        pausePanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused; // Переключаем состояние паузы

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0; // Останавливаем время
        pausePanel.SetActive(true); // Показываем меню паузы
        
    }

    private void ResumeGame()
    {
        Time.timeScale = 1; // Возобновляем время
        
        pausePanel.SetActive(false); // Скрываем меню паузы
        
    }

    public void enablePausePanel()
    {
        if(Time.timeScale != 0)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        pausePanel.SetActive(!pausePanel.activeSelf);
    }
    public void backToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
