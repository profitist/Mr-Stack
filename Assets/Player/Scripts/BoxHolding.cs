using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerBoxHolder : MonoBehaviour
{
    private static readonly int IsSleeping = Animator.StringToHash("isSleeping");
    [SerializeField] private AudioSource pickingSound;
    [SerializeField] private AudioSource removeSound;
    
    public Stack<GameObject> boxes { get; private set;}
    private GameObject nearestBox;
    public int heavyBoxesCount;
    private bool isEggOnStack;
    public bool HoldingHeavyBox {get; set;}
    private Stopwatch wait = new();
    public static event Action<PlayerBoxHolder> OnPickingBox;
    public HashSet<GameObject> ActiveBoxes { get; private set; }
    public List<GameObject> AllBoxes { get; private set;}
    public Transform holdPoint;
    public static PlayerBoxHolder Instance { get; private set; }
    private GameObject[] dots;
    [SerializeField] private GameObject dotPref;
    private int dotsCount = 15;
    private float dotSpacing = 0.07f;

    
    
    void Awake()
    {
        dots = new GameObject[dotsCount];
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
        if (wait.IsRunning && wait.ElapsedMilliseconds >= 700)
            wait = new Stopwatch();
        if (GameInput.Instance.GrabbingBox && !wait.IsRunning && Player.Instance.rb.linearVelocityY < 1e-2)
        {
            PickUpBox();
            Debug.Log(boxes.Count);
        }
        if (GameInput.Instance.PuttingBox && boxes.Count > 0 && !wait.IsRunning)
            RemoveBox(boxes.Pop());
        if (ActiveBoxes.Count > 0)
        {
            for (var i = 0; i < dotsCount; i++)
            {
                float time = i * dotSpacing;
                var velocityX = 5 * (Player.Instance.facingDirection == FacingDirection.Right ? 1 : -1)
                                + Player.Instance.rb.linearVelocity.x;
                var velocityY = 6 + (Player.Instance.rb.linearVelocityY > 1e-2 ? Player.Instance.rb.linearVelocityY : 0);
                Vector2 dotPos = new Vector2(
                    holdPoint.transform.position.x + velocityX * time,
                    holdPoint.transform.position.y + ActiveBoxes.Count - 1 + velocityY * time +
                    Physics2D.gravity.y * time * time);
                
                if (dots[i] == null)
                    dots[i] = Instantiate(dotPref, dotPos, Quaternion.identity);
                else
                    dots[i].transform.position = dotPos;
            }
        }
        else
            foreach (var dot in dots)
            {
                Destroy(dot);
            }
    }
    
    private void PickUpBox()
    {
        FindNearestBox();
        var box = nearestBox;
        if (box is null)
            return;
        if (isEggOnStack)
            return;
        if (box.GetComponent<BoxUpdating>().boxType == BoxTypes.Egg)
            isEggOnStack = true;
        wait.Start();
        OnPickingBox?.Invoke(Instance);
        var capsuleCollider = Player.Instance.GetComponent<CapsuleCollider2D>();
        var hit = Physics2D.Raycast(
            Player.Instance.transform.position + new Vector3(0,0.2f,0),
            Vector2.up,
            2 + boxes.Count);
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Ground"))
            return;
        StartCoroutine(AnimatePickingBox(box, ActiveBoxes.Count));
        capsuleCollider.offset += new Vector2(0, 0.5f);
        capsuleCollider.size = new Vector2(capsuleCollider.size.x, capsuleCollider.size.y + 1);
        Player.Instance.rb.MovePosition(new Vector3(Player.Instance.transform.position.x, Player.Instance.transform.position.y +0.1f,0));
        if (box.GetComponent<BoxUpdating>().boxType == BoxTypes.Heavy)
        {
            heavyBoxesCount++;
            HoldingHeavyBox = true;
        }
        if (heavyBoxesCount == 0)
            HoldingHeavyBox = false;
        pickingSound.Play();
        ActiveBoxes.Add(box);
        boxes.Push(box);
        if (box.GetComponent<BoxUpdating>().boxType == BoxTypes.Standart)
        {
            Debug.Log("SLEEP");
            var boxAnimator = box.GetComponentInChildren<Animator>();
            boxAnimator.SetBool("isSleeping", true);
        }
    }

    private void RemoveBox(GameObject box)
    {
        wait.Start();
        var capsuleCollider = Player.Instance.GetComponent<CapsuleCollider2D>();
        capsuleCollider.offset -= new Vector2(0, 0.5f);
        capsuleCollider.size = new Vector2(capsuleCollider.size.x, capsuleCollider.size.y - 1);
        Player.Instance.rb.MovePosition(new Vector3(Player.Instance.transform.position.x, Player.Instance.transform.position.y +0.1f,0));
        var rb = box.GetComponent<Rigidbody2D>();
        if (rb) rb.simulated = true;
        box.transform.parent = null;
        if (box.GetComponent<BoxUpdating>().boxType == BoxTypes.Heavy)
        {
            heavyBoxesCount--;
            if (heavyBoxesCount == 0)
                HoldingHeavyBox = false;
        }
        if (box.GetComponent<BoxUpdating>().boxType == BoxTypes.Egg)
            isEggOnStack = false;
        var velocityX = 5 * (Player.Instance.facingDirection == FacingDirection.Right ? 1 : -1)  
                  + Player.Instance.rb.linearVelocity.x;
        var velocityY = 6 + (Player.Instance.rb.linearVelocityY > 1e-2 ? Player.Instance.rb.linearVelocityY : 0);
        rb.linearVelocity = new Vector2(velocityX, velocityY);
        removeSound.Play();
        ActiveBoxes.Remove(box);
        if (box.GetComponent<BoxUpdating>().boxType == BoxTypes.Standart)
        {
            var boxAnimator = box.GetComponentInChildren<Animator>();
            boxAnimator.SetBool(IsSleeping, false);
        }
    }

    private IEnumerator AnimatePickingBox(GameObject box, float stackHeight)
    {
        var duration = 0.5f;
        var time = 0f;
        var start = box.transform.position;
        var end = holdPoint.position + new Vector3(0, stackHeight * 1, 0);
        var constVel = 10f;
        var rb = box.GetComponent<Rigidbody2D>();
        var cl = box.GetComponent<BoxCollider2D>();
        box.GetComponent<BoxUpdating>().IsGrounded = false;
        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        cl.enabled = false;
        var velX = (end.x - start.x) / duration;
        if (rb) rb.bodyType = RigidbodyType2D.Kinematic;
        while (time < duration)
        {
            var t = time / duration;
            var verticalSpeed = Mathf.Cos(t * Mathf.PI) * constVel + (end.y - start.y) / duration;
            rb.linearVelocity =  new Vector2(
                velX + Player.Instance.rb.linearVelocityX * (Player.Instance.AgainstWall ? 0 : 1),
                Player.Instance.rb.linearVelocityY * (Player.Instance.AgainstWall ? 0 : 1) + verticalSpeed);
            time += Time.deltaTime;
            yield return null;
        }
        cl.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.simulated = false;
        box.transform.SetParent(holdPoint);
        box.transform.localPosition = new Vector3(0, stackHeight, 0);
    }
    
    private void FindNearestBox()
    {
        GameObject currentBox = null;
        var collision = Player.Instance.GetComponent<CapsuleCollider2D>();
        foreach (var box in AllBoxes)
        {
            if (!ActiveBoxes.Contains(box) && collision.IsTouching(box.GetComponent<BoxCollider2D>()))
                currentBox = box;
        }
        nearestBox = currentBox;
    }
}
