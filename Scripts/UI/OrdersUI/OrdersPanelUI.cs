using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class OrdersPanelUI : MonoBehaviour
{
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _orderTemplatePref;
    [SerializeField] private TextMeshProUGUI _moneyEarnedText;
    [SerializeField] private Button _closeButton;

    private Dictionary<Guest, GameObject> _activeOrders = new();
    private SlideAnimation _slideAnimation;
    private GuestsManager _guestsManager;
    private PlayerWallet _playerWallet;
    private bool _isOpen;

    [Inject]
    public void Construct(GuestsManager guestsManager, PlayerWallet playerWallet)
    {
        _guestsManager = guestsManager;
        _playerWallet = playerWallet;
        _isOpen = false;
        SubscribeToEvents();
    }

    private void Awake()
    {
        _slideAnimation = GetComponent<SlideAnimation>();
        _closeButton.onClick.AddListener(SFX.Instance.PlayClickButtonClose);
    }

    private void Start()
    {
        AddOrders(_guestsManager.GuestForDay);
        UpdateMoneyEarnedText(_playerWallet.DailyEarning);
    }

    private void SubscribeToEvents()
    {
        _playerWallet.OnDailyEarningChanged += UpdateMoneyEarnedText;
    }

    private void OnDisable()
    {
        _playerWallet.OnDailyEarningChanged -= UpdateMoneyEarnedText;
    }

    public void AddOrders(IReadOnlyList<Guest> guests)
    {
        //Clear();
        foreach (Guest guest in guests)
        {
            Order order = guest.CurrentOrder;
            GameObject orderTemplate = Instantiate(_orderTemplatePref, _content);
            _activeOrders.Add(order.Guest, orderTemplate);
            orderTemplate
                .GetComponent<OrderTemplate>()
                .DisplayOrder(order);

            if (guest.IsServed)
                UpdateStatusOrder(guest, guest.CurrentOrder.IsCompleted);
            else
                guest.OnOrderCompleted += UpdateStatusOrder;
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
        _closeButton.gameObject.SetActive(true);
        _slideAnimation.Transition(_isOpen);
    }

    public void Close()
    {
        if (_isOpen)
        {
            _isOpen = false;
            _closeButton.gameObject.SetActive(false);
            _slideAnimation.Transition(_isOpen);
        }
    }

    private void Clear()
    {
        if (_activeOrders.Count == 0) return;

        foreach (Guest guest in _activeOrders.Keys)
        {
            var orderPref = _activeOrders[guest];
            guest.OnOrderCompleted -= UpdateStatusOrder;
            Destroy(orderPref);
        }
        _activeOrders.Clear();
    }

    private void OnDestroy()
    {
        Clear();
    }
}
