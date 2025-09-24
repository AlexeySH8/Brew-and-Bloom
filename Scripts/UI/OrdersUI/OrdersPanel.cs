using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdersPanel : MonoBehaviour
{
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _orderTemplatePref;

    private Dictionary<Guest, GameObject> _activeOrders;
    private SlideAnimation _slideAnimation;
    private bool _isOpen;

    private void Awake()
    {
        _isOpen = false;
        _slideAnimation = GetComponent<SlideAnimation>();
    }

    private void Start()
    {
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        GuestsManager.Instance.OnGuestsArrived += AddOrders;
    }

    private void OnDisable()
    {
        GuestsManager.Instance.OnGuestsArrived -= AddOrders;
    }

    public void AddOrders(IReadOnlyList<Guest> guests)
    {
        Clear();
        foreach (Guest guest in guests)
        {
            Order order = guest.CurrentOrder;
            GameObject orderTemplate = Instantiate(_orderTemplatePref, _content);
            _activeOrders.Add(order.Guest, orderTemplate);
            orderTemplate
                .GetComponent<OrderTemplate>()
                .DisplayOrder(order);
        }
    }

    public void UpdateStatusOrder(bool isOrderCompleted, Guest guest)
    {
        OrderTemplate orderTemplate =
            _activeOrders[guest]
            ?.GetComponent<OrderTemplate>();

        if (orderTemplate == null)
        {
            Debug.LogError($"{guest.ToString()} orderTemplate not found");
            return;
        }

        orderTemplate.SetStatus(isOrderCompleted);
    }

    public void Open()
    {
        if (_isOpen) return;
        _isOpen = true;
        _slideAnimation.Transition(_isOpen);
    }

    public void Close() { if (_isOpen) { StartCoroutine(CloseRoutine()); } }

    private IEnumerator CloseRoutine()
    {
        yield return _slideAnimation.TransitionRoutine(!_isOpen);
        _isOpen = false;
    }

    private void Clear()
    {
        if (_activeOrders != null && _activeOrders.Count > 0)
            foreach (GameObject order in _activeOrders.Values)
                Destroy(order);
        _activeOrders = new Dictionary<Guest, GameObject>();
    }
}
