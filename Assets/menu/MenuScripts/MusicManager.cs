using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    [SerializeField] private AudioSource musicSource;
    private float defaultVolume = 0.5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", defaultVolume);
        }
        else Destroy(gameObject);
    }

    public void SetVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public float GetCurrentVolume()
    {
        return musicSource.volume;
    }
}
