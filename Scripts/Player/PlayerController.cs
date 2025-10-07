using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    public Vector2 InteractionDirection { get; private set; }
    public float FaceDirection { get; private set; }

    [SerializeField] private bool _canMove;
    [SerializeField] private bool _canDrop;
    [SerializeField] private bool _canInteract;

    private IPlayerInput _input;
    private PlayerMovement _movement;
    private PlayerWallet _wallet;
    private PlayerVisual _visual;
    private InteractionHandler _interactiveHandler;
    private ItemHolder _itemHolder;
    private GuestDialogue _dialoguePartner;
    private float _horizontalInput;
    private float _verticalInput;
    private float _xInteractionDirection;
    private float _yInteractionDirection;

    [Inject]
    public void Construct(IPlayerInput input , PlayerWallet playerWallet)
    {
        _input = input;
        _wallet = playerWallet;
    }

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _visual = GetComponentInChildren<PlayerVisual>();
        _interactiveHandler = GetComponent<InteractionHandler>();
        _itemHolder = GetComponent<ItemHolder>();
        FaceDirection = 1;
        _xInteractionDirection = 1;
        _yInteractionDirection = 0;
        _canMove = true;
        _canDrop = true;
        _canInteract = true;
    }

    private void Update()
    {
        if (_canMove)
        {
            _horizontalInput = _input.GetHorizontal();
            _verticalInput = _input.GetVertical();
        }

        UpdateDirections();

        _visual.UpdateVisual(_horizontalInput, _verticalInput);
        _visual.FlipVisual();

        if (_canInteract && _input.IsInteractPressed())
        {
            if (_dialoguePartner != null)
                _dialoguePartner.NextLineDialogue();
            else
                _interactiveHandler.Interact(FaceDirection);
        }

        if (_canDrop && _input.IsDropPressed())
            _itemHolder.Drop(InteractionDirection.normalized);
    }

    private void FixedUpdate()
    {
        if (_canMove)
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

    public void StartDialogue(GuestDialogue guestDialogue)
    {
        _dialoguePartner = guestDialogue;
        _canMove = false;
        _canDrop = false;

        _horizontalInput = 0;
        _verticalInput = 0;
    }

    public void EndDialogue()
    {
        _dialoguePartner = null;
        _canMove = true;
        _canDrop = true;
    }

    private void OnDestroy()
    {
        _wallet.Dispose();
    }
}
