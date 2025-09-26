using UnityEngine;

public class Guest
{
    public GuestData Data { get; private set; }

    public Order CurrentOrder { get; private set; }
    public bool IsServed { get; private set; }

    private GuestDialogue _guestDialogue;

    public Guest(GuestData data)
    {
        Data = data;
        _guestDialogue = new GuestDialogue(data.DialogueData, 
            Data.Portrait, Data.Name);
    }

    public void MakeOrder()
    {
        IsServed = false;
        DishData dish = Recipes.GetRandomDish();
        int payment = Random.Range(Data.MinPayment, Data.MaxPayment);
        CurrentOrder = new Order(this, dish, payment);
    }

    public void StartDialogue() => _guestDialogue.StartDialogue();

    public void EndDialogue() => _guestDialogue.SetNextDialoguePart();

    public override string ToString()
    {
        return Data.Name;
    }
}
