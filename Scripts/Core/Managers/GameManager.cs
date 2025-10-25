using System;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    public event Action OnGameStart;

    private IDataPersistenceManager _dataPresistenceManager;

    [Inject]
    public void Construct(IDataPersistenceManager dataPresistenceManager)
    {
        _dataPresistenceManager = dataPresistenceManager;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            _dataPresistenceManager.SaveGame();
            Debug.Log("Data is Saved");
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            _dataPresistenceManager.LoadGame();
            Debug.Log("Data is Load");
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            _dataPresistenceManager.NewGame();
            Debug.Log("NewGame");
        }
    }

    private void StartGame()
    {
        OnGameStart?.Invoke();
    }
}
