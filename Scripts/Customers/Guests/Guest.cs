using System;
using UnityEngine;

public class Guest
{
    public GuestData Data { get; private set; }
    public Order CurrentOrder { get; set; }
    public int DialoguePartIndex { get; private set; }
    public bool IsServed { get; set; }

    public event Action<Guest, bool> OnOrderCompleted;

    private Recipes _recipes;
    private PlayerWallet _playerWallet;
    private GuestDialogue _guestDialogue;

    public Guest(GuestData data, Recipes recipes, PlayerWallet playerWallet,
         int dialoguePartIndex = 0, bool isServed = false)
    {
        Data = data;
        DialoguePartIndex = dialoguePartIndex;
        IsServed = isServed;
        _recipes = recipes;
        _playerWallet = playerWallet;
        _guestDialogue = new GuestDialogue(Data);
    }

    public void MakeOrder()
    {
        if (CurrentOrder != null) return;
        IsServed = false;
        DishData dish = _recipes.GetRandomDish();
        int payment = UnityEngine.Random.Range(Data.MinPayment, Data.MaxPayment);
        CurrentOrder = new Order(this, dish, payment, false);
    }

    public void CompleteOrder(int dishIngredientsMask)
    {
        IsServed = true;
        CurrentOrder.IsCompleted =
            dishIngredientsMask == CurrentOrder.Dish.IngredientsMask;

        if (CurrentOrder.IsCompleted)
        {
            _playerWallet.AddToDailyEarning(CurrentOrder.Payment);
        }
        OnOrderCompleted?.Invoke(this, CurrentOrder.IsCompleted);
    }

    public void StartDialogue() => _guestDialogue.StartDialogue(DialoguePartIndex);

    public void SetNextDialoguePart()
    {
        DialoguePartIndex++;
    }

    public void RestoreFromSaveData(Guest guestSaveData, Recipes recipes, PlayerWallet playerWallet)
    {
        DialoguePartIndex = guestSaveData.DialoguePartIndex;
        IsServed = guestSaveData.IsServed;
        CurrentOrder = null;

        if (guestSaveData.CurrentOrder != null &&
            guestSaveData.CurrentOrder.Dish.IngredientsMask != 0)
        {
            if (recipes.TryGetDish(
                guestSaveData.CurrentOrder.Dish.IngredientsMask, out GameObject dishObj))
            {
                var loadDish = dishObj.GetComponent<Dish>();
                CurrentOrder = new Order(this, loadDish.Data,
                    guestSaveData.CurrentOrder.Payment, guestSaveData.CurrentOrder.IsCompleted);
            }
        }
    }

    public override string ToString() => Data.Name;
}
