using Unity.VisualScripting;
using UnityEngine;

public class Background_Paralax : MonoBehaviour
{
    public GameObject Instance{get; private set;}
    public Rigidbody2D rb{get; private set;}
    private void Awake()
    {
        Instance = this.gameObject;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocityX = -Player.Instance.rb.linearVelocityX / 10;
    }
}