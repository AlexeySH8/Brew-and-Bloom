using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [field: SerializeField] public int Balance { get; private set; }

    public event Action<int> OnBalanceChanged;

    public void Add(int amount)
    {
        Balance += amount;
        OnBalanceChanged?.Invoke(Balance);
    }

    public bool Remove(int amount)
    {
        if (Balance < amount) return false;
        Balance -= amount;
        OnBalanceChanged?.Invoke(Balance);
        return true;
    }
}
