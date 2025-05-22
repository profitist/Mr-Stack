using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TMP_Text musicButtonText;
    [SerializeField] private TMP_Text volumeText;
    [SerializeField] private Slider volumeSlider;
    private bool isMusicOn;
    
    private void Start()
    {
        var savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = savedVolume;
        audioSource.volume = savedVolume;
        UpdateVolumeText(savedVolume);
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    
    public void StartGame()
    {
        SceneManager.LoadScene("tutorial");
    }
    
    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        if (isMusicOn)
            audioSource.Play();
        else audioSource.Pause();
        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        musicButtonText.text = isMusicOn ? "Music: ON" : "Music: OFF";
    }
    
    private void OnVolumeChanged(float volume)
    {
        audioSource.volume = volume;
        UpdateVolumeText(volume);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    private void UpdateVolumeText(float volume)
    {
        volumeText.text = Mathf.RoundToInt(volume * 100) + "/100";
    }
}
