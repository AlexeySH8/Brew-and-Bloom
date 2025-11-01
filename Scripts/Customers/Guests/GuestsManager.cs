using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GuestsManager : MonoBehaviour
{
    public event Action<IReadOnlyList<Guest>> OnGuestsArrived;
    public IReadOnlyList<Guest> GuestForDay => _guestForDay;

    [SerializeField] private int _minGuestCount;

    private GuestSaveSystem _guestSaveSystem;
    private GameSceneManager _gameSceneManager;
    private List<Guest> _allGuests;
    private List<Guest> _guestForDay;

    [Inject]
    public void Construct(GameSceneManager gameSceneManager, GuestSaveSystem guestSaveSystem)
    {
        _gameSceneManager = gameSceneManager;
        _guestSaveSystem = guestSaveSystem;
        SubscribeToEvents();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            foreach (var guest in LoadGuestForDay())
            {
                Debug.Log(guest.CurrentOrder.Dish.name);
            }
        }
    }

    private void Start()
    {
        if (_guestSaveSystem.AllGuests == null ||
           _guestSaveSystem.AllGuests.Count == 0)
        {
            Debug.LogError("Problems loading guests");
            return;
        }
        _allGuests = _guestSaveSystem.AllGuests;
    }

    private void SubscribeToEvents()
    {
        _gameSceneManager.OnHouseLoaded += ChooseGuestsForDay;
        _gameSceneManager.OnTavernLoaded += RestoreGuestForDay; // if the tavern was loaded from a save
        _gameSceneManager.OnTavernUnloading += ClearGuestForDay;
    }

    private void ChooseGuestsForDay()
    {
        var loadGuestsForDay = LoadGuestForDay();

        if (loadGuestsForDay.Count > 0)
            _guestForDay = loadGuestsForDay;
        else
            _guestForDay = ChooseNewGuestsForDay();

        OnGuestsArrived?.Invoke(_guestForDay);
    }

    private List<Guest> ChooseNewGuestsForDay()
    {
        List<Guest> newGuestsForDay = new List<Guest>();
        int count = UnityEngine.Random.Range(_minGuestCount, _allGuests.Count);
        List<Guest> temp = new List<Guest>(_allGuests);
        // mixes up the guests
        for (int i = 0; i < temp.Count; i++)
        {
            int r = UnityEngine.Random.Range(0, _allGuests.Count);
            (temp[i], temp[r]) = (temp[r], temp[i]);
        }

        for (int i = 0; i < count; i++)
        {
            Guest guest = temp[i];
            guest.MakeOrder();
            newGuestsForDay.Add(guest);
        }
        return newGuestsForDay;
    }

    private void RestoreGuestForDay()
    {
        if (_guestForDay == null)
            _guestForDay = LoadGuestForDay();
    }

    private List<Guest> LoadGuestForDay()
    {
        List<Guest> loadGuestsForDay = new List<Guest>();
        foreach (Guest guest in _allGuests)
        {
            if (guest.CurrentOrder != null &&
                guest.CurrentOrder.Dish.IngredientsMask != 0)
                loadGuestsForDay.Add(guest);
        }
        return loadGuestsForDay;
    }

    private void ClearGuestForDay()
    {
        foreach (Guest guest in _guestForDay)
            guest.CurrentOrder = null;
    }

    private void OnDisable()
    {
        _gameSceneManager.OnHouseLoaded -= ChooseGuestsForDay;
        _gameSceneManager.OnTavernLoaded -= RestoreGuestForDay;
        _gameSceneManager.OnTavernUnloading -= ClearGuestForDay;
    }
}