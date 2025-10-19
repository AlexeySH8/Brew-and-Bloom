using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public ShopItemData Data { get; private set; }

    [SerializeField] private Image _itemImage;
    [SerializeField] private Image _coinImage;
    [SerializeField] private TextMeshProUGUI _priceText;

    private Button _buyButton;
    private Shop _shop;

    public void Init(ShopItemData data, Shop shop)
    {
        Data = data;
        _shop = shop;
        _itemImage.sprite = Data.Icon;
        _priceText.text = Data.Price.ToString();
        _buyButton = GetComponent<Button>();
        _buyButton.onClick.AddListener(Buy);
    }

    public void Buy()
    {
        _shop.TryBuy(this);
    }

    public void BlockItem()
    {
        Color colorItem = _itemImage.color;
        colorItem.a = 0.5f;
        _itemImage.color = colorItem;

        Color colorCoin = _coinImage.color;
        colorCoin.a = 0.5f;
        _coinImage.color = colorCoin;

        Color colorText = _priceText.color;
        colorText.a = 0.5f;
        _priceText.color = colorText;

        _buyButton.interactable = false;
    }
}
