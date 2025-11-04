using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class GuestSaveSystem : MonoBehaviour, IDataPersistence
{
    public List<Guest> AllGuests { get; private set; } = new();

    [SerializeField] private List<GuestData> _allGuestsData;

    private Recipes _recipes;
    private PlayerWallet _playerWallet;
    private IDataPersistenceManager _dataPersistenceManager;
    private GuestsManager _guestsManager;

    [Inject]
    public void Construct(Recipes recipes, PlayerWallet playerWallet,
        IDataPersistenceManager dataPersistenceManager, GuestsManager guestsManager)
    {
        _guestsManager = guestsManager;
        _recipes = recipes;
        _playerWallet = playerWallet;
        _dataPersistenceManager = dataPersistenceManager;
        _dataPersistenceManager.Register(this);
    }

    private void Awake()
    {
        CreateNewGuests();
    }

    public void LoadData(GameData gameData)
    {
        if (gameData.GuestsSaveData == null ||
           gameData.GuestsSaveData.Count != _allGuestsData.Count)
        {
            Debug.LogError("The list of loaded guests is empty or incorrect.");
            return;
        }

        foreach (var guestSaveData in gameData.GuestsSaveData)
        {
            GuestData guestData = _allGuestsData
                .FirstOrDefault(d => d.GuestId == guestSaveData.GuestId);
            if (guestData == null)
            {
                Debug.LogError("An invalid GuestID was saved.");
                return;
            }

            Guest loadGuest = new Guest(guestData, _recipes, _playerWallet,
                guestSaveData.DialoguePartIndex, guestSaveData.IsServed);

            CurrentOrderSaveData loadOrder = guestSaveData.CurrentOrderSaveData;
            Order currentOrder = null;
            if (loadOrder.IngredientsMask != 0) // the guest ordered a dish
            {
                if (_recipes.TryGetDish(loadOrder.IngredientsMask, out GameObject dishObj))
                {
                    Dish loadDish = dishObj.GetComponent<Dish>();
                    currentOrder = new Order(loadGuest, loadDish.Data,
                        loadOrder.Payment, loadOrder.IsCompleted);
                }
                else
                {
                    Debug.LogError("An invalid IngredientsMask was saved.");
                    return;
                }
            }
            loadGuest.CurrentOrder = currentOrder;
            Guest oldGuest = AllGuests.FirstOrDefault(g => g.Data.GuestId == loadGuest.Data.GuestId);
            oldGuest.RestoreFromSaveData(loadGuest, _recipes, _playerWallet);
        }
    }

    public void SaveData(GameData gameData)
    {
        List<GuestSaveData> guestsSaveData = new();
        foreach (var guest in AllGuests)
        {
            GuestSaveData guestSaveData = new();

            guestSaveData.GuestId = guest.Data.GuestId;
            guestSaveData.DialoguePartIndex = guest.DialoguePartIndex;
            guestSaveData.IsServed = guest.IsServed;

            CurrentOrderSaveData currentOrderSaveData = null;
            if (guest.CurrentOrder != null)
            {
                Order currentOrder = guest.CurrentOrder;
                currentOrderSaveData = new CurrentOrderSaveData();
                currentOrderSaveData.Payment = currentOrder.Payment;
                currentOrderSaveData.IngredientsMask = currentOrder.Dish.IngredientsMask;
                currentOrderSaveData.IsCompleted = currentOrder.IsCompleted;
            }
            guestSaveData.CurrentOrderSaveData = currentOrderSaveData;
            guestsSaveData.Add(guestSaveData);
        }

        if (guestsSaveData.Count != _allGuestsData.Count)
        {
            Debug.LogError("The list of saved guests is  incorrect.");
            return;
        }
        gameData.GuestsSaveData = guestsSaveData;
    }

    private void CreateNewGuests()
    {
        foreach (GuestData guestData in _allGuestsData)
        {
            AllGuests.Add(new Guest(guestData, _recipes, _playerWallet));
        }
    }

    private void OnDestroy()
    {
        _dataPersistenceManager.Unregister(this);
    }
}
