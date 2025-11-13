using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Zenject;

public class CookingStation : BaseItemHolder
{
    public bool IsCookingStop;
    [SerializeField] private int _maxHeldIngredient;
    [SerializeField] private Transform _dishHolder;
    [SerializeField] private IngredientData[] _unavailableIngredients;
    [SerializeField] private float _minCookingTime;
    [SerializeField] private float _maxCookingTime;
    [Header("Audio")]
    [SerializeField] private AudioClip _receiveSound;
    [SerializeField] private AudioClip _startCoockingSound;
    [SerializeField] private AudioClip _spawnDishSound;
    [SerializeField] private AudioClip _loseIngredientSound;

    private int _currentIngredientsMask;
    private int _index = 0;
    private Recipes _recipes;
    private CoockingStationVisual _stationVisual;
    private Coroutine _cooking;

    public override Transform ParentPoint => _dishHolder;

    public override int SortingOrderOffset
        => _stationVisual.SpriteRenderer.sortingOrder + 1;

    [Inject]
    public void Construct(Recipes recipes)
    {
        _recipes = recipes;
    }

    protected override void Awake()
    {
        base.Awake();
        IsCookingStop = false;
        _currentIngredientsMask = 0;
        _stationVisual = GetComponent<CoockingStationVisual>();
    }

    public override bool TryReceive(BaseHoldItem heldItem)
    {
        if (_cooking == null &&
            heldItem.TryGetComponent(out Ingredient ingredient) &&
            !_unavailableIngredients.Contains(ingredient.Data) &&
            !ContainsIngredient(ingredient))
        {
            SFX.Instance.PlayAudioClip(_receiveSound);
            heldItem.Discard();
            Cook(ingredient);
            return true;
        }
        return false;
    }

    public override BaseHoldItem GiveItem()
    {
        var heldItem = base.GiveItem();
        if (heldItem != null)
            Clear();
        return heldItem;
    }

    private void Cook(Ingredient ingredient)
    {
        if (_index >= _maxHeldIngredient)
            Clear();

        AddIngredient(ingredient);
        _stationVisual.ChangeColorTo(ingredient.Data.Color);

        if (_recipes.TryGetDish(_currentIngredientsMask, out GameObject dish))
        {
            _cooking = StartCoroutine(CookingDish(dish));
            _stationVisual.ClearIngredients();
        }
        else if (!CouldMakeDish())
        {
            SFX.Instance.PlayAudioClip(_loseIngredientSound);
            Clear();
        }
    }

    private bool CouldMakeDish()
    {
        foreach (var recipe in _recipes.RecipesDic)
        {
            var recipeMask = recipe.Key;
            if ((_currentIngredientsMask & recipeMask) == _currentIngredientsMask)
                return true;
        }
        return false;
    }

    private IEnumerator CookingDish(GameObject dish)
    {
        SFX.Instance.PlayAudioClip(_startCoockingSound);
        float duration = UnityEngine.Random.Range(_minCookingTime, _maxCookingTime);
        float elapsed = 0;
        while (elapsed < duration)
        {
            if (!IsCookingStop)
                elapsed += Time.deltaTime;

            _stationVisual.UpdateClockAnimation(elapsed / duration);
            yield return null;
        }
        _stationVisual.UpdateClockAnimation(1);
        SpawnDish(dish);
    }

    private void SpawnDish(GameObject dish)
    {
        SFX.Instance.PlayAudioClip(_spawnDishSound);
        BaseHoldItem holdItem = Instantiate(
            dish, transform.position, transform.rotation)
            .GetComponent<BaseHoldItem>();
        base.TryReceive(holdItem);
    }

    private void AddIngredient(Ingredient ingredient)
    {
        _currentIngredientsMask |= (int)ingredient.Data.IngredientType;
        _stationVisual.AddIngredient(ingredient.Data.Icon);
        _index++;
    }

    private bool ContainsIngredient(Ingredient ingredient)
    {
        return (_currentIngredientsMask & (int)ingredient.Data.IngredientType) != 0;
    }

    private void Clear()
    {
        _currentIngredientsMask = 0;
        _index = 0;
        _cooking = null;
        _stationVisual.ClearIngredients();
    }
}
