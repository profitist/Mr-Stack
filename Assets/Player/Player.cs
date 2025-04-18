using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class Player : MonoBehaviour
{
    [SerializeField] private float movingSpeed = 5f;
    [SerializeField] private float jumpForce = 300f;
    
    public bool IsJumping { get; private set; }
    public bool IsRunning { get; private set; }
    
    public bool AgainstWall { get; private set; }

    public FacingDirection facingDirection { get; private set; }
    public static Player Instance { get; private set; }
    
    public GameObject NearestBox { get;  set; }
    
    public Rigidbody2D rb{ get; private set; }

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        facingDirection = FacingDirection.Right;
    }
    
    private void Update()
    {
        IsRunning = Math.Abs(rb.linearVelocity.x) > 1e-3 && !IsJumping && !AgainstWall;
        if (Math.Abs(rb.linearVelocity.x) > 1e-3)
        {
            facingDirection = (rb.linearVelocity.x > 1e-3) ? FacingDirection.Right: FacingDirection.Left;
        }
        if (GameInput.Instance.Jumping && !IsJumping && Math.Abs(rb.linearVelocity.y) <= 1e-2)
        {
            rb.AddForce(Vector2.up * (Time.fixedDeltaTime * jumpForce) , ForceMode2D.Impulse);
            IsJumping = true;
        }
        var movementVector = GameInput.Instance.GetMovementVector();
        movementVector = movementVector.normalized;
        rb.linearVelocity = new Vector2(movementVector.x * movingSpeed, rb.linearVelocity.y);
    }
    


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsGroundedCollision(collision))
        {
            IsJumping = false;
        }
        
    }
    
    private bool IsGroundedCollision(Collision2D collision)
    {
        AgainstWall = false;
        if (collision.gameObject.CompareTag("Wall"))
            AgainstWall = true;
        return collision.contacts.Any(contact => contact.normal.y >= 0.5f);
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (IsGroundedCollision(collision))
        {
            IsJumping = true;
        }
    }
}
