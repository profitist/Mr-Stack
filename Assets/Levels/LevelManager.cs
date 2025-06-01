using System.Threading;
using menu;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class LevelManager : MonoBehaviour
{
    [SerializeField] 
    private AudioSource successSound;
    public BoxCheckpoint[] items{get; private set;}
    public int placeCounter {get; private set;}
    
    void Awake()
    {
        items = FindObjectsByType<BoxCheckpoint>(FindObjectsSortMode.None);
        Debug.Log(items.Length);
    }

    private void OnEnable()
    {
        BoxCheckpoint.OnCheckpointFilling += ActionOnPlacing;
        BoxUpdating.OnEggCrushing += ActionOnEggCrushing;
    }

    private void OnDisable()
    {
        BoxCheckpoint.OnCheckpointFilling -= ActionOnPlacing;
    }


    private void ActionOnPlacing(BoxCheckpoint checkpoint)
    {
        placeCounter++;
        if (placeCounter >= items.Length)
        {
            GameInput.Instance.playerInputActions.Disable();
            LevelMenu.nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
            if (SceneManager.GetActiveScene().buildIndex == 8)
                SceneManager.LoadScene("mainMenu");
            else
                SceneManager.LoadScene("levelMenu");
        }
    }
    
    private void ActionOnEggCrushing(BoxUpdating boxUpdating)
    {
        GameInput.Instance.playerInputActions.Disable();
        GameInput.IsDead = true;
    }
}
