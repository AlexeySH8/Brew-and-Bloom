using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseHoldItem : MonoBehaviour, IGiveHeldItem
{
    protected Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public GameObject Give() => _rb.transform.gameObject;

    public virtual void Use(Collider2D target) { }

    public virtual void Discard()
    {
        Destroy(gameObject);
    }
}
