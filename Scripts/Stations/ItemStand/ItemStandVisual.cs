using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStandVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] _ingredientsVisual;
    [SerializeField] private Sprite _defaultSprite;

    private ItemStand _itemStand;
    private SpriteRenderer _renderer;

    public int SortingOrderOffset => _renderer.sortingOrder + 1;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _itemStand = GetComponent<ItemStand>();
    }

    private void Start()
    {
        if (_ingredientsVisual.Length != _itemStand.MaxPlaceCount)
        {
            Debug.LogError($"The number of IngredientsVisual must be equal to the number of MaxPlaceCount");
        }
    }

    public void UpdateVisual(int currentPlace, Sprite ingredientIcon)
    {
        for (int i = 0; i < _ingredientsVisual.Length; i++)
        {
            if (i < currentPlace)
                _ingredientsVisual[i].sprite = ingredientIcon;
            else
                _ingredientsVisual[i].sprite = _defaultSprite;
        }
    }
}
