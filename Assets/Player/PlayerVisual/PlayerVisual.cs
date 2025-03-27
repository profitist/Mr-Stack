using System;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        AdjustPlayerFacingDirection();
    }

    private void AdjustPlayerFacingDirection()
    {
        var direction = GameInput.Instance.GetMovementVector();
        if (direction == Vector2.zero)
            return;
        _spriteRenderer.flipX = direction.x <= 0;
    }
}