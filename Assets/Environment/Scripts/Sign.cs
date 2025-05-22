using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Sign : MonoBehaviour
{
    public CapsuleCollider2D cl { get; private set; }
    public Canvas canvas { get; private set; }
    public SpriteRenderer textBG { get; private set; }
    public bool playerNerby { get; private set; }

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
        canvas.enabled = playerNerby;
        textBG.enabled = playerNerby;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            playerNerby = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            playerNerby = false;
    }
}