using System;
using UnityEngine;

public class BoxCheckpoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    [SerializeField] private Transform holdPoint;
    [SerializeField] private string targetTag = "Teleportable";

    private bool filled;
    public static event Action<BoxCheckpoint> OnCheckpointFilling;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag) && !filled)
        {
            var rb = other.GetComponent<Rigidbody2D>();
            var cp = other.GetComponent<BoxCollider2D>();
            if (rb != null)
            {
                cp.enabled = false;
            }
            filled = true;
            OnCheckpointFilling?.Invoke(this);
            PlayerBoxHolder.Instance.ActiveBoxes.Remove(other.gameObject);
            PlayerBoxHolder.Instance.AllBoxes.Remove(other.gameObject);
            other.tag = "decoration";
        }
    }
}
