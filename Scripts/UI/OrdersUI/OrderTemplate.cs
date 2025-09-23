using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OrderTemplate : MonoBehaviour
{
    [SerializeField] private Image GuestIcon;
    [SerializeField] private Image DishIcon;
    [SerializeField] private GameObject _completeStatus;
    [SerializeField] private GameObject _incompleteStatus;
    [SerializeField] private TextMeshProUGUI _price;

    public void DisplayOrder(Order order)
    {
        GuestIcon.sprite = order.Guest.Data.Portrait;
        DishIcon.sprite = order.Dish.Icon;
        _price.text = order.Payment.ToString();
    }

    public void SetStatus(bool isOrderCompleted)
    {
        if (isOrderCompleted)
        {
            _completeStatus.SetActive(true);
            _incompleteStatus.SetActive(false);
        }
        else
        {
            _completeStatus.SetActive(false);
            _incompleteStatus.SetActive(true);
        }

    }
}
