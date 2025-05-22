using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TransparentWalls : MonoBehaviour
{
    public Tilemap Instance { get; private set; }
    
    public bool makeTransparent;
    private void Awake()
    {
        Instance = gameObject.GetComponent<Tilemap>();
    }

    private void FixedUpdate()
    {
        var color = Instance.color;
        if (makeTransparent)
        {
            color.a -= (color.a <= 0.2f) ? 0 : 0.1f;
            Instance.color = color;
            return;
        }
        color.a += (color.a >= 1f) ? 0 : 0.1f;
        Instance.color = color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            makeTransparent = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            makeTransparent = false;
    }
}