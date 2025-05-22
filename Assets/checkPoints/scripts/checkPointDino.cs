using UnityEngine;

public class checkpoint : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private void Awake()
    
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    
    private void FlipSprite()
    {
        _spriteRenderer.flipX = !_spriteRenderer.flipX;
    }

    // Update is called once per frame
}
