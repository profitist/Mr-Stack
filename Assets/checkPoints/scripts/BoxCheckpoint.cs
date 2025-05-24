using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoxCheckpoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    [SerializeField] private Transform holdPoint;
    [SerializeField] private string targetTag = "Box";
    [SerializeField] public BoxTypes boxType;
    
    private bool completed;
    public static event Action<BoxCheckpoint> OnCheckpointFilling;
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(targetTag) && !completed && other.gameObject.GetComponent<BoxUpdating>().IsGrounded 
            && other.gameObject.GetComponent<BoxUpdating>().boxType == boxType)
        {
            Debug.Log($"{DateTime.Now.Millisecond} -- Checkpoint finished");
            var rb = other.GetComponent<Rigidbody2D>();
            var cp = other.GetComponent<BoxCollider2D>();
            var boxObjecct = other.gameObject.GetComponent<BoxUpdating>();
            boxObjecct.isFinished = true;
            if (rb != null)
            {
                cp.enabled = false;
                rb.simulated = false;
            }
            completed = true;
            OnCheckpointFilling?.Invoke(this);
            PlayerBoxHolder.Instance.ActiveBoxes.Remove(other.gameObject);
            PlayerBoxHolder.Instance.AllBoxes.Remove(other.gameObject);
            other.tag = "decoration";
        }
    }
}
