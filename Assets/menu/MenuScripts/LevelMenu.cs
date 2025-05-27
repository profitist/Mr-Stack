using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace menu
{
    public class LevelMenu : MonoBehaviour
    {
        public static int nextLevel = 3;
        
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