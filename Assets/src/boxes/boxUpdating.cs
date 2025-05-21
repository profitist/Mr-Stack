using System;
using System.Linq;
using UnityEngine;

public class boxUpdating : MonoBehaviour
{
    public readonly int boxId;
    public BoxTypes boxType;
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
        if (IsGrounded)
            collider.isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
            IsGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
            IsGrounded = false;
    }
}
