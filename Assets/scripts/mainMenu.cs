using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public GameObject settingsPanel;

    private void Start()
    {
        settingsPanel.SetActive(false);
    }
    public void NewGame()
    {
        SceneManager.LoadScene(1);

    }
    public void Continue()
    {

    }

    public void SettingsOpenClose()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
