using System;
using UnityEngine;

public class Guest
{
    public GuestData Data { get; private set; }
    public Order CurrentOrder { get; private set; }
    public bool IsServed { get; private set; }
    public event Action<Guest, bool> OnOrderCompleted;

    private Recipes _recipes;
    private PlayerWallet _playerWallet;
    private GuestDialogue _guestDialogue;

    public Guest(GuestData data, Recipes recipes, PlayerWallet playerWallet)
    {
        Data = data;
        _recipes = recipes;
        _playerWallet = playerWallet;
        _guestDialogue = new GuestDialogue(Data);
    }

    public void MakeOrder()
    {
        IsServed = false;
        DishData dish = _recipes.GetRandomDish();
        int payment = UnityEngine.Random.Range(Data.MinPayment, Data.MaxPayment);
        CurrentOrder = new Order(this, dish, payment);
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

    public void StartDialogue() => _guestDialogue.StartDialogue();

    public void SetNextDialoguePart() => _guestDialogue.SetNextDialoguePart();

    public override string ToString()
    {
        return Data.Name;
    }
}
