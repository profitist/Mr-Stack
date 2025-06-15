using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class MainMenuManager : MonoBehaviour
{
    private void Awake()
    {
        Cursor.visible = true;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("levels");
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
