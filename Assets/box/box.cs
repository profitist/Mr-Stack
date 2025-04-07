using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Box : MonoBehaviour
{
    public readonly int boxId;
    public readonly BoxTypes boxType;

    public Box(int boxId, BoxTypes boxType)
    {
        this.boxId = boxId;
        this.boxType = boxType;
    }
}
