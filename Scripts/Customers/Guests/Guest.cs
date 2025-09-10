using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guest
{
    private GuestData _data;

    public Order CurrentOrder { get; private set; }
    public bool IsServed { get; private set; }

    public Guest(GuestData data)
    {
        _data = data;
    }

    public void MakeOrder()
    {
        IsServed = false;
        DishData dish = Recipes.GetRandomDish();
        int payment = Random.Range(_data.MinPayment, _data.MaxPayment);
        CurrentOrder = new Order(this, dish, payment);
    }
}
