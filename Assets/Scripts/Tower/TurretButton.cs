using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TurretButton : MonoBehaviour
{
    public int cost = 50; // Costo della torretta
    private Button button;
    public GameObject TurretPrefab;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    // Funzione per aggiornare l'interagibilità del pulsante
    public void UpdateButtonState(int playerCoins)
    {
        button.interactable = playerCoins >= cost;
    }
}
