using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

public class CoinsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _addCoinsText;
    [SerializeField] private int _stepAmount;
    [SerializeField] private float _stepTime;

    private PlayerWallet _playerWallet;
    private int _currentBalance;
    private int _targetBalance;
    private Coroutine _coinsUpdating;

    [Inject]
    public void Construct(PlayerWallet playerWallet)
    {
        _playerWallet = playerWallet;
    }

    private void Start()
    {
        _currentBalance = _playerWallet.Balance;
        SubscribeToEvents();
        UpdateCoinsAmout(_playerWallet.Balance);
    }

    private void SubscribeToEvents()
    {
        _playerWallet.OnBalanceChanged += UpdateCoinsAmout;
    }

    private void UpdateCoinsAmout(int newBalance)
    {
        if (_currentBalance == newBalance) return;

        if (_coinsUpdating != null)
        {
            _currentBalance = _targetBalance;
            _coinsText.text = $"{_targetBalance}";
            StopCoroutine(_coinsUpdating);
            _coinsUpdating = null;
        }

        _targetBalance = newBalance;
        _coinsUpdating = StartCoroutine(UpdateCoinsAmoutRoutine());
    }

    private IEnumerator UpdateCoinsAmoutRoutine()
    {
        var delta = _targetBalance - _currentBalance;
        var deltaAbs = Mathf.Abs(delta);

        if (delta > 0)
            _addCoinsText.text = $"+{deltaAbs}";
        else
            _addCoinsText.text = $"-{deltaAbs}";

        _addCoinsText.gameObject.SetActive(true);
        while (_currentBalance != _targetBalance)
        {
            _currentBalance += (int)(Mathf.Sign(delta) * _stepAmount);
            _coinsText.text = $"{_currentBalance}";
            yield return new WaitForSeconds(_stepTime);
        }
        _coinsUpdating = null;
        _addCoinsText.gameObject.SetActive(false);
        _coinsText.text = $"{_targetBalance}";
    }

    private void OnDisable()
    {
        _playerWallet.OnBalanceChanged -= UpdateCoinsAmout;
    }
}
