using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : MonoBehaviour
{
    [Header("Selection Settings")]
    public Color selectedColor = Color.yellow;
    public Color defaultColor = Color.white;

    [Header("References")]
    public TowerGridManager towerGridManager;
    public TowerInfoUI towerInfoUI;

    private GameObject selectedTower;

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(worldPos);

            if (hit != null)
            {
                GameObject tower = towerGridManager.GetTowerFromChild(hit.transform);
                if (tower != null)
                {
                    SelectTower(tower);
                    return;
                }
            }

            DeselectTower();
        }
    }

    void SelectTower(GameObject tower)
    {
        TurretController turret = tower.GetComponentInParent<TurretController>(true);

        
        if (turret != null && towerInfoUI != null)
        {
            towerInfoUI.SelectTurret(turret);
        }

        if (selectedTower == tower) return;

        DeselectTower();

        selectedTower = tower;

        foreach (var sr in selectedTower.GetComponentsInChildren<SpriteRenderer>(true))
        {
            sr.color = selectedColor;
        }
    }

    void DeselectTower()
    {
        if (selectedTower != null)
        {
            foreach (var sr in selectedTower.GetComponentsInChildren<SpriteRenderer>(true))
            {
                sr.color = defaultColor;
            }
        }

        if (towerInfoUI != null)
        {
            towerInfoUI.ClearUI();
        }

        selectedTower = null;
    }
}
