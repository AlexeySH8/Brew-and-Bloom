using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Recipes _recipes;

    private void Awake()
    {
        _recipes.InitializeDishesDictionary();
    }
}
