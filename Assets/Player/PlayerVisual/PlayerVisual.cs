using System;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private const string isRunning = "isRunning";
    private const string isJumping = "isJumping";
    
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        AdjustPlayerFacingDirection();
        _animator.SetBool(isRunning, Player.Instance.isRunning);
        _animator.SetBool(isJumping, Player.Instance.isJumping);
    }

    private void AdjustPlayerFacingDirection()
    {
        var direction = GameInput.Instance.GetMovementVector();
        if (direction == Vector2.zero)
            return;
        _spriteRenderer.flipX = direction.x <= 0;
    }
}