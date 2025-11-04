using UnityEngine;

public class GuestCreature : MonoBehaviour, IReceiveHeldItem, IFreeInteractable
{
    private Guest _guest;
    private GameObject _orderDisplay;
    private SpriteRenderer _dishDisplay;

    public void Init(Guest guest)
    {
        _guest = guest;

        _orderDisplay = transform.Find("OrderContainer")?.gameObject;
        if (_orderDisplay == null)
        {
            Debug.LogError($"{gameObject.name} OrderContainer is not specified");
            return;
        }

        _dishDisplay = _orderDisplay.transform
            .Find("DishDisplay")
            ?.GetComponent<SpriteRenderer>();
        if (_dishDisplay == null)
            Debug.LogError($"{gameObject.name} DishDisplay is not specified");

        if (_guest.IsServed)
            HideOrderDisplay();
        else
            ShowOrderDisplay();
    }

    public void Interact()
    {
        if (_guest.IsServed)
            _guest.StartDialogue();
    }

    public bool TryReceive(BaseHoldItem heldItem)
    {
        bool reciveResult = false;
        if (_guest == null)
        {
            Debug.LogError("Guest creature is not initialized");
            return false;
        }

        if (!_guest.IsServed && heldItem.TryGetComponent(out Dish dish))
        {
            reciveResult = true;
            dish.Discard();
            HideOrderDisplay();
            _guest.CompleteOrder(dish.Data.IngredientsMask);
        }
        _guest.StartDialogue();
        return reciveResult;
    }

    private void ShowOrderDisplay()
    {
        _orderDisplay.SetActive(true);
        _dishDisplay.sprite = _guest.CurrentOrder.Dish.Icon;
    }

    private void HideOrderDisplay()
    {
        _orderDisplay.SetActive(false);
    }
}