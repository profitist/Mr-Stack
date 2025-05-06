using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerBoxHolder : MonoBehaviour
{
    private Stack<GameObject> boxes;
    private GameObject NearestBox;
    private Stopwatch wait = new ();
    public static event Action<PlayerBoxHolder> OnPickingBox;
    public HashSet<GameObject> ActiveBoxes { get; private set; }
    public List<GameObject> AllBoxes { get; private set; }
    public Transform holdPoint;
    public static PlayerBoxHolder Instance { get; private set; }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        AllBoxes = new List<GameObject>();
        foreach (var collider in FindObjectsByType<GameObject>(default))
        {
            if (collider.CompareTag("Box"))
                AllBoxes.Add(collider.gameObject);
        }
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
        if (GameInput.Instance.GrabingBox && !wait.IsRunning && Player.Instance.rb.linearVelocityY < 0.1f)
        {
            PickUpBox();
            Debug.Log(boxes.Count);
        }
        if (GameInput.Instance.PuttingBox && boxes.Count > 0 && !wait.IsRunning)
            RemoveBox(boxes.Pop());
    }
    
    private void PickUpBox()
    {
        FindNearestBox();
        var box = NearestBox;
        if (box is null)
            return;
        wait.Start();
        OnPickingBox?.Invoke(Instance);
        var capsuleCollider = Player.Instance.GetComponent<CapsuleCollider2D>();
        var hit = Physics2D.Raycast(Player.Instance.transform.position + new Vector3(0,0.2f,0), Vector2.up, 2 + boxes.Count);
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Ground"))
        {
            return;
        }
        capsuleCollider.offset += new Vector2(0, 0.51f);
        capsuleCollider.size = new Vector2(capsuleCollider.size.x, capsuleCollider.size.y + 1);
        var rb = box.GetComponent<Rigidbody2D>();
        if (rb) rb.simulated = false;
        box.transform.SetParent(holdPoint);
        box.transform.localPosition = new Vector3(0, boxes.Count * 1, 0);
        ActiveBoxes.Add(box);
        boxes.Push(box);
        
    }

    private void RemoveBox(GameObject box)
    {
        wait.Start();
        var capsuleCollider = Player.Instance.GetComponent<CapsuleCollider2D>();
        capsuleCollider.offset -= new Vector2(0, 0.51f);
        capsuleCollider.size = new Vector2(capsuleCollider.size.x, capsuleCollider.size.y - 1);
        var rb = box.GetComponent<Rigidbody2D>();
        if (rb) rb.simulated = true;
        box.transform.parent = null;
        var velocityX = 5 * (Player.Instance.facingDirection == FacingDirection.Right ? 1 : -1)  
                  + Player.Instance.rb.linearVelocity.x;
        var velocityY = 6 + (Player.Instance.rb.linearVelocityY > 1e-2 ? Player.Instance.rb.linearVelocityY : 0);
        rb.linearVelocity = new Vector2(velocityX, velocityY);
        ActiveBoxes.Remove(box);
    }
    
    private void FindNearestBox()
    {
        GameObject currentBox = default;
        var collision = Player.Instance.GetComponent<CapsuleCollider2D>();
        foreach (var box in AllBoxes)
        {
            if (!ActiveBoxes.Contains(box) && collision.IsTouching(box.GetComponent<BoxCollider2D>()))
                currentBox = box;
        }
        if (currentBox is not null)
        {
            NearestBox = currentBox;
            Debug.Log("FOUND!");
        }
        else
            NearestBox = null;
    }
}
