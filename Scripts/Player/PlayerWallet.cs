using System;

public class PlayerWallet
{
    public int Balance { get; private set; }
    public int DailyEarning { get; private set; }

    public event Action<int> OnBalanceChanged;
    public event Action<int> OnDailyEarningChanged;

    private const int MaxBalance = 999;
    private GameSceneManager _gameSceneManager;

    public PlayerWallet(GameSceneManager gameSceneManager, int startingBalance)
    {
        Balance = startingBalance;
        _gameSceneManager = gameSceneManager;
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        _gameSceneManager.OnHouseLoaded += StartNewDay;
    }

    public void AddToBalance(int amount)
    {
        if (amount <= 0) return;
        Balance = (int)MathF.Min(MaxBalance, Balance + amount);
        OnBalanceChanged?.Invoke(Balance);
    }

    public void AddToDailyEarning(int amount)
    {
        if (amount <= 0) return;
        DailyEarning += amount;
        OnDailyEarningChanged?.Invoke(DailyEarning);
    }

    private void StartNewDay()
    {
        AddToBalance(DailyEarning);
        DailyEarning = 0;
    }

    public bool TryRemove(int amount)
    {
        if (Balance < amount) return false;
        Balance -= amount;
        OnBalanceChanged?.Invoke(Balance);
        return true;
    }
}
