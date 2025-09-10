using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Mesh;

public class OrderTemplate : MonoBehaviour
{
    public bool HasOrder { get; private set; }

    [SerializeField] private Image[] _ingreientsTemplate;
    [SerializeField] private Image _dishTemplate;
    [SerializeField] private Sprite _defaultSprite;

    public void DisplayOrder(DishData dishData)
    {
        HasOrder = true;
        if (dishData.Ingredients.Length > _ingreientsTemplate.Length)
        {
            Debug.LogError($"There are too many ingredients in the {dishData.name}");
            return;
        }

        for (int i = 0; i < dishData.Ingredients.Length; i++)
        {
            IngredientData ingredientData = dishData.Ingredients[i];
            _ingreientsTemplate[i].sprite = ingredientData.Icon;
        }
        _dishTemplate.sprite = dishData.Icon;
    }

    public void ClearVisual()
    {
        HasOrder = false;

        for (int i = 0; i < _ingreientsTemplate.Length; i++)
            _ingreientsTemplate[i].sprite = _defaultSprite;

        _dishTemplate.sprite = _defaultSprite;
    }
}
