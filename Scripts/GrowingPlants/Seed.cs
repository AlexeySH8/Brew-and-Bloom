using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour, IHoldItem
{
    [field: SerializeField] public SeedData Data { get; private set; }

    public void Use(Collider2D target)
    {
        return;
    }

    public void Discard()
    {
        Destroy(gameObject);
    }
}
