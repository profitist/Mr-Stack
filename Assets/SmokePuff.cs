using UnityEngine;

public class SmokePuff : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        PlayerBoxHolder.OnPickingBox += OnBoxPlacing;
        if (PlayerBoxHolder.Instance != null)
            OnBoxPlacing(PlayerBoxHolder.Instance);
    }

    private void OnDisable()
    {
        PlayerBoxHolder.OnPickingBox -= OnBoxPlacing;
    }

    private void OnBoxPlacing(PlayerBoxHolder boxHolder)
    {
        if(animator == null) Debug.LogError("Animator not found!");
        else if(!animator.enabled) Debug.LogWarning("Animator disabled!");
        else
        {
            Debug.Log("Playing puff animation");
            animator.Play("puff"); // или "pdff"
        }
    }
}
