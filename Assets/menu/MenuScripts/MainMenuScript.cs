using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene("tutorial");
    }
}
