using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    public Vector2 InteractionDirection { get; private set; }
    public float FaceDirection { get; private set; } = 1;

    private IPlayerInput _input;
    private PlayerMovement _movement;
    private PlayerVisual _visual;
    private PlayerInteractionHandler _interactiveHandler;
    private PlayerItemHolder _itemHolder;
    private IActiveInteraction _interactionPartner;
    private GameSceneManager _gameSceneManager;
    private float _horizontalInput;
    private float _verticalInput;
    private bool _canMove = true;
    private bool _canDrop = true;
    private bool _canInteract = true;

    [Inject]
    public void Construct(IPlayerInput input, GameSceneManager gameSceneManager)
    {
        _input = input;
        _gameSceneManager = gameSceneManager;
    }

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _visual = GetComponentInChildren<PlayerVisual>();
        _interactiveHandler = GetComponent<PlayerInteractionHandler>();
        _itemHolder = GetComponent<PlayerItemHolder>();
    }

    private void Start()
    {
        _gameSceneManager.OnHouseUnloading += DropHeldItem;
    }

    private void Update()
    {
        HandleInput();
        UpdateDirections();
        _visual.UpdateVisual(_horizontalInput, _verticalInput);
        HandleInteractInput();
    }

    private void FixedUpdate()
    {
        if (_canMove)
            _movement.Move(_horizontalInput, _verticalInput);
    }

    private void HandleInput()
    {
        if (_canMove)
        {
            _horizontalInput = _input.GetHorizontal();
            _verticalInput = _input.GetVertical();
        }
    }

    private void UpdateDirections()
    {
        var direction = new Vector2(_horizontalInput, _verticalInput);

        if (direction.x != 0)
            FaceDirection = Mathf.Sign(direction.x);

        if (direction != Vector2.zero)
            InteractionDirection = direction.normalized;
    }

    private void HandleInteractInput()
    {
        if (_canInteract && _input.IsInteractPressed())
        {
            if (_interactionPartner != null)
                _interactionPartner.HandleInteractPressed();
            else
                _interactiveHandler.Interact(FaceDirection);
        }

        if (_canDrop && _input.IsDropPressed())
            DropHeldItem();
    }

    private void DropHeldItem() => _itemHolder.Drop(InteractionDirection.normalized);

    public void StartActiveInteraction(IActiveInteraction activeInteraction)
    {
        _interactionPartner = activeInteraction;
        _canMove = false;
        _canDrop = false;

        _horizontalInput = 0;
        _verticalInput = 0;
    }

    public void EndActiveInteraction()
    {
        _interactionPartner = null;
        _canMove = true;
        _canDrop = true;
    }

    private void OnDisable()
    {
        _gameSceneManager.OnHouseUnloading -= DropHeldItem;
    }
}
