using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Shop : MonoBehaviour
{
    public bool IsOpen { get; private set; }

    [SerializeField] private ShopItemData[] Assortment;
    [SerializeField] private GameObject _shopItemPrefab;

    [SerializeField] private Vector3 _startPos = new Vector3(-340f, 180f, 0f);
    [SerializeField] private int _colNumber = 3;
    [SerializeField] private float _widthOffset = 340f;
    [SerializeField] private float _heightOffset = 140f;

    private Seller _seller;
    private PlayerWallet _playerWallet;
    private ShopServiceFactory _shopServiceFactory;
    private Transform _container;
    private List<GameObject> _purchasedItems;
    private SlideAnimation _slideAnimation;

    [Inject]
    public void Construct(PlayerWallet playerWallet, Seller seller,
        ShopServiceFactory shopServiceFactory)
    {
        _shopServiceFactory = shopServiceFactory;
        _playerWallet = playerWallet;
        _seller = seller;
        IsOpen = false;
    }

    private void Awake()
    {
        _container = transform.Find("Container").gameObject.transform;
        _slideAnimation = GetComponent<SlideAnimation>();
        InitShop();
    }

    private void InitShop()
    {
        for (int i = 0; i < Assortment.Length; i++)
        {
            var data = Assortment[i];
            var item = Instantiate(_shopItemPrefab, _container);
            item.GetComponent<ShopItemUI>().Init(data, this);

            Vector3 pos = GetGridPosition(i);
            item.transform.localPosition = pos;
        }
    }

    private Vector3 GetGridPosition(int index)
    {
        int col = index % _colNumber;
        int row = index / _colNumber;

        return new Vector3(_startPos.x + (col * _widthOffset),
            _startPos.y - (row * _heightOffset), 0);
    }

    public void TryBuy(ShopItemUI shopItemUI)
    {
        ShopItemData itemData = shopItemUI.Data;
        if (!_playerWallet.TryRemove(itemData.Price))
            return;

        if (itemData.ServiceType != ShopServiceType.None)
        {
            var service = _shopServiceFactory.Create(itemData.ServiceType);
            if (service == null || !service.TryApply())
                shopItemUI.BlockItem();
            return;
        }
        _purchasedItems.Add(itemData.ItemPrefab);
    }

    public void OpenShop()
    {
        IsOpen = true;
        _purchasedItems = new List<GameObject>();
        _slideAnimation.Transition(IsOpen);
    }

    public void CloseShop()
    {
        IsOpen = false;
        _seller.DeliverItems(_purchasedItems);
        _slideAnimation.Transition(IsOpen);
    }
}
