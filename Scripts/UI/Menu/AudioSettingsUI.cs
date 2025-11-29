using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AudioSettingsUI : MonoBehaviour
{
    private const string MUSIC_VOLUME = "musicVolume";
    private const string SFX_VOLUME = "sfxVolume";

    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundSlider;

    private MusicManager _musicManager;

    [Inject]
    public void Construct(MusicManager musicManager)
    {
        _musicManager = musicManager;
    }

    private void Start() 
    {
        LoadVolume();
    } 

    public void SetMusicVolume()
    {
        float volume = _musicSlider.value;
        _musicManager.SetMusicVolume(volume);
    }

    public void SetSFXVolume()
    {
        float volume = _soundSlider.value;
        SFX.Instance.SetSFXVolume(volume);
    }

    private void LoadVolume()
    {
        _musicSlider.value = PlayerPrefs.GetFloat(MUSIC_VOLUME, 0.5f);
        _soundSlider.value = PlayerPrefs.GetFloat(SFX_VOLUME, 0.5f);
    }
}