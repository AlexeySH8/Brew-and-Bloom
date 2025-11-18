using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameMenu : MonoBehaviour
{
    public static GameMenu Instance { get; private set; }

    [SerializeField] private Button _gameMenuButton;
    [SerializeField] private GameObject _gameMenu;
    [SerializeField] private Button _saveGameButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _exitButton;

    private GameSceneManager _gameSceneManager;
    private IDataPersistenceManager _dataPersistenceManager;

    [Inject]
    public void Construct(GameSceneManager gameSceneManager,
        IDataPersistenceManager dataPersistenceManager)
    {
        _gameSceneManager = gameSceneManager;
        _dataPersistenceManager = dataPersistenceManager;
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        _gameSceneManager.OnHouseLoaded += ShowGameMenu;
        _gameSceneManager.OnTavernLoaded += ShowGameMenu;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate ItemPool detected, destroying new one.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SetButtons();
        HideGameMenu();
    }

    private void SetButtons()
    {
        _gameMenuButton.onClick.AddListener(PauseGame);
        _continueButton.onClick.AddListener(ResumeGame);
        _saveGameButton.onClick.AddListener(_dataPersistenceManager.SaveGame);
        _exitButton.onClick.AddListener(Application.Quit);
    }

    private void PauseGame() => Time.timeScale = 0f;

    private void ResumeGame() => Time.timeScale = 1f;

    public void PlayClickDefault() => SFX.Instance.PlayClickButtonDefault();

    public void PlayClickClose() => SFX.Instance.PlayClickButtonClose();

    private void ShowGameMenu()
    {
        _gameMenuButton.gameObject.SetActive(true);
        _gameMenu.SetActive(false);
    }

    private void HideGameMenu()
    {
        _gameMenuButton.gameObject.SetActive(false);
        _gameMenu.SetActive(false);
    }

    private void UnsubscribeFromEvents()
    {
        _gameSceneManager.OnHouseLoaded -= ShowGameMenu;
        _gameSceneManager.OnTavernLoaded -= ShowGameMenu;
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }
}
