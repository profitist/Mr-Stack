using System;
using UnityEditor;
using UnityEngine;

namespace Environment.Scripts
{
    public class DoorScript : MonoBehaviour
    {
        private GameObject door;
        [SerializeField] PlayerBoxHolder playerBoxHolder;

        private void Awake()
        {
            door = gameObject;
        }

        private void FixedUpdate()
        {
            if (playerBoxHolder.boxes.Count == 2)
            {
                door.SetActive(false);
            }
        }
    }
}