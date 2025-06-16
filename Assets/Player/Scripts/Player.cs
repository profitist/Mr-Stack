using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using checkPoints.scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class Player : MonoBehaviour
{
    [SerializeField] private float movingSpeed = 5f;
    [SerializeField] private float jumpForce = 300f;
    [SerializeField] private AudioSource runningAudio;
    public bool IsJumping { get; private set; }
    public bool IsRunning { get; private set; }
    
    public bool AgainstWall { get; private set; }

    public FacingDirection facingDirection { get; private set; }
    public static Player Instance { get; private set; }
    
    public Rigidbody2D rb { get; private set; }

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        facingDirection = FacingDirection.Right;
    }
    
    private void Update()
    {
        IsRunning = (!IsJumping && !AgainstWall && GameInput.Instance.GetMovementVector() != Vector2.zero) || FinalCheckpoint.Final;
        ManageAudio();
        if (GameInput.Instance.GetMovementVector() != Vector2.zero)
        {
            facingDirection = GameInput.Instance.GetMovementVector().x > 0 ? FacingDirection.Right: FacingDirection.Left;
        }
        if (GameInput.Instance.Jumping && !IsJumping && Math.Abs(rb.linearVelocity.y) <= 0.1f 
            && !PlayerBoxHolder.Instance.HoldingHeavyBox)
        {
            rb.AddForce(Vector2.up * (Time.fixedDeltaTime * jumpForce) , ForceMode2D.Impulse);
            IsJumping = true;
        }
        var movementVector = GameInput.Instance.GetMovementVector();
        movementVector = movementVector.normalized;
        rb.linearVelocity = new Vector2(movementVector.x * movingSpeed, rb.linearVelocity.y);
    }

    private void ManageAudio()
    {
        if (IsRunning)
        {
            if (!runningAudio.isPlaying)
            {
                Debug.Log("Starting audio");
                runningAudio.Play();
            }
        }
        else
        {
            if (runningAudio.isPlaying)
                runningAudio.Stop();
        }
    }
    
    private bool IsGroundedCollision(Collision2D collision)
    {
        return collision.contacts.Any(contact => Mathf.Abs(contact.normal.y - 1) <= 1e-3);
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Soft") || collision.gameObject.CompareTag("Box") ) && AgainstWall)
            AgainstWall = false;
        if (IsGroundedCollision(collision))
            IsJumping = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.contacts.Any(contact => Mathf.Abs(contact.normal.y) <= 1e-3))
            AgainstWall = true;
        if (IsGroundedCollision(collision))
            IsJumping = false;
    }
}
