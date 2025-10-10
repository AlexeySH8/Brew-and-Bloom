using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer {  get; private set; }

    private PlayerController _playerController;
    private Animator _animator;

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _playerController = GetComponentInParent<PlayerController>();
    }

    public void UpdateVisual(float horizontalInput, float verticalInput)
    {
        var isRunning = horizontalInput != 0 || verticalInput != 0;
        UpdateAnimation(isRunning);
    }

    private void UpdateAnimation(bool isRunning)
    {
        _animator.SetBool("IsRunning", isRunning);
    }

    public void FlipVisual()
    {
        transform.localScale = new Vector2(_playerController.FaceDirection, transform.localScale.y);
    }
}
