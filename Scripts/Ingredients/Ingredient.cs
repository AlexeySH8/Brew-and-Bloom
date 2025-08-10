using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour, IHoldItem
{
    public virtual void Use(Collider2D target)
    {
        return;
    }

    public virtual void Discard()
    {
        Destroy(gameObject);
    }
}
