using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace menu
{
    public class LevelMenu : MonoBehaviour
    {
        public static int nextLevel = 2;
        
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