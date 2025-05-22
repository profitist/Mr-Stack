using System;
using Unity.VisualScripting;
using UnityEngine;

public class grass_particle : MonoBehaviour
{
    ParticleSystem ps;

    public void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public void FixedUpdate()
    {
        if (!Player.Instance.IsRunning || Player.Instance.IsJumping)
        {
            ps.Stop();
        }
        ps.Play();
    }
}