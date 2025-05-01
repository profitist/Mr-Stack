using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class PlayerBoxHolder : MonoBehaviour
{
    private const int maxBoxCount = 3;
    private Stack<GameObject> boxes;
    private GameObject NearestBox;
    private Stopwatch wait = new Stopwatch();
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
        if (GameInput.Instance.GrabingBox && maxBoxCount > boxes.Count && !wait.IsRunning)
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
        var capsuleCollider = Player.Instance.GetComponent<CapsuleCollider2D>();
        capsuleCollider.offset += new Vector2(0, 0.5f);
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
    
    public void FindNearestBox()
    {
        var boxCollider = AllBoxes
            .FirstOrDefault(x => !ActiveBoxes.Contains(x) 
                                 && Vector2.Distance(x.transform.position, Player.Instance.transform.position) < 2
                                 && System.Math.Abs(x.transform.position.y - Player.Instance.transform.position.y) < 1);
        if (boxCollider is not null)
        {
            NearestBox = boxCollider;
            Debug.Log("FOUND!");
        }
        else
            NearestBox = null;
    }
}
