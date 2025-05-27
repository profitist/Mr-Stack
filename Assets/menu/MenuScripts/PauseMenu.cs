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
            SceneManager.LoadScene("mainMenu");
        }

        public void RestartGame()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ContinueGame()
        {
            pauseMenu.enabled = false;
            Time.timeScale = 1;
            GameInput.Instance.playerInputActions.Enable();
        }

        public void OpenSettings()
        {
            pauseMenu.enabled = false;
            Time.timeScale = 1;
            SceneManager.LoadScene("settings");
        }
    }
}