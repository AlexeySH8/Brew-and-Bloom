using UnityEngine;

public interface IHoldItem
{
    void Use(Collider2D target);
    void Discard();
}

