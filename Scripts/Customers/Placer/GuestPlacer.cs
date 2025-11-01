using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GuestPlacer : MonoBehaviour
{
    [SerializeField] List<Table> _tables;

    private List<Table> _freeTables;
    private List<GameObject> _guests;
    private GuestsManager _guestsManager;

    [Inject]
    public void Construct(GuestsManager guestsManager)
    {
        _guestsManager = guestsManager;
        //SubscribeToEvents();
    }

    private void Start()
    {
        Place(_guestsManager.GuestForDay);
    }

    //private void SubscribeToEvents()
    //{
    //    _guestsManager.OnGuestsArrived += Place;
    //}

    //private void OnDisable()
    //{
    //    _guestsManager.OnGuestsArrived -= Place;
    //}

    public void Place(IReadOnlyList<Guest> guestsForDay)
    {
        Clear();
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

    private void Clear()
    {
        if (_guests != null)
            foreach (var guest in _guests)
                Destroy(guest);
        if (_tables != null)
            foreach (var table in _tables)
                table.Clear();

        _freeTables = new List<Table>(_tables);
        _guests = new List<GameObject>();
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
}
