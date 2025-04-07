using System;
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
    public static Player Instance { get; private set; }
    
    public static Box NearestBox { get; private set; }
    
    private Rigidbody2D rb;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }
    
    [Obsolete("Obsolete")]
    private void FixedUpdate()
    {
        FindNearestBox();
        IsRunning = Math.Abs(rb.velocity.x) > 1e-3 && !IsJumping;
        if (GameInput.Instance.IsJump() && !IsJumping && Math.Abs(rb.velocity.y) <= 1e-2)
        {
            rb.AddForce(Vector2.up * (Time.fixedDeltaTime * jumpForce) , ForceMode2D.Impulse);
            IsJumping = true;
        }
        var movementVector = GameInput.Instance.GetMovementVector();
        movementVector = movementVector.normalized;
        rb.velocity = new Vector2(movementVector.x * movingSpeed, rb.velocity.y);
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
        return collision.contacts.Any(contact => contact.normal.y >= 1f);
    }

    private void FindNearestBox()
    {
        var boxCollider = Physics2D.OverlapCircleAll(transform.position, 1)
            .Where(c => c.gameObject.CompareTag("Box"))
            .OrderBy(x =>  Vector2.Distance(x.transform.position, transform.position))
            .FirstOrDefault();
        if (boxCollider is not null)
            NearestBox = boxCollider.gameObject.GetComponent<Box>();
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (IsGroundedCollision(collision))
        {
            IsJumping = true;
        }
    }
}
