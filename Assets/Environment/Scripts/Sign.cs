using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Sign : MonoBehaviour
{
    public CapsuleCollider2D cl { get; private set; }
    public Canvas canvas { get; private set; }
    public SpriteRenderer textBG { get; private set; }
    public bool playerNearby { get; private set; }

    private void Awake()
    {
        cl = GetComponent<CapsuleCollider2D>();
        canvas = GetComponentInChildren<Canvas>();
        textBG = GetComponentsInChildren<SpriteRenderer>().FirstOrDefault(x => x.name == "text bg");
        canvas.enabled = false;
        textBG.enabled = false;
    }

    private void FixedUpdate()
    {
        canvas.enabled = playerNearby;
        textBG.enabled = playerNearby;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            playerNearby = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            playerNearby = false;
    }
}