using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class Player : MonoBehaviour
{
    [SerializeField] private float movingSpeed = 5f;
    [SerializeField] private float jumpForce = 300f;
    
    private bool isGrounded;
    private bool isJumping;
    
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    [Obsolete("Obsolete")]
    private void FixedUpdate()
    {
        if (isGrounded && GameInput.Instance.IsJump() && !isJumping)
        {
            rb.AddForce(Vector2.up * (Time.fixedDeltaTime * jumpForce) , ForceMode2D.Impulse);
            isJumping = true;
        }
        var movementVector = GameInput.Instance.GetMovementVector();
        movementVector = movementVector.normalized;
        rb.velocity = new Vector2(movementVector.x * movingSpeed,rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Collision");
            isGrounded = true;
            isJumping = false;
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
