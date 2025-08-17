using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : BaseHoldItem
{
    [field: SerializeField] public SeedData Data { get; private set; }
}
