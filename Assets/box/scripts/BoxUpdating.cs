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
        if (!IsGrounded && !isFinished && (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Soft")) && collision.contacts.Any(contact => Mathf.Abs(contact.normal.y - 1) < 1e-3))
        {
            IsGrounded = true;
            rb.mass = float.MaxValue;
            Debug.Log("WTF");
            if (collision.gameObject.CompareTag("Ground"))
            {
                Invoke(nameof(DelayedBreakCheck), 0.5f);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Soft"))
            IsGrounded = false;
    }

    private void BreakEgg()
    {
        OnEggCrushing?.Invoke(this);
    }
    
    private void DelayedBreakCheck()
    {
        if (!isFinished)
        {
            if (boxType == BoxTypes.Egg)
            {
                animator.SetBool("isBreaking", true);
                Invoke(nameof(BreakEgg), 1f);
            }
        }
    }
}
