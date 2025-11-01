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
        SetDisplayContinueButton();
    }

    private void SetFunctionsToButtons()
    {
        _newGameButton.onClick.AddListener(NewGame);
        _continueGameButton.onClick.AddListener(ContinueGame);
    }

    private void NewGame()
    {
        _dataPersistenceManager.NewGame();
        _gameSceneManager.LoadHouseScene();
    }

    private void ContinueGame()
    {
        _dataPersistenceManager.LoadGame();
        _gameSceneManager.LoadCurrentScene();
    }

    private void SetDisplayContinueButton()
    {
        _continueGameButton.gameObject
            .SetActive(_dataPersistenceManager.HasSave());
    }
}
