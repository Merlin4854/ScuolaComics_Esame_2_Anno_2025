using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerInfoUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject turretInfoPanel;

    public TextMeshProUGUI costText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI fireRateText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI upgradeCostText;

    public Button upgradeButton;
    public Button sellButton;

    public Image rangeCircleImage;

    private TurretController selectedTurret;

    private void Start()
    {
        turretInfoPanel.SetActive(false);
        upgradeButton.onClick.AddListener(OnUpgradeClicked);
        sellButton.onClick.AddListener(OnSellClicked);
    }

    public void SelectTurret(TurretController turret)
    {
        selectedTurret = turret;

        if (selectedTurret == null) return;

        turretInfoPanel.SetActive(true);
        UpdateUI();
        UpdateRangeCircle();
    }

    public void ClearUI()
    {
        selectedTurret = null;
        turretInfoPanel.SetActive(false);
        rangeCircleImage.gameObject.SetActive(false);
    }

    private void UpdateUI()
    {
        if (selectedTurret == null) return;

        costText.text = $"Cost: {selectedTurret.Cost}";
        damageText.text = $"Damage: {selectedTurret.DamageAmount:F1}";
        fireRateText.text = $"Fire Rate: {selectedTurret.FireRate:F2}";
        levelText.text = $"Level: {selectedTurret.Level}";
        upgradeCostText.text = $"Upgrade Cost: {selectedTurret.UpgradeCost}";

        upgradeButton.interactable = GameManager.Instance.CurrentCoins >= selectedTurret.UpgradeCost;
    }

    private void UpdateRangeCircle()
    {
        if (selectedTurret == null) return;

        rangeCircleImage.transform.position = selectedTurret.transform.position;

        float diameter = selectedTurret.Range * 2f;
        rangeCircleImage.rectTransform.sizeDelta = new Vector2(diameter * 100f, diameter * 100f);

        rangeCircleImage.gameObject.SetActive(true);
    }

    private void OnUpgradeClicked()
    {
        if (selectedTurret != null && selectedTurret.Upgrade())
        {
            UpdateUI();
            UpdateRangeCircle();
        }
    }

    private void OnSellClicked()
    {
        if (selectedTurret != null)
        {
            ClearUI();
            selectedTurret.Sell();
        }
    }
}
