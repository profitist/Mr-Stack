using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    void Start()
    {
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }
    
    private void ChangeVolume(float volume)
    {
        if (MusicManager.instance != null)
            MusicManager.instance.SetVolume(volume);
    }
    public void ExitGame()
    {
        SceneManager.LoadScene("mainMenu");
    }
    
    // Update is called once per frame
    void Update()
    {
        volumeSlider.value = MusicManager.instance.musicSource.volume;
    }
}
