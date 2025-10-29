using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button _continueGameButton;
    [SerializeField] private Button _newGameButton;

    private IDataPersistenceManager _dataPersistenceManager;
    private GameSceneManager _gameSceneManager;

    [Inject]
    public void Construct(IDataPersistenceManager dataPersistenceManager,
        GameSceneManager gameSceneManager)
    {
        _dataPersistenceManager = dataPersistenceManager;
        _gameSceneManager = gameSceneManager;
    }

    private void Awake()
    {
        SetFunctionsToButtons();
        _continueGameButton.gameObject
            .SetActive(_dataPersistenceManager.HasSave());
    }

    private void SetFunctionsToButtons()
    {
        _continueGameButton.onClick
            .AddListener(_gameSceneManager.LoadCurrentScene);

        _newGameButton.onClick
            .AddListener(_dataPersistenceManager.NewGame);
        _newGameButton.onClick
            .AddListener(_gameSceneManager.LoadHouseScene);
    }
}
