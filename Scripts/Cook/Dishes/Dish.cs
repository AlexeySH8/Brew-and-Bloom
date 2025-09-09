using UnityEngine;

public class Dish : BaseHoldItem
{
    [field: SerializeField] public DishData Data { get; private set; }
}
