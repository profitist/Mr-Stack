using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBoxHolder : MonoBehaviour
{
    private const int maxBoxCount = 3;
    private Stack<GameObject> boxes;
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
    void Update()
    {
        Debug.Log(boxes.Count);
        if (GameInput.Instance.GrabingBox && maxBoxCount > boxes.Count && Player.Instance.NearestBox)
            PickUpBox(Player.Instance.NearestBox);
        if (GameInput.Instance.PuttingBox && boxes.Count > 0)
           RemoveBox(boxes.Pop());
        
    }
    
    private void PickUpBox(GameObject box)
    {
        var rb = box.GetComponent<Rigidbody2D>();
        if (rb) rb.simulated = false;
        box.transform.SetParent(holdPoint);
        box.transform.localPosition = new Vector3(0, boxes.Count * 1, 0);
        ActiveBoxes.Add(box);
        boxes.Push(box);
    }

    private void RemoveBox(GameObject box)
    {
        var rb = box.GetComponent<Rigidbody2D>();
        if (rb) rb.simulated = true;
        box.transform.parent = null;
        ActiveBoxes.Remove(box);
    }
}
