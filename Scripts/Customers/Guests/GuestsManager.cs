using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GuestsManager : MonoBehaviour
{
    public event Action<IReadOnlyList<Guest>> OnGuestsArrived;
    public IReadOnlyList<Guest> GuestForDay => _guestForDay;

    [SerializeField] private List<GuestData> _allGuestsData;
    [SerializeField] private int _minGuestCount;

    private GameManager _gameManager;
    private Recipes _recipes;
    private PlayerWallet _playerWallet;
    private List<Guest> _allGuests;
    private List<Guest> _guestForDay;

    [Inject]
    public void Construct(GameManager gameManager, Recipes recipes, PlayerWallet playerWallet)
    {
        _gameManager = gameManager;
        _recipes = recipes;
        _playerWallet = playerWallet;

        LoadGuests();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            ChooseGuestsForDay();
    }

    private void SubscribeToEvents()
    {
        _gameManager.OnGameStart += ChooseGuestsForDay;
    }

    private void OnDisable()
    {
        _gameManager.OnGameStart -= ChooseGuestsForDay;
    }

    private void ChooseGuestsForDay()
    {
        _guestForDay = new List<Guest>();

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
            _guestForDay.Add(guest);
        }

        OnGuestsArrived?.Invoke(_guestForDay);
    }

    private void LoadGuests()
    {
        _allGuests = new List<Guest>(); // then it will load from saves
        if (_allGuests == null || _allGuests.Count == 0)
        {
            foreach (GuestData guestData in _allGuestsData)
            {
                _allGuests.Add(new Guest(guestData, _recipes, _playerWallet));
            }
        }
    }
}