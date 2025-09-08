using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestsManager : MonoBehaviour
{
    public static GuestsManager Instance;

    public event Action<int> OnGuestsSpawned;

    [SerializeField] private int _minGuestCount = 3;
    [SerializeField] private int _maxGuestCount = 7;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SpawnGuests()
    {
        int guestCount = UnityEngine.Random.Range(_minGuestCount, _maxGuestCount);
        OnGuestsSpawned?.Invoke(guestCount);
    }
}
