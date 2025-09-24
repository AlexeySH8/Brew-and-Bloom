using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class GuestsManager : MonoBehaviour
{
    public static GuestsManager Instance;

    public event Action<IReadOnlyList<Guest>> OnGuestsArrived;

    [SerializeField] private List<GuestData> _allGuestsData;
    [SerializeField] private int _minGuestCount;

    private List<Guest> _allGuests;
    private List<Guest> _guestForDay;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        LoadGuests();
    }

    private void Start()
    {
        //SubscribeToEvents();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            CreateGuestsForDay();
    }

    //private void SubscribeToEvents()
    //{
    //    GameManager.Instance.OnGameStart += PrepareGuestsForDay;
    //}

    //private void OnDisable()
    //{
    //    GameManager.Instance.OnGameStart -= PrepareGuestsForDay;
    //}

    private void CreateGuestsForDay()
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
                _allGuests.Add(new Guest(guestData));
            }
        }
    }
}
