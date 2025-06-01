using System;
using UnityEngine;

public class thoughtRouter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is create

    private void OnEnable()
    {
        BoxCheckpoint.OnCheckpointFilling += PutTick;
    }

    private void OnDisable()
    {
        BoxCheckpoint.OnCheckpointFilling -= PutTick;
    }

    private void PutTick(BoxCheckpoint checkpoint)
    {
        foreach (var sprite in checkpoint.gameObject.GetComponentsInChildren<SpriteRenderer>(true))
        {
            if (sprite.CompareTag("boximage"))
            {
                sprite.sortingOrder = -2;
            }
            else if (sprite.CompareTag("tick"))
            {
                sprite.sortingOrder = 1;
            }
        }
    }
        
}
