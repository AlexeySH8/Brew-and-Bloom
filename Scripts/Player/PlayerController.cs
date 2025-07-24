using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IPlayerInput _input;
    private PlayerMovement _movement;
    private PlayerVisual _visual;
    private float _horizontalInput;
    private float _verticalInput;

    private void Awake()
    {
        _input = new PCInput();
        _movement = GetComponent<PlayerMovement>();
        _visual = GetComponentInChildren<PlayerVisual>();
    }

    private void Update()
    {
        _horizontalInput = _input.GetHorizontal();
        _verticalInput = _input.GetVertical();
        _visual.UpdateVisual(_horizontalInput, _verticalInput);
    }

    private void FixedUpdate()
    {
        _movement.Move(_horizontalInput, _verticalInput);
    }
}
