using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerBoxHolder : MonoBehaviour
{
    private const int maxBoxCount = 3;
    private Stack<GameObject> boxes;
    private Stopwatch wait = new Stopwatch();
    public HashSet<GameObject> ActiveBoxes { get; private set; }
    public Transform holdPoint;
    public static PlayerBoxHolder Instance { get; private set; }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        boxes = new Stack<GameObject>();
        holdPoint = GetComponent<Transform>();
        Debug.Log($"{holdPoint.position.y}");
        ActiveBoxes = new HashSet<GameObject>();
        Instance = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (wait.IsRunning && wait.ElapsedMilliseconds >= 500)
            wait = new Stopwatch();
        if (GameInput.Instance.GrabingBox && maxBoxCount > boxes.Count && Player.Instance.NearestBox && !wait.IsRunning)
        {
            PickUpBox(Player.Instance.NearestBox);
            Debug.Log(boxes.Count);
        }

        if (GameInput.Instance.PuttingBox && boxes.Count > 0 && !wait.IsRunning)
            RemoveBox(boxes.Pop());

    }
    
    private void PickUpBox(GameObject box)
    {
        wait.Start();
        var rb = box.GetComponent<Rigidbody2D>();
        if (rb) rb.simulated = false;
        box.transform.SetParent(holdPoint);
        box.transform.localPosition = new Vector3(0, boxes.Count * 1, 0);
        var capsuleCollider = Player.Instance.GetComponent<CapsuleCollider2D>();
        capsuleCollider.offset += new Vector2(0, 0.5f);
        capsuleCollider.size = new Vector2(capsuleCollider.size.x, capsuleCollider.size.y + 1);
        ActiveBoxes.Add(box);
        boxes.Push(box);
        
    }

    private void RemoveBox(GameObject box)
    {
        wait.Start();
        var capsuleCollider = Player.Instance.GetComponent<CapsuleCollider2D>();
        capsuleCollider.offset -= new Vector2(0, 0.5f);
        capsuleCollider.size = new Vector2(capsuleCollider.size.x, capsuleCollider.size.y - 1);
        var rb = box.GetComponent<Rigidbody2D>();
        if (rb) rb.simulated = true;
        box.transform.parent = null;
        var vel = 5 * (Player.Instance.facingDirection == FacingDirection.Right ? 1 : -1)  
                  + Player.Instance.rb.linearVelocity.x;
        rb.linearVelocity = new Vector2(vel, 6);
        ActiveBoxes.Remove(box);
    }
}
