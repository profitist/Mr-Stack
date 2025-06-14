using System;
using UnityEngine;

public class ControlsScript : MonoBehaviour
{
    private Canvas canvas;

    private void Awake()
    {
        canvas = gameObject.GetComponent<Canvas>();
        canvas.enabled = false;
    }

    private void FixedUpdate()
    {
        canvas.enabled = GameInput.Instance.Tab;
    }
}