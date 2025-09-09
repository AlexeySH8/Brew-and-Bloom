public class Order 
{
    public DishData Dish { get; private set; }
    public int Payment { get; private set; }
    public bool IsOrderPlaced { get; private set; }

    public Order(DishData dishData, int payment)
    {
        Dish = dishData;
        Payment = payment;
        IsOrderPlaced = false;
    }

    public void MarkAsPlaced()
    {
        IsOrderPlaced = true;
    }
}
