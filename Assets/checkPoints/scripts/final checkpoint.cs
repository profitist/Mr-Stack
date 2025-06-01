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
        private FinalCheckpoint Instance;
        private Camera mainCamera;
        private Rigidbody2D Text;
        private LevelManager levelManager;
        private GameObject thought;
        private GameObject thought2;
        public static bool Final { get; private set; }
        private void Awake()
        {
            levelManager = FindObjectsByType<LevelManager>(FindObjectsSortMode.None).First();
            Instance = this;
            mainCamera = Camera.main;
            Instance.GetComponentInChildren<SpriteRenderer>().enabled = false;
            Instance.GetComponentInChildren<Animator>().enabled = false;
            thought = Player.Instance.GetComponentsInChildren<SpriteRenderer>().Where(x => x.gameObject.CompareTag("tick")).First().gameObject;
            thought.SetActive(false);
            thought2 = GetComponentInChildren<thoughtRouter>().gameObject;
            thought2.SetActive(false);
            
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
            if (levelManager.placeCounter == 2)
            {
                Instance.GetComponentInChildren<SpriteRenderer>().enabled = true;
                Instance.GetComponentInChildren<Animator>().enabled = true;
            }
        }

        private IEnumerator AnimateFinal()
        {
            var wait = new Stopwatch();
            wait = Stopwatch.StartNew();
            while (wait.ElapsedMilliseconds < 2000)
            {
                yield return null;
            }
            Final = true;
            while (wait.ElapsedMilliseconds < 6000)
            {
                Player.Instance.rb.linearVelocity = Vector2.right;
                yield return null;
            }
            Final = false;
            wait.Stop();
            FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None).First().enabled = false;
            wait = Stopwatch.StartNew();
            var rb = mainCamera.GetComponent<Rigidbody2D>();
            thought.SetActive(true);
            thought2.SetActive(true);
            while (wait.ElapsedMilliseconds < 2000)
            {
                yield return null;
            }
            while (wait.ElapsedMilliseconds < 27000)
            {
                rb.linearVelocityY = 0.7f;
                yield return null;
            }
            rb.linearVelocityY = 0;
            while (wait.ElapsedMilliseconds < 32000)
            {
                yield return null;
            }
            SceneManager.LoadScene("MainMenu");
        }
    }
}