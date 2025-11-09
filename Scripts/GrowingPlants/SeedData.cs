using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSeedData", menuName = "Farming/Seed Data")]
public class SeedData : ScriptableObject
{
    [field: SerializeField] public float MinStageTime { get; private set; } = 1f;
    [field: SerializeField] public float MaxStageTime { get; private set; } = 1f;
    [field: SerializeField] public int MinHarvestCount { get; private set; } = 1;
    [field: SerializeField] public int MaxHarvestCount { get; private set; } = 1;
    [field: SerializeField] public List<Sprite> GrowthStageSprites { get; private set; }
    [field: SerializeField] public GameObject IngredientPrefab { get; private set; }
}
