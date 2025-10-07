using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameSceneManager : MonoBehaviour
{
    private const string HouseSceneName = "House";
    private const string TavernSceneName = "Tavern";

    public event Action OnTavernLoaded;
    public event Action OnHouseLoaded;

    private void Awake()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case HouseSceneName:
                OnHouseLoaded?.Invoke();
                break;
            case TavernSceneName:
                OnTavernLoaded?.Invoke();
                break;
        }
    }

    public void LoadHouseScene() => SceneManager.LoadScene(HouseSceneName);

    public void LoadTavernScene() => SceneManager.LoadScene(TavernSceneName);
}
