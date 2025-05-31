using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Turret Buttons")]
    public List<TurretButton> turretButtons;
    [SerializeField] private TextMeshProUGUI playerCoins;

    private void OnEnable()
    {
        GameManager.Instance.OnCoinsChanged += OnCoinsChanged;
        OnCoinsChanged(GameManager.Instance.CurrentCoins);
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnCoinsChanged -= OnCoinsChanged;
    }

    private void OnCoinsChanged(int newCoinAmount)
    {
        UpdatePlayerCoins(newCoinAmount);
        UpdateTurretButtons(newCoinAmount);
    }

    private void UpdatePlayerCoins(int coins)
    {
        playerCoins.text = $"{coins}";
    }

    private void UpdateTurretButtons(int playerCoins)
    {
        foreach (var button in turretButtons)
        {
            button.UpdateButtonState(playerCoins);
        }
    }
}
