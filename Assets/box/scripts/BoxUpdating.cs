using System;
using System.Linq;
using UnityEngine;

public class BoxUpdating : MonoBehaviour
{
    private static readonly int IsBreaking = Animator.StringToHash("isBreaking");
    public BoxTypes boxType;
    public bool IsGrounded { get; set; } 
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
        rb.constraints |= RigidbodyConstraints2D.FreezePositionX;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsGrounded && !isFinished && (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Box") || collision.gameObject.CompareTag("Soft")) && collision.contacts.Any(contact => Mathf.Abs(contact.normal.y - 1) < 1e-3))
        {
            rb.constraints |= RigidbodyConstraints2D.FreezePositionX;
            IsGrounded = true;
            if (collision.gameObject.CompareTag("Ground"))
            {
                Invoke(nameof(DelayedBreakCheck), 0.5f);
            }
        }
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!IsGrounded && !isFinished && (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Box") || collision.gameObject.CompareTag("Soft")) && collision.contacts.Any(contact => Mathf.Abs(contact.normal.y - 1) < 1e-3))
        {
            rb.constraints |= RigidbodyConstraints2D.FreezePositionX;
            IsGrounded = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if ((other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Box") ||
                           other.gameObject.CompareTag("Soft")))
        {
            rb.constraints |= RigidbodyConstraints2D.FreezePositionX;
            IsGrounded = false;
        }
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
                animator.SetBool(IsBreaking, true);
                Invoke(nameof(BreakEgg), 1f);
            }
        }
    }

}
