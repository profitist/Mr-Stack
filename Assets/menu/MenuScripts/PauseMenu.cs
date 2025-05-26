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
                    GameInput.Instance.playerInputActions.Disable();
                else
                    GameInput.Instance.playerInputActions.Enable();
                pauseMenu.enabled = !pauseMenu.enabled;
            }
        }

        public void ExitGame()
        {
            SceneManager.LoadScene("mainMenu");
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ContinueGame()
        {
            pauseMenu.enabled = false;
            GameInput.Instance.playerInputActions.Enable();
        }

        public void OpenSettings()
        {
            SceneManager.LoadScene("settings");
        }
    }
}