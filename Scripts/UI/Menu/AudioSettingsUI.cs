using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    private const string MUSIC_VOLUME = "musicVolume";
    private const string SFX_VOLUME = "sfxVolume";

    private const string MUSIC_MIXER = "MusicVolume";
    private const string SFX_MIXER = "SFXVolume";

    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundSlider;

    void Start()
    {
        LoadVolume();
    }

    public void SetMusicVolume()
    {
        float volume = _musicSlider.value;
        _mixer.SetFloat(MUSIC_MIXER, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume()
    {
        float volume = _soundSlider.value;
        _mixer.SetFloat(SFX_MIXER, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(SFX_VOLUME, volume);
        PlayerPrefs.Save();
    }

    private void LoadVolume()
    {
        _musicSlider.value = PlayerPrefs.GetFloat(MUSIC_VOLUME, 0.5f);
        _soundSlider.value = PlayerPrefs.GetFloat(SFX_VOLUME, 0.5f);
        SetMusicVolume();
        SetSFXVolume();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        AudioListener.pause = !hasFocus;
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        AudioListener.pause = pauseStatus;
    }
}