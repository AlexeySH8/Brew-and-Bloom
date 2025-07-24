using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void UpdateVisual(float horizontalInput, float verticalInput)
    {
        var isRunning = horizontalInput != 0 || verticalInput != 0;
        UpdateAnimation(isRunning);
        UpdateFaceDirection(horizontalInput);
    }

    private void UpdateAnimation(bool isRunning)
    {
        _animator.SetBool("IsRunning", isRunning);
    }

    private void UpdateFaceDirection(float horizontalInput)
    {
        if (horizontalInput == 0) return;

        float faceDirection = Mathf.Sign(horizontalInput);
        transform.localScale = new Vector2(faceDirection, 1);
    }
}
