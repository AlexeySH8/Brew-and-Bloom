using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestPlacer : MonoBehaviour
{
    [SerializeField] List<Table> _tables;
    private List<Table> _freeTables;
    private List<GameObject> _guests;

    [Header("Test")]
    [SerializeField] private List<GuestData> _allGuestsData;

    private void Awake()
    {
        // _freeTables = new List<Table>(_tables);

        //if (GuestsManager.Instance != null &&
        //    GuestsManager.Instance.HasGuestForDay)
        //    Place(GuestsManager.Instance.GuestsForDay);
        //else
        //    Debug.Log("There are no guests.");

        // Place(TestGuestsForDay());
    }

    // DELEATE LATER
    private void Start()
    {
        _freeTables = new List<Table>(_tables);
        var testList = TestGuestsForDay();
        Place(testList);
        OrdersPanel.Instance.AddOrders(testList);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            ReRespawn();
    }

    private void ReRespawn()
    {
        foreach (var guest in _guests)
            Destroy(guest);
        foreach (var table in _tables)
            table.Clear();
        _freeTables = new List<Table>(_tables);
        var testList = TestGuestsForDay();
        OrdersPanel.Instance.AddOrders(testList);
        Place(testList);
    }

    private void Place(IReadOnlyList<Guest> guestsForDay)
    {
        _guests = new List<GameObject>();
        foreach (Guest guest in guestsForDay)
        {
            GameObject instance = Instantiate(guest.Data.GuestPrefab,
                transform.position, Quaternion.identity);

            if (instance.TryGetComponent(out GuestCreature creature))
            {
                creature.Init(guest);
                SetTablePosition(instance);
                _guests.Add(instance);
            }
            else
                Debug.LogError($"{instance.name} does not have GuestCreature");
        }
    }

    private void SetTablePosition(GameObject guest)
    {
        if (_freeTables.Count == 0)
        {
            Debug.Log("All tables are occupied");
            return;
        }
        Table table = _freeTables[Random.Range(0, _freeTables.Count)];
        table.SitDown(guest);
        if (!table.IsFree())
            _freeTables.Remove(table);
    }

    private List<Guest> TestGuestsForDay()
    {
        Debug.Log("TEST GUESTS appeared");

        var allGuests = SpawnTestGuests();
        var testGuestsForDay = new List<Guest>();
        int count = UnityEngine.Random.Range(3, allGuests.Count);
        List<Guest> temp = new List<Guest>(allGuests);

        // mixes up the guests
        for (int i = 0; i < temp.Count; i++)
        {
            int r = UnityEngine.Random.Range(0, allGuests.Count);
            (temp[i], temp[r]) = (temp[r], temp[i]);
        }

        for (int i = 0; i < count; i++)
        {
            Guest guest = temp[i];
            guest.MakeOrder();
            testGuestsForDay.Add(guest);
        }
        return testGuestsForDay;
    }

    private List<Guest> SpawnTestGuests()
    {
        var testGuests = new List<Guest>(); // then it will load from saves
        if (testGuests == null || testGuests.Count == 0)
        {
            foreach (GuestData guestData in _allGuestsData)
            {
                testGuests.Add(new Guest(guestData));
            }
        }
        return testGuests;
    }
}
