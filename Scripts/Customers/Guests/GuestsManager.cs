using System;
using System.Collections.Generic;
using UnityEngine;

public class GuestsManager : MonoBehaviour
{
    public static GuestsManager Instance;

    [SerializeField] private List<GuestData> _allGuestsData;
    [SerializeField] private int _minGuestCount = 3;
    public event Action<List<Guest>> OnGuestsPrepared;
    private List<Guest> _allGuests = new List<Guest>();
    private List<Guest> _guestForDay = new List<Guest>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        LoadGuests();
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            PrepareGuestsForDay();
        }
    }

    private void Start()
    {
        //SubscribeToEvents();
    }

    //private void SubscribeToEvents()
    //{
    //    GameManager.Instance.OnGameStart += PrepareGuestsForDay;
    //}

    //private void OnDisable()
    //{
    //    GameManager.Instance.OnGameStart -= PrepareGuestsForDay;
    //}

    private void PrepareGuestsForDay()
    {
        _guestForDay.Clear();

        int count = UnityEngine.Random.Range(_minGuestCount, _allGuests.Count);
        List<Guest> temp = new List<Guest>(_allGuests);
        for (int i = 0; i < count; i++)
        {
            int r = UnityEngine.Random.Range(0, _allGuests.Count);
            (temp[i], temp[r]) = (temp[r], temp[i]);
            Guest guest = temp[i];
            guest.MakeOrder();
            _guestForDay.Add(guest);
        }
        OnGuestsPrepared.Invoke(_guestForDay);
    }

    private void LoadGuests()
    {
        foreach (GuestData guestData in _allGuestsData)
        {
            _allGuests.Add(new Guest(guestData));
        }
    }
}
