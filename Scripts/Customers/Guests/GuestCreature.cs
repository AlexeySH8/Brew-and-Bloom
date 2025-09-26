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

        ShowOrderDisplay();
    }

    public void Interact() => _guest.StartDialogue();

    public void Receive(GameObject heldItem)
    {
        if (_guest == null)
        {
            Debug.LogError("Guest creature is not initialized");
            return;
        }

        if (heldItem.TryGetComponent(out Dish dish))
        {
            dish.GetComponent<BaseHoldItem>().Discard();
            HideOrderDisplay();

            if (dish.Data.IngredientsMask == _guest.CurrentOrder.Dish.IngredientsMask)
                _guest.CurrentOrder.MarkAsCompleted();

            _guest.StartDialogue();
        }
    }

    private void OnDestroy()
    {
        _guest.EndDialogue();
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