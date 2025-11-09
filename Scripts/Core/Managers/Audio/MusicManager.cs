using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip _houseMusic;
    [SerializeField] private AudioClip _tavernMusic;

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

    private void SubcribeToEvents()
    {
        _gameSceneManager.OnHouseLoaded += EnterHouse;
        _gameSceneManager.OnHouseUnloading += ExitHouse;
        _gameSceneManager.OnTavernLoaded += EnterTavern;
        _gameSceneManager.OnTavernUnloading += ExitTavern;
    }

    private void EnterHouse()
    {
        _musicSource.clip = _houseMusic;
        _musicSource.Play();
    }

    private void ExitHouse()
    {

    }

    private void EnterTavern()
    {
        _musicSource.clip = _tavernMusic;
        _musicSource.Play();
    }

    private void ExitTavern()
    {

    }

    private void OnDestroy()
    {
        UnscribeFromEvents();
    }

    private void UnscribeFromEvents()
    {
        _gameSceneManager.OnHouseLoaded -= EnterHouse;
        _gameSceneManager.OnHouseUnloading -= ExitHouse;
        _gameSceneManager.OnTavernLoaded -= EnterTavern;
        _gameSceneManager.OnTavernUnloading -= ExitTavern;
    }
}
