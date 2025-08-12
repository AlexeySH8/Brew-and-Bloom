using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Recipes
{
    public static readonly Dictionary<int, GameObject> _recipes = new();

    public enum Ingredient
    {
        Carrot = 1 << 0,
        Potato = 1 << 1,
        Mushroom = 1 << 2,
        Chamomile = 1 << 3,
        Onion = 1 << 4,
        Turnip = 1 << 5,
        Radish = 1 << 6,
        Spinach = 1 << 7,
        Sunflower = 1 << 8,
        Rose = 1 << 9,
        Coral = 1 << 10,
        Powder = 1 << 11
    }
}
