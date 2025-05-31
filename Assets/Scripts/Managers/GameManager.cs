using System;
using DesignPatterns.Generics;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Monete del giocatore")]
    [SerializeField] private int startingCoins = 100;
    private int currentCoins;

    public int CurrentCoins => currentCoins;

    
    public event Action<int> OnCoinsChanged;

    public override void Awake()
    {
        base.Awake();
        currentCoins = startingCoins;
        OnCoinsChanged?.Invoke(currentCoins);  
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        Debug.Log($"Aggiunti {amount} coins. Coins totali: {currentCoins}");
        OnCoinsChanged?.Invoke(currentCoins);  
    }

    public bool SpendCoins(int amount)
    {
        if (currentCoins >= amount)
        {
            currentCoins -= amount;
            Debug.Log($"Spesi {amount} coins. Coins rimasti: {currentCoins}");
            OnCoinsChanged?.Invoke(currentCoins);  
            return true;
        }
        else
        {
            Debug.LogWarning("Non hai abbastanza monete!");
            OnCoinsChanged?.Invoke(currentCoins);  
            return false;
        }
    }
}
