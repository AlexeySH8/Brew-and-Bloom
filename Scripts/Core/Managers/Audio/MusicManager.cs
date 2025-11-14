using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip _houseMusic;
    [SerializeField] private AudioClip _tavernMusic;
    [SerializeField] private AudioClip _menuMusic;

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
        EnterMenu();
    }

    private void SubcribeToEvents()
    {
        _gameSceneManager.OnHouseLoaded += EnterHouse;
        _gameSceneManager.OnTavernLoaded += EnterTavern;
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

    private void OnDestroy()
    {
        UnscribeFromEvents();
    }

    private void UnscribeFromEvents()
    {
        _gameSceneManager.OnHouseLoaded -= EnterHouse;
        _gameSceneManager.OnTavernLoaded -= EnterTavern;
    }
}
