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
            Cursor.visible = true;
            GameInput.Instance.playerInputActions.Disable();
            deathMenu.enabled = true;
            Time.timeScale = 0;
        }
    }
    
    public void ExitGame()
    {
        deathMenu.enabled = false;
        Time.timeScale = 1;
        GameInput.Instance.playerInputActions.Enable();
        GameInput.IsDead = false;
        SceneManager.LoadScene("mainMenu");
    }

    public void RestartGame()
    {
        Cursor.visible = false;
        deathMenu.enabled = false;
        Time.timeScale = 1;
        GameInput.Instance.playerInputActions.Enable();
        GameInput.IsDead = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameInput.Instance.playerInputActions.Enable();
    }
    
    public void OpenSettings()
    {
        GameInput.Instance.playerInputActions.Enable();
        deathMenu.enabled = false;
        Time.timeScale = 1;
        SettingMenu.levelId = SceneManager.GetActiveScene().buildIndex;
        GameInput.IsDead = false;
        SceneManager.LoadScene("settings");
    }
}
