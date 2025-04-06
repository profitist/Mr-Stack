using System;
using UnityEngine;

public class NewEmptyCSharpScript : MonoBehaviour
{
    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

}

