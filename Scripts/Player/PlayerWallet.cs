using System;

public class PlayerWallet
{
    public event Action<int> OnBalanceChanged;
    public event Action<int> OnDailyEarningChanged;

    private PlayerData _playerData;
    private DayManager _dayManager;

    public int Balance => _playerData.Balance;

    public int DailyEarning => _playerData.DailyEarning;

    public PlayerWallet(PlayerData playerData , DayManager dayManager)
    {
        _playerData = playerData;
        _dayManager = dayManager;   
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        _dayManager.OnStartDay += StartNewDay;
    }

    public void Dispose()
    {
        _dayManager.OnStartDay -= StartNewDay;
    }

    public void AddToBalance(int amount)
    {
        if (amount <= 0) return;
        _playerData.Balance += amount;
        OnBalanceChanged?.Invoke(_playerData.Balance);
    }

    public void AddToDailyEarning(int amount)
    {
        if (amount <= 0) return;
        _playerData.DailyEarning += amount;
        OnDailyEarningChanged?.Invoke(_playerData.DailyEarning);
    }

    private void StartNewDay()
    {
        AddToBalance(_playerData.DailyEarning);
        _playerData.DailyEarning = 0;
    }

    public bool Remove(int amount)
    {
        if (_playerData.Balance < amount) return false;
        _playerData.Balance -= amount;
        OnBalanceChanged?.Invoke(_playerData.Balance);
        return true;
    }
}
