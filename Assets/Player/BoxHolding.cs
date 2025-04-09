using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBoxHolder : MonoBehaviour
{
    private const int maxBoxCount = 3;
    private List<GameObject> boxes;
    public HashSet<GameObject> ActiveBoxes { get; private set; }
    public Transform holdPoint;
    public static PlayerBoxHolder Instance { get; private set; }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        boxes = new List<GameObject>();
        holdPoint = GetComponent<Transform>();
        Debug.Log($"{holdPoint.position.y}");
        ActiveBoxes = new HashSet<GameObject>();
        Instance = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(boxes.Count);
        if (GameInput.Instance.IsGrabbingBox() && maxBoxCount > boxes.Count && Player.Instance.NearestBox)
        {
            Player.Instance.NearestBox.transform.position = holdPoint.position;
            holdPoint.position = holdPoint.position + Vector3.up;
            ActiveBoxes.Add(Player.Instance.NearestBox);
            boxes.Add(Player.Instance.NearestBox);
        }

        // if (boxes.Count > 0)
        // {
        //     for (var i = 0; i < boxes.Count; i++)
        //     {
        //         var box = boxes[i];
        //         box.transform.position = holdPoint.position - Vector3.up * i;
        //     }
        // }
    }
}
