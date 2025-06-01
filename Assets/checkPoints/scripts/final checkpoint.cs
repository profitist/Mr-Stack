using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace checkPoints.scripts
{
    public class FinalCheckpoint : MonoBehaviour
    {
        private int placeCounter;
        private FinalCheckpoint Instance;
        private Camera mainCamera;
        private Rigidbody2D Text;
        public static bool Final { get; private set; }
        private void Awake()
        {
            placeCounter = FindObjectsByType<LevelManager>(FindObjectsSortMode.None).First().placeCounter;
            Instance = this;
            mainCamera = Camera.main;
            Instance.GetComponentInChildren<SpriteRenderer>().enabled = false;
            Instance.GetComponentInChildren<Animator>().enabled = false;
            
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                GameInput.Instance.playerInputActions.Disable();
                StartCoroutine(AnimateFinal());
            }
        }

        private void FixedUpdate()
        {
            if (placeCounter == 2)
            {
                Instance.GetComponentInChildren<SpriteRenderer>().enabled = true;
                Instance.GetComponentInChildren<Animator>().enabled = true;
                placeCounter = 0;
            }
        }

        private IEnumerator AnimateFinal()
        {
            var wait = new Stopwatch();
            wait = Stopwatch.StartNew();
            while (wait.ElapsedMilliseconds < 1000)
            {
                yield return null;
            }
            Final = true;
            while (wait.ElapsedMilliseconds < 4000)
            {
                Player.Instance.rb.linearVelocity = Vector2.right;
                yield return null;
            }
            Final = false;
            wait.Stop();
            FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None).First().enabled = false;
            wait = Stopwatch.StartNew();
            var rb = mainCamera.GetComponent<Rigidbody2D>();
            while (wait.ElapsedMilliseconds < 1000)
            {
                yield return null;
            }
            while (wait.ElapsedMilliseconds < 5000)
            {
                rb.linearVelocityY = 0.7f;
                yield return null;
            }
        }
    }
}