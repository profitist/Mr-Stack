using System;
using NUnit.Framework;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Box : MonoBehaviour
{
    public readonly int boxId;
    public readonly BoxTypes boxType;
    private bool IsGrounded; 
    public Rigidbody2D rb { get; private set; }
    public BoxCollider2D collider { get; private set; }
    public bool isFinished;

    public Box(int boxId, BoxTypes boxType)
    {
        this.boxId = boxId;
        this.boxType = boxType;
    }

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
    }

    public void FixedUpdate()
    {
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            rb.bodyType = RigidbodyType2D.Static;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            rb.bodyType = RigidbodyType2D.Dynamic;
    }
}
