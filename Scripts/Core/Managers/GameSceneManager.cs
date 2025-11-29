using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameSceneManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject _houseLoadingUI;
    [SerializeField] private GameObject _tavernLoadingUI;
    private const string HouseSceneName = "House";
    private const string TavernSceneName = "Tavern";

    public string SavedSceneName { get; private set; } = HouseSceneName;
    public event Action OnTavernLoaded;
    public event Action OnHouseLoaded;
    public event Action OnTavernUnloading;
    public event Action OnHouseUnloading;
    private IDataPersistenceManager _persistenceManager;

    public string CurrenSceneName => SceneManager.GetActiveScene().name;

    [Inject]
    public void Construct(IDataPersistenceManager dataPersistenceManager)
    {
        _persistenceManager = dataPersistenceManager;
        _persistenceManager.Register(this);
    }

    private void Awake()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case HouseSceneName:
                _persistenceManager.LoadGame();
                OnHouseLoaded?.Invoke();
                _persistenceManager.SaveGame();
                break;
            case TavernSceneName:
                _persistenceManager.LoadGame();
                OnTavernLoaded?.Invoke();
                _persistenceManager.SaveGame();
                break;
        }
    }

    public void LoadSavedScene()
    {
        switch (_persistenceManager.GameData.SavedSceneName)
        {
            case HouseSceneName:
                LoadHouseScene();
                break;
            case TavernSceneName:
                LoadTavernScene();
                break;
        }
    }

    public void LoadHouseScene()
    {
        if (CurrenSceneName == TavernSceneName)
        {
            OnTavernUnloading?.Invoke();
            _persistenceManager.SaveGame();
        }
        StartCoroutine(LoadSceneRoutine(HouseSceneName, _houseLoadingUI));
    }

    public void LoadTavernScene()
    {
        if (CurrenSceneName == HouseSceneName)
        {
            OnHouseUnloading?.Invoke();
            _persistenceManager.SaveGame();
        }
        StartCoroutine(LoadSceneRoutine(TavernSceneName, _tavernLoadingUI));
    }

    private IEnumerator LoadSceneRoutine(string sceneName, GameObject loadingUI)
    {
        if (loadingUI == null)
        {
            SceneManager.LoadScene(sceneName);
            yield break;
        }

        loadingUI.SetActive(true);
        Animator anim = loadingUI.GetComponent<Animator>();

        anim.Play("StartLoading");
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        anim.Play("LoopLoading");

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        yield return new WaitUntil(() => op.isDone);

        anim.Play("EndLoading");
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        loadingUI.SetActive(false);
    }

    public void LoadData(GameData gameData)
    {
        SavedSceneName = gameData.SavedSceneName;
    }

    public void SaveData(GameData gameData)
    {
        gameData.SavedSceneName = CurrenSceneName;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }
}
