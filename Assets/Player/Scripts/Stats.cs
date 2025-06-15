
using System;
using TMPro;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private LevelManager levelManager;
    private static string initialText = "Deliveries remaining: ";
    private static string finalText = "Well done!";
    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
    }

    private void FixedUpdate()
    {
        if (levelManager.Instance.placeCounter < levelManager.Instance.length)
            text.text = initialText + (levelManager.Instance.length - levelManager.Instance.placeCounter);
        else
            text.text = finalText;
    }
}
