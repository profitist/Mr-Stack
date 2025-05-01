using System;
using UnityEngine;

public class BoxVisual: MonoBehaviour
{
    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

}

