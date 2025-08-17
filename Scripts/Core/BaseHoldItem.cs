using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseHoldItem : MonoBehaviour
{
    public virtual void Use(Collider2D target) { }

    public virtual void Discard()
    {
        Destroy(gameObject);
    }
}
