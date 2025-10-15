using TMPro;
using UnityEngine;
using Zenject;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;

    private PlayerWallet _playerWallet;

    [Inject]
    public void Construct(PlayerWallet playerWallet)
    {
        _playerWallet = playerWallet;
    }

    private void Start()
    {
        SubscribeToEvents();
        UpdateCoinsAmout(_playerWallet.Balance);
    }

    private void SubscribeToEvents()
    {
        _playerWallet.OnBalanceChanged += UpdateCoinsAmout;
    }

    private void OnDisable()
    {
        _playerWallet.OnBalanceChanged -= UpdateCoinsAmout;
    }

    private void UpdateCoinsAmout(int balance)
    {
        _coinsText.text = $"{balance}";
    }
}
