using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Wallet _playerWallet;
    [SerializeField] private TextMeshProUGUI _coinsText;

    private void Start()
    {
        SubscribeToEvents();
        UpdateCoinsAmout(_playerWallet.Balance);
    }

    private void SubscribeToEvents()
    {
        _playerWallet.OnBalanceChanged += UpdateCoinsAmout;
    }

    private void UpdateCoinsAmout(int balance)
    {
        _coinsText.text = $"{balance}";
    }
}
