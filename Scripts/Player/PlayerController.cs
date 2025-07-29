using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 InteractionDirection { get; private set; }
    public float FaceDirection { get; private set; }
    private IPlayerInput _input;
    private PlayerMovement _movement;
    private PlayerVisual _visual;
    private InteractionHandler _interactiveHandler;
    private ItemHolder _itemHolder;
    private float _horizontalInput;
    private float _verticalInput;
    private float _xInteractionDirection;
    private float _yInteractionDirection;

    private void Awake()
    {
        _input = new PCInput();
        _movement = GetComponent<PlayerMovement>();
        _visual = GetComponentInChildren<PlayerVisual>();
        _interactiveHandler = GetComponent<InteractionHandler>();
        _itemHolder = GetComponent<ItemHolder>();
        FaceDirection = 1;
        _xInteractionDirection = 1;
        _yInteractionDirection = 0;
    }

    private void Update()
    {
        _horizontalInput = _input.GetHorizontal();
        _verticalInput = _input.GetVertical();
        UpdateDirections();

        _visual.UpdateVisual(_horizontalInput, _verticalInput);
        _visual.FlipVisual();

        if (_input.IsInteractPressed())
            _interactiveHandler.Interact();

        if (_input.IsDropPressed())
            _itemHolder.Drop(InteractionDirection.normalized);
    }

    private void FixedUpdate()
    {
        _movement.Move(_horizontalInput, _verticalInput);
    }

    private void UpdateDirections()
    {
        if (_horizontalInput != 0)
        {
            _xInteractionDirection = _horizontalInput;
            FaceDirection = _horizontalInput;
        }
        else if (_verticalInput != 0)
            _xInteractionDirection = 0;

        if (_verticalInput != 0)
            _yInteractionDirection = _verticalInput;
        else if (_horizontalInput != 0)
            _yInteractionDirection = 0;

        InteractionDirection = new Vector2(_xInteractionDirection, _yInteractionDirection);
    }
}
