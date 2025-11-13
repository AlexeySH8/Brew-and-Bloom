using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Button _continueGameButton;
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _exitButton;

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
        _exitButton.onClick.AddListener(ExitGame);
    }

    private void NewGame()
    {
        SFX.Instance.PlayClickButtonNewGame();
        _dataPersistenceManager.NewGame();
        _gameSceneManager.LoadHouseScene();
    }

    private void ContinueGame()
    {
        SFX.Instance.PlayClickButtonNewGame();
        _dataPersistenceManager.LoadGameMeta();
        _gameSceneManager.LoadCurrentScene();
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void SetDisplayContinueButton()
    {
        _continueGameButton.interactable = _dataPersistenceManager.HasSave();
    }
}
