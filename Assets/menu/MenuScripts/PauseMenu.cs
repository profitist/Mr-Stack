using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace menu
{
    public class PauseMenu : MonoBehaviour
    {
        private Canvas pauseMenu;
        private void Awake()
        {
            pauseMenu = GetComponent<Canvas>();
            pauseMenu.enabled = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!pauseMenu.enabled)
                {
                    GameInput.Instance.playerInputActions.Disable();
                    Time.timeScale = 0;
                }
                else
                {
                    GameInput.Instance.playerInputActions.Enable();
                    Time.timeScale = 1;
                }

                pauseMenu.enabled = !pauseMenu.enabled;
            }   
        }

        public void ExitGame()
        {
            pauseMenu.enabled = false;
            Time.timeScale = 1;
            SceneManager.LoadScene("mainMenu");
        }

        public void RestartGame()
        {
            pauseMenu.enabled = false;
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ContinueGame()
        {
            GameInput.Instance.playerInputActions.Enable();
            pauseMenu.enabled = false;
            Time.timeScale = 1;
        }

        public void OpenSettings()
        {
            GameInput.Instance.playerInputActions.Enable();
            pauseMenu.enabled = false;
            Time.timeScale = 1;
            SettingMenu.levelId = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene("settings");
        }
    }
}