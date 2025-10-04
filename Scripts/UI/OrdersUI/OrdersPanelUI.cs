using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrdersPanelUI : MonoBehaviour
{
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _orderTemplatePref;
    [SerializeField] private TextMeshProUGUI _moneyEarnedText;

    private Dictionary<Guest, GameObject> _activeOrders;
    private SlideAnimation _slideAnimation;
    private bool _isOpen;
    private Wallet _playerWallet;

    private void Awake()
    {
        _isOpen = false;
        _slideAnimation = GetComponent<SlideAnimation>();
    }

    private void Start()
    {
        _playerWallet = FindAnyObjectByType<PlayerController>().Wallet;
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        GuestsManager.Instance.OnGuestsArrived += AddOrders;
        _playerWallet.OnDailyEarningChanged += UpdateMoneyEarnedText;
    }

    private void OnDisable()
    {
        GuestsManager.Instance.OnGuestsArrived -= AddOrders;
        _playerWallet.OnDailyEarningChanged -= UpdateMoneyEarnedText;
    }

    public void AddOrders(IReadOnlyList<Guest> guests)
    {
        Clear();
        foreach (Guest guest in guests)
        {
            guest.OnOrderCompleted += UpdateStatusOrder;
            Order order = guest.CurrentOrder;
            GameObject orderTemplate = Instantiate(_orderTemplatePref, _content);
            _activeOrders.Add(order.Guest, orderTemplate);
            orderTemplate
                .GetComponent<OrderTemplate>()
                .DisplayOrder(order);
        }
    }

    public void UpdateStatusOrder(Guest guest, bool isOrderCompleted)
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

    public void UpdateMoneyEarnedText(int currentAmount)
    {
        _moneyEarnedText.text = currentAmount.ToString();
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
        {
            foreach (Guest guest in _activeOrders.Keys)
            {
                var orderPref = _activeOrders[guest];
                guest.OnOrderCompleted -= UpdateStatusOrder;
                Destroy(orderPref);
            }
        }
        _activeOrders = new Dictionary<Guest, GameObject>();
    }
}
