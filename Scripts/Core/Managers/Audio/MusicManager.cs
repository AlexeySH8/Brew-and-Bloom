using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using Zenject;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioClip _houseMusic;
    [SerializeField] private AudioClip _tavernMusic;
    [SerializeField] private AudioClip _menuMusic;

    private const string MUSIC_VOLUME = "musicVolume";
    private const string MUSIC_MIXER = "MusicVolume";
    private AudioSource _musicSource;
    private GameSceneManager _gameSceneManager;

    [Inject]
    public void Construct(GameSceneManager gameSceneManager)
    {
        _gameSceneManager = gameSceneManager;
        SubcribeToEvents();
    }

    private void Awake()
    {
        _musicSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        LoadVolume();
        EnterMenu();
    }

    private void SubcribeToEvents()
    {
        _gameSceneManager.OnHouseLoaded += EnterHouse;
        _gameSceneManager.OnTavernLoaded += EnterTavern;
    }

    public void SetMusicVolume(float volume)
    {
        _mixer.SetFloat(MUSIC_MIXER, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    private void EnterMenu()
    {
        _musicSource.clip = _menuMusic;
        _musicSource.Play();
    }

    private void EnterHouse()
    {
        _musicSource.clip = _houseMusic;
        _musicSource.Play();
    }

    private void EnterTavern()
    {
        _musicSource.clip = _tavernMusic;
        _musicSource.Play();
    }

    private void LoadVolume()
    {
        var volume = PlayerPrefs.GetFloat(MUSIC_VOLUME, 0.5f);
        SetMusicVolume(volume);
    }

    private void OnDestroy()
    {
        UnscribeFromEvents();
    }

    private void UnscribeFromEvents()
    {
        _gameSceneManager.OnHouseLoaded -= EnterHouse;
        _gameSceneManager.OnTavernLoaded -= EnterTavern;
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
