using UnityEngine;
using Zenject;

public class CursesBook : BaseItemHolder, IActiveInteraction
{
    [SerializeField] private TimingBar _timmingBar;
    [SerializeField] private Transform _parentPoint;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private int _attemptsCount = 3;
    [SerializeField] private int _requiredSuccessCount = 3;
    [SerializeField] private Vector3 _spawnOffset;

    private int _currentSuccessStreak;
    private int _currentAttempt = 0;
    private bool _isSuccess;
    private PlayerController _playerController;

    public override Transform ParentPoint => _parentPoint;

    public override int SortingOrderOffset => _spriteRenderer.sortingOrder + 1;

    [Inject]
    public void Construct(PlayerController playerController)
    {
        _playerController = playerController;
    }

    public void HandleInteractPressed()
    {
        if (!_timmingBar.CheckSuccess())
        {
            _isSuccess = false;
            EndActiveInteraction();
        }
        else
        {
            _currentSuccessStreak++;
            if (_currentSuccessStreak == _requiredSuccessCount)
            {
                _isSuccess = true;
                EndActiveInteraction();
            }
        }
    }

    private void StartActiveInteraction()
    {
        _isSuccess = false;
        _currentSuccessStreak = 0;
        _timmingBar.StartPlay();
        _playerController.StartActiveInteraction(this);
    }

    public void EndActiveInteraction()
    {
        _currentAttempt++;
        HandleResult();
        _timmingBar.EndPlay();
        _playerController.EndActiveInteraction();
    }

    private void HandleResult()
    {
        if (_isSuccess)
            SpawnItems();
        else
            _heldItem.Discard();
    }

    private void SpawnItems()
    {
        BaseHoldItem item = GiveItem();
        BaseHoldItem newItem = Instantiate(item);
        var rightSpawn = _spawnOffset;
        var leftSpawn = new Vector3(-_spawnOffset.x, _spawnOffset.y, _spawnOffset.z);
        item.ArcAnimation.Animate(transform.position, leftSpawn, 2);
        newItem.ArcAnimation.Animate(transform.position, rightSpawn, 2);
    }

    public override bool TryReceive(BaseHoldItem heldItem)
    {
        if (_currentAttempt < _attemptsCount)
        {
            bool result = base.TryReceive(heldItem);
            if (result)
                StartActiveInteraction();
            return result;
        }
        return false;
    }
}
