using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float movingSpeed = 10f;
    [SerializeField] private float jumpForce = 30f;
    
    private bool isGrounded;
    
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    [Obsolete("Obsolete")]
    private void FixedUpdate()
    {
        if (isGrounded && GameInput.Instance.IsJump())
        {
            Debug.Log("Jump");
            
            isGrounded = false;
            return;
        }
        rb.velocity = new Vector2(rb.linearVelocity.x, jumpForce); 
        var movementVector = GameInput.Instance.GetMovementVector();
        Debug.Log("Move");
        movementVector = movementVector.normalized;
        rb.MovePosition(rb.position + movementVector * (movingSpeed * Time.fixedDeltaTime));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Collision");
            isGrounded = true;
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("No Collision");
            isGrounded = false;
        }
    }
}
