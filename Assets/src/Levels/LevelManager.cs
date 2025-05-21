using System;
using menu;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private BoxCheckpoint[] items;

    private int placeCounter = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        
        items = FindObjectsByType<BoxCheckpoint>(FindObjectsSortMode.None);
        Debug.Log(items.Length);
    }
    // Update is called once per frame
    private void OnEnable()
    {
        BoxCheckpoint.OnCheckpointFilling += ActionOnPlacing;
    }

    private void OnDisable()
    {
        BoxCheckpoint.OnCheckpointFilling -= ActionOnPlacing;
    }


    private void ActionOnPlacing(BoxCheckpoint checkpoint)
    {
        placeCounter++;
        if (placeCounter >= items.Length)
        {
            GameInput.Instance.playerInputActions.Disable();
            LevelMenu.nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                SceneManager.LoadScene("MainMenu");
            }
            else
            {
                SceneManager.LoadScene("LevelMenu");
            }
        }
    }
    
}
