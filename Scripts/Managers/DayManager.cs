using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance { get; private set; }

    [field: SerializeField] public int CurrentDay { get; private set; }

    public event Action OnStartDay;
    public event Action OnEndDay;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        CurrentDay = 1;
    }

    public void StartDay() => OnStartDay?.Invoke();

    public void EndDay()
    {
        CurrentDay++;
        OnEndDay?.Invoke();       
    }
}
