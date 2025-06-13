using System;
using Levels;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(SaveManager.LoadProgress().currentLevelName);
    }
    
    public void OpenSettings()
    {
        SettingMenu.levelId = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("settings");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
