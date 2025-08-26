using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public ShopItemData Data { get; private set; }
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _priceText;

    private Shop _shop;

    public void Init(ShopItemData data, Shop shop)
    {
        Data = data;
        _shop = shop;
        _itemImage.sprite = Data.Item.GetComponent<SpriteRenderer>().sprite;
        _priceText.text = Data.Price.ToString();
        GetComponent<Button>().onClick.AddListener(Buy);
    }

    public void Buy()
    {
        _shop.TryBuy(Data);
    }
}
