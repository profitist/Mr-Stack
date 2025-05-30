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
        if (wait.ElapsedMilliseconds <= 1000)
        {
            rb.linearVelocityX = 0.3f;
        }
        else if (wait.ElapsedMilliseconds <= 2000)
        {
            rb.linearVelocityX = -0.3f;
        }
        else
        {
            wait.Restart();
        }
    }
}