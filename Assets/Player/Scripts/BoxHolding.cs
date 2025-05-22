using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerBoxHolder : MonoBehaviour
{
    private Stack<GameObject> boxes;
    private GameObject nearestBox;
    public int heavyBoxesCount;
    private bool isEggOnStack;
    private Stopwatch wait = new();
    public static event Action<PlayerBoxHolder> OnPickingBox;
    public HashSet<GameObject> ActiveBoxes { get; private set; }
    public List<GameObject> AllBoxes { get; private set;}
    public Transform holdPoint;
    public static PlayerBoxHolder Instance { get; private set; }
    
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
        if (heavyBoxesCount != 0)
            Player.Instance.rb.mass = float.MaxValue;
        else
            Player.Instance.rb.mass = 1;
    }
    
    private void PickUpBox()
    {
        FindNearestBox();
        var box = nearestBox;
        if (box is null)
            return;
        if (isEggOnStack)
            return;
        if (box.GetComponent<boxUpdating>().boxType == BoxTypes.Egg)
            isEggOnStack = true;
        wait.Start();
        OnPickingBox?.Invoke(Instance);
        var capsuleCollider = Player.Instance.GetComponent<CapsuleCollider2D>();
        var hit = Physics2D.Raycast(
            Player.Instance.transform.position + new Vector3(0,0.2f,0),
            Vector2.up,
            2 + boxes.Count);
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Ground"))
        {
            return;
        }
        StartCoroutine(AnimatePickingBox(box, 2, ActiveBoxes.Count));
        capsuleCollider.offset += new Vector2(0, 0.5f);
        capsuleCollider.size = new Vector2(capsuleCollider.size.x, capsuleCollider.size.y + 1);
        if (box.GetComponent<boxUpdating>().boxType == BoxTypes.Heavy)
        {
            heavyBoxesCount++;
        }
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
        if (box.GetComponent<boxUpdating>().boxType == BoxTypes.Heavy)
        {
            heavyBoxesCount--;
        }
        if (box.GetComponent<boxUpdating>().boxType == BoxTypes.Egg)
        {
            isEggOnStack = false;
        }
        var velocityX = 5 * (Player.Instance.facingDirection == FacingDirection.Right ? 1 : -1)  
                  + Player.Instance.rb.linearVelocity.x;
        var velocityY = 6 + (Player.Instance.rb.linearVelocityY > 1e-2 ? Player.Instance.rb.linearVelocityY : 0);
        rb.linearVelocity = new Vector2(velocityX, velocityY);
        ActiveBoxes.Remove(box);
    }

    private IEnumerator AnimatePickingBox(GameObject box, float arcHeight, float stackHeight)
    {
        var duration = 0.5f;
        var time = 0f;
        var start = box.transform.position;
        var end = holdPoint.position + new Vector3(0, stackHeight * 1, 0);
        var constVel = 10f;
        var rb = box.GetComponent<Rigidbody2D>();
        var cl = box.GetComponent<BoxCollider2D>();
        cl.enabled = false;
        var velX = (end.x - start.x) / duration;
        if (rb) rb.bodyType = RigidbodyType2D.Kinematic;
        while (time < duration)
        {
            var t = time / duration;
            var verticalSpeed = Mathf.Cos(t * Mathf.PI) * constVel + (end.y - start.y) / duration;
            rb.linearVelocity =  new Vector2(
                velX + Player.Instance.rb.linearVelocityX,
                Player.Instance.rb.linearVelocityY + verticalSpeed);
            time += Time.deltaTime;
            yield return null;
        }
        rb.bodyType = RigidbodyType2D.Dynamic;
        cl.enabled = true;
        rb.simulated = false;
        box.transform.SetParent(holdPoint);
        box.transform.localPosition = new Vector3(0, stackHeight, 0);
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
            nearestBox = currentBox;
            Debug.Log("FOUND!");
        }
        else
            nearestBox = null;
    }
}
