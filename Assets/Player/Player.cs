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
    
    public bool isGrounded { get; private set; }
    public bool isJumping { get; private set; }
    public bool isRunning {get; private set;}
    
    private Rigidbody2D rb;
    public static Player Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }
    
    [Obsolete("Obsolete")]
    private void FixedUpdate()
    {
        isRunning = Math.Abs(rb.velocity.x) > 1e-3 && !isJumping;
        if (GameInput.Instance.IsJump() && !isJumping && Math.Abs(rb.velocity.y) <= 1e-2)
        {
            rb.AddForce(Vector2.up * (Time.fixedDeltaTime * jumpForce) , ForceMode2D.Impulse);
            isJumping = true;
        }
        var movementVector = GameInput.Instance.GetMovementVector();
        movementVector = movementVector.normalized;
        rb.velocity = new Vector2(movementVector.x * movingSpeed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsGroundedCollision(collision))
        {
            Debug.Log("Collision");
            isJumping = false;
        }
        
    }
    
    
    private bool IsGroundedCollision(Collision2D collision)
    {
        // Проходим по всем точкам контакта
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // Если нормаль направлена вверх (с допустимым отклонением)
            if (contact.normal.y >= 1f) // 0.7f ≈ 45 градусов
            {
                return true;
            }
        }
        return false;
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (IsGroundedCollision(collision))
        {
            Debug.Log("No Collision");
            isJumping = true;
        }
    }
}
