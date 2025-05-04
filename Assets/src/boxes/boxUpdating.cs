using System;
using System.Linq;
using UnityEngine;

public class boxUpdating : MonoBehaviour
{
    public readonly int boxId;
    public readonly BoxTypes boxType;
    public bool IsGrounded { get; private set; } 
    public Rigidbody2D rb { get; private set; }
    public BoxCollider2D collider { get; private set; }
    public bool isFinished;
    

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
    }

    public void FixedUpdate()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts.Any(n => n.normal.y < 0.3f ))
            IsGrounded = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            IsGrounded = false;
    }
    
}
