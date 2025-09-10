using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestsManager : MonoBehaviour
{
    public static GuestsManager Instance;
    public event Action<Guest> OnGuestArrived;

    [SerializeField] private List<GuestData> _allGuestsData;
    [SerializeField] private int _minGuestCount = 3;
    [SerializeField] private float _minTimeNextGuest = 1f;
    [SerializeField] private float _maxTimeNextGuest = 3f;

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
    }

    private void Start()
    {
        //SubscribeToEvents();
        StartCoroutine(CheckOrders());
    }

    private IEnumerator CheckOrders()
    {
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(InviteGuestsForDay());
    }

    //private void SubscribeToEvents()
    //{
    //    GameManager.Instance.OnGameStart += PrepareGuestsForDay;
    //}

    //private void OnDisable()
    //{
    //    GameManager.Instance.OnGameStart -= PrepareGuestsForDay;
    //}

    private IEnumerator InviteGuestsForDay()
    {
        _guestForDay.Clear();

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
            OnGuestArrived.Invoke(guest);
            yield return new WaitForSeconds(UnityEngine.Random.Range(
                _minTimeNextGuest, _maxTimeNextGuest));
        }
    }

    private void LoadGuests()
    {
        foreach (GuestData guestData in _allGuestsData)
        {
            _allGuests.Add(new Guest(guestData));
        }
    }
}
