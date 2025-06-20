﻿using System;
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
                    Cursor.visible = true;
                    pauseMenu.enabled = true;
                }
                else
                {
                    GameInput.Instance.playerInputActions.Enable();
                    Time.timeScale = 1;
                    Cursor.visible = false;
                    pauseMenu.enabled = false;
                }
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
            Cursor.visible = false;
            pauseMenu.enabled = false;
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ContinueGame()
        {
            Cursor.visible = false;
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