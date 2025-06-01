using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class Background_Paralax : MonoBehaviour
{
    public GameObject Instance{get; private set;}
    public Rigidbody2D rb{get; private set;}

    private Stopwatch wait = new Stopwatch();
    private void Awake()
    {
        Instance = this.gameObject;
        rb = gameObject.GetComponent<Rigidbody2D>();
        wait.Start();
    }

    private void FixedUpdate()
    {
        if (wait.ElapsedMilliseconds <= 1200)
        {
            rb.linearVelocityX = 0.1f;
        }
        else if (wait.ElapsedMilliseconds <= 2400)
        {
            rb.linearVelocityX = -0.1f;
        }
        else
        {
            wait.Restart();
        }
    }
}