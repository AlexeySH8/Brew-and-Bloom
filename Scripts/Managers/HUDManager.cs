using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;
    private Wallet _playerWallet;

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        _playerWallet = player.GetComponent<PlayerController>().Wallet;
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
