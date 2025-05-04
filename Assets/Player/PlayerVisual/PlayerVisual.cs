using System;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField]
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
        _animator.SetBool(isRunning, Player.Instance.IsRunning);
        _animator.SetBool(isJumping, Player.Instance.IsJumping);
    }

    private void AdjustPlayerFacingDirection()
    {
            _spriteRenderer.flipX = Player.Instance.facingDirection == FacingDirection.Left;
    }

    private void OnEnable()
    {
        PlayerBoxHolder.OnPickingBox += OnPickingBox;
    }

    private void OnPickingBox(PlayerBoxHolder boxHolder)
    {

    }
}