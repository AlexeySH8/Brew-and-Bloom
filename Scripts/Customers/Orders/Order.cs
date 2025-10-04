using UnityEngine;

[System.Serializable]
public class Order
{
    public Guest Guest { get; private set; }
    [field: SerializeField] public DishData Dish { get; private set; }
    [field: SerializeField] public int Payment { get; private set; }
    [field: SerializeField] public bool IsCompleted { get; set; }

    public Order(Guest guest, DishData dishData, int payment)
    {
        Guest = guest;
        Dish = dishData;
        Payment = payment;
        IsCompleted = false;
    }
}
