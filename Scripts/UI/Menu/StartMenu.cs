using TMPro;
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
        _exitButton.onClick.AddListener(Application.Quit);
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
        _gameSceneManager.LoadSavedScene();
    }

    private void SetDisplayContinueButton()
    {
        if (!_dataPersistenceManager.HasSave())
        {
            _continueGameButton.interactable = false;
            var text = _continueGameButton
                .GetComponentInChildren<TextMeshProUGUI>();
            Color colorText = text.color;
            colorText.a = 0.5f;
            text.color = colorText;
        }
    }

    public void PlayClickDefault() => SFX.Instance.PlayClickButtonDefault();

    public void PlayClickClose() => SFX.Instance.PlayClickButtonClose();
}
