using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace menu
{
    public class LevelMenu : MonoBehaviour
    {
        public static int nextLevel = 2;

        private void Awake()
        {
            Cursor.visible = true;
        }

        public void PlayPressed()
        {
            SceneManager.LoadScene(nextLevel);
        }

        public void ExitPressed()
        {
            SceneManager.LoadScene("mainMenu");
        }
    }
}