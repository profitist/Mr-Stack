using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace menu
{
    public class PauseMenu : MonoBehaviour
    {
        Canvas pauseMenu;
        private void Awake()
        {
            pauseMenu = GetComponent<Canvas>();
            pauseMenu.enabled = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameInput.Instance.playerInputActions.Disable();
                pauseMenu.enabled = true;
            }
        }

        public void ExitGame()
        {
            SceneManager.LoadScene(0);
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
    }
}