using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScript : MonoBehaviour
{
    private Canvas deathMenu;
    private void Awake()
    {
        deathMenu = GetComponent<Canvas>();
        deathMenu.enabled = false;
    }
    
    private void Update()
    {
        
        if (GameInput.IsDead)
        {
            deathMenu.enabled = true;
            Time.timeScale = 0;
        }
    }
    
    public void ExitGame()
    {
        deathMenu.enabled = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("mainMenu");
    }

    public void RestartGame()
    {
        deathMenu.enabled = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void OpenSettings()
    {
        GameInput.Instance.playerInputActions.Enable();
        deathMenu.enabled = false;
        Time.timeScale = 1;
        SettingMenu.levelId = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("settings");
    }
}
