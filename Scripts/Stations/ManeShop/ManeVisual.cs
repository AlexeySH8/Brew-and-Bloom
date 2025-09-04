using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManeVisual : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FlipVisual(float direction)
    {
        _spriteRenderer.flipX = direction < 0;  
    }
}
