using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using menu;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Debug = System.Diagnostics.Debug;

namespace checkPoints.scripts
{
    public class FinalCheckpoint : MonoBehaviour
    {
        private FinalCheckpoint Instance;
        [SerializeField] Camera mainCamera;
        private Rigidbody2D Text;
        [SerializeField] LevelManager levelManager;
        private GameObject thought;
        [SerializeField] GameObject box;
        [SerializeField] private GameObject sign;
        public static bool Final { get; private set; }
        private void Awake()
        {
            box.SetActive(false);
            Instance = this;
            Instance.GetComponentInChildren<SpriteRenderer>().enabled = false;
            Instance.GetComponentInChildren<Animator>().enabled = false;
            thought = GetComponentsInChildren<SpriteRenderer>().Where(x => x.gameObject.CompareTag("tick")).First().gameObject;
            thought.SetActive(false);
            sign.SetActive(false);
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
                box.SetActive(true);
                sign.SetActive(true);
            }
        }

        private IEnumerator AnimateFinal()
        {
            var wait = new Stopwatch();
            wait.Start();
            while (wait.ElapsedMilliseconds < 2000)
            {
                yield return null;
            }
            Final = true;
            while (wait.ElapsedMilliseconds < 8000)
            {
                Player.Instance.rb.linearVelocity = Vector2.right;
                yield return null;
            }
            FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None).First().enabled = false;
            var rb = mainCamera.GetComponent<Rigidbody2D>();
            thought.SetActive(true);
            Player.Instance.rb.simulated = false;
            wait.Restart();
            while (wait.ElapsedMilliseconds < 2000)
            {
                Final = false;
                yield return null;
            }

            while (wait.ElapsedMilliseconds < 27000)
            {
                rb.linearVelocityY = 0.7f;
                yield return null;
            }

            while (wait.ElapsedMilliseconds < 37000)
            {
                rb.linearVelocityY = 0;
                yield return null;
            }
            SceneManager.LoadScene("MainMenu");
        }
    }
}