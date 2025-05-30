using System;
using System.Linq;
using UnityEngine;

public class BoxUpdating : MonoBehaviour
{
    public readonly int boxId;
    public BoxTypes boxType;
    public bool IsGrounded { get; private set; } 
    public Rigidbody2D rb { get; private set; }
    private BoxCollider2D collider { get; set; }
    private Animator animator;
    public bool isFinished;
    
    public static event Action<BoxUpdating> OnEggCrushing;
    

    public void Awake()
    {
        IsGrounded = true;
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        if (boxType == BoxTypes.Egg)
            animator = GetComponent<Animator>();
    }

    public void FixedUpdate()
    {
        if (IsGrounded)
        {
            collider.isTrigger = false;
        }
    }
    
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsGrounded && collision.gameObject.CompareTag("Ground") && !isFinished && collision.contacts.Any(contact => Mathf.Abs(contact.normal.y - 1) < 1e-3))
        {
            IsGrounded = true;
            rb.mass = float.MaxValue;
            Invoke(nameof(DelayedBreakCheck), 0.5f);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            IsGrounded = false;
    }

    private void BreakEgg()
    {
        animator.SetBool("isBreaking", true);
        OnEggCrushing?.Invoke(this);
    }
    
    private void DelayedBreakCheck()
    {
        if (!isFinished)
        {
            IsGrounded = true;
            if (boxType == BoxTypes.Egg)
                BreakEgg();
        }
    }
}
