using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseIngredient : MonoBehaviour, IHoldItem
{
    public virtual void Use(float faceDirection)
    {
        return;
    }

    public virtual void Discard()
    {
        Destroy(gameObject);
    }
}
