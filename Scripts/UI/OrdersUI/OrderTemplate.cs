using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OrderTemplate : MonoBehaviour
{
    [SerializeField] private Image[] _ingredientsTemplate;
    [SerializeField] private Image _dishTemplate;
    [SerializeField] private Sprite _defaultSprite;

    public void DisplayOrder(DishData dishData)
    {
        ClearVisual();
        StopAllCoroutines();
        StartCoroutine(DisplayOrderRoutine(dishData));
    }

    private IEnumerator DisplayOrderRoutine(DishData dishData)
    {
        if (dishData.Ingredients.Length > _ingredientsTemplate.Length)
        {
            Debug.LogError($"There are too many ingredients in the {dishData.name}");
            yield break;
        }

        for (int i = 0; i < dishData.Ingredients.Length; i++)
        {
            IngredientData ingredientData = dishData.Ingredients[i];
            _ingredientsTemplate[i].sprite = ingredientData.Icon;
            yield return new WaitForSeconds(0.08f);
        }
        _dishTemplate.sprite = dishData.Icon;
    }

    public void ClearVisual()
    {
        for (int i = 0; i < _ingredientsTemplate.Length; i++)
            _ingredientsTemplate[i].sprite = _defaultSprite;

        _dishTemplate.sprite = _defaultSprite;
    }
}
