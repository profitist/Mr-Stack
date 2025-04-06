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
        if (direction.x == 0)
            return;
        _spriteRenderer.flipX = direction.x <= 0;
    }
}