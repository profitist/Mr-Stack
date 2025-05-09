using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] 
    private AudioSource successSound;
    
    
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
        successSound.Play();
        placeCounter++;
        if (placeCounter >= items.Length)
            Debug.Log("Все объектны найдены!!!");
    }
    
}
