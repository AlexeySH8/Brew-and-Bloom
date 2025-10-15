using System;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    public event Action OnGameStart;

    private void StartGame()
    {
        OnGameStart?.Invoke();
    }
}
