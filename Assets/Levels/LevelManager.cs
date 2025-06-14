using System.Collections;
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
    public int length {get; private set;}
    public int placeCounter {get; private set;}

    public LevelManager Instance {get; private set;}
    
    void Awake()
    {
        Instance = gameObject.GetComponent<LevelManager>();
        items = FindObjectsByType<BoxCheckpoint>(FindObjectsSortMode.None);
        length = items.Length;
        Debug.Log(items.Length);
        Cursor.visible = false;
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
            LevelMenu.nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
            
            StartCoroutine(WaitBeforeTransition());
        }
    }
    
    private void ActionOnEggCrushing(BoxUpdating boxUpdating)
    {
        GameInput.Instance.playerInputActions.Disable();
        GameInput.IsDead = true;
    }

    private IEnumerator WaitBeforeTransition()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("levelMenu");
        GameInput.Instance.playerInputActions.Disable();
        yield return null;
    } 
}
