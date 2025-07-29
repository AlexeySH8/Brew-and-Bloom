using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 FaceDirection { get; private set; }
    private IPlayerInput _input;
    private PlayerMovement _movement;
    private PlayerVisual _visual;
    private ItemHandler _itemHandler;
    private float _horizontalInput;
    private float _verticalInput;
    private float _xFaceDirection;
    private float _yFaceDirection;

    private void Awake()
    {
        _input = new PCInput();
        _movement = GetComponent<PlayerMovement>();
        _visual = GetComponentInChildren<PlayerVisual>();
        _itemHandler = GetComponent<ItemHandler>();
        _xFaceDirection = 1;
        _yFaceDirection = 0;
    }

    private void Update()
    {
        _horizontalInput = _input.GetHorizontal();
        _verticalInput = _input.GetVertical();
        UpdateFaceDirection();

        _visual.UpdateVisual(_horizontalInput, _verticalInput);
        _visual.FlipVisual();

        if (_input.IsInteractPressed())
            _itemHandler.HandleItem();

        if (_input.IsDropPressed())
            _itemHandler.Drop();
    }

    private void FixedUpdate()
    {
        _movement.Move(_horizontalInput, _verticalInput);
    }

    private void UpdateFaceDirection()
    {
        if (_horizontalInput != 0)
            _xFaceDirection = _horizontalInput;
        else if (_verticalInput != 0)
            _xFaceDirection = 0;

        if (_verticalInput != 0)
            _yFaceDirection = _verticalInput;
        else if (_horizontalInput != 0)
            _yFaceDirection = 0;

        FaceDirection = new Vector2(_xFaceDirection, _yFaceDirection);
    }
}
