using System;

public class PlayerWallet : IDataPersistence
{
    public int Balance { get; private set; }
    public int DailyEarning { get; private set; }

    public event Action<int> OnBalanceChanged;
    public event Action<int> OnDailyEarningChanged;

    private const int _startingBalance = 999;
    private const int MaxBalance = 999;
    private GameSceneManager _gameSceneManager;
    private IDataPersistenceManager _dataPresistenceManager;

    public PlayerWallet(GameSceneManager gameSceneManager,
        IDataPersistenceManager dataPresistenceManager)
    {
        _gameSceneManager = gameSceneManager;
        _dataPresistenceManager = dataPresistenceManager;
        _dataPresistenceManager.Register(this);
        Balance = _startingBalance;
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        _gameSceneManager.OnHouseLoaded += StartNewDay;
    }

    public void AddToBalance(int amount)
    {
        if (amount <= 0) return;
        Balance = Math.Min(MaxBalance, Balance + amount);
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

    public void LoadData(GameData gameData)
    {
        Balance = gameData.Balance;
        DailyEarning = gameData.DailyEarning;
        OnBalanceChanged?.Invoke(Balance);
    }

    public void SaveData(GameData gameData)
    {
        gameData.Balance = Balance;
        gameData.DailyEarning = DailyEarning;
    }
}
