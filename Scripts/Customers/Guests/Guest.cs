using UnityEngine;

public class Guest
{
    public GuestData Data { get; private set; }

    public Order CurrentOrder { get; private set; }
    public bool IsServed { get; private set; }

    public Guest(GuestData data)
    {
        Data = data;
    }

    public void MakeOrder()
    {
        IsServed = false;
        DishData dish = Recipes.GetRandomDish();
        int payment = Random.Range(Data.MinPayment, Data.MaxPayment);
        CurrentOrder = new Order(this, dish, payment);
    }

    public void StartDialogue()
    {
        Debug.Log("Dialogue Started");
    }
}
