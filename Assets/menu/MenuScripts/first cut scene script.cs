using System;
using System.Collections;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace menu
{
    public class FirstCutSceneScript : MonoBehaviour
    {
        [SerializeField] private GameObject box;
        [SerializeField] BoxUpdating boxUpdating;
        [SerializeField] private Canvas canvas;
        private TMP_Text text;
        private GameObject dino;
        private Rigidbody2D dinoRb;
        [SerializeField] Transform holdPoint;
        [SerializeField] Animator animator;
        

        private void Awake()
        {
            dino = gameObject;
            box.SetActive(false);
            text = canvas.GetComponentInChildren<TMP_Text>();
            dinoRb = dino.GetComponent<Rigidbody2D>();
            StartCoroutine(PlayCutScene());

        }

        private IEnumerator PlayCutScene()
        {
            var timer = 0f;
            animator.Play("idle");
            while (timer < 2)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            box.SetActive(true);
            while (timer < 3)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            while (!boxUpdating.IsGrounded)
            {
                yield return null;
            }
            yield return null;
            dinoRb.linearVelocityY = 10;
            while (timer < 5)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            animator.SetBool("isRunning", true);
            dinoRb.linearVelocityX = 5;
            yield return null;
            StartCoroutine(AnimatePickingBox(box, 0));
            StartCoroutine(TypeText());
            yield return null;
            while (timer < 10)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            SceneManager.LoadScene("MainMenu");
        }
        
        private IEnumerator TypeText()
        {
            var fullText = "DeliveRex";
            for (int i = 0; i <= fullText.Length; i++)
            {
                text.text = fullText.Substring(0, i);
                yield return new WaitForSeconds(0.5f);
            }
        }
        private IEnumerator AnimatePickingBox(GameObject box, float stackHeight)
        {
            var duration = 0.5f;
            var time = 0f;
            var start = box.transform.position;
            var end = holdPoint.position + new Vector3(0, stackHeight * 1, 0);
            var constVel = 10f;
            var rb = box.GetComponent<Rigidbody2D>();
            var cl = box.GetComponent<BoxCollider2D>();
            cl.enabled = false;
            var velX = (end.x - start.x) / duration;
            if (rb) rb.bodyType = RigidbodyType2D.Kinematic;
            while (time < duration)
            {
                var t = time / duration;
                var verticalSpeed = Mathf.Cos(t * Mathf.PI) * constVel + (end.y - start.y) / duration;
                rb.linearVelocity =  new Vector2(
                    velX + dinoRb.linearVelocityX, 
                    verticalSpeed);
                time += Time.deltaTime;
                yield return null;
            }
            rb.bodyType = RigidbodyType2D.Dynamic;
            cl.enabled = true;
            rb.simulated = false;
            box.transform.SetParent(holdPoint);
            box.transform.localPosition = new Vector3(0, stackHeight, 0);
        }
    }
}