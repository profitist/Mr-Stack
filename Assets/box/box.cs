using System;
using System.Linq;
using NUnit.Framework;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Box : MonoBehaviour
{
    public readonly int boxId;
    public readonly BoxTypes boxType;
    public bool IsGrounded { get; private set; } 
    public Rigidbody2D rb { get; private set; }
    public BoxCollider2D collider { get; private set; }
    public bool isFinished;

    public Box(int boxId, BoxTypes boxType)
    {
        this.boxId = boxId;
        this.boxType = boxType;
    }

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
    }
}
