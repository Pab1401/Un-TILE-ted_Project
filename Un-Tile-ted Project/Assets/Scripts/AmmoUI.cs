using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;

    private void OnEnable()
    {
        // Subscribe to the event
        PlayerStatus.OnAmmoChanged += UpdateAmmoDisplay;
        PlayerStatus.OnPlayerReload += ToggleReloadDisplay;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks or errors when the object is destroyed
        PlayerStatus.OnAmmoChanged -= UpdateAmmoDisplay;
        PlayerStatus.OnPlayerReload -= ToggleReloadDisplay;
    }

    private void UpdateAmmoDisplay(int currentAmmo, int maxAmmo)
    {
        // This only runs when the Action is Invoked in PlayerStatus
        ammoText.text = currentAmmo.ToString() + " / " + maxAmmo.ToString();
    }

    private void ToggleReloadDisplay(bool isReloading)
    {
        ammoText.text = "Reloading...";
    }
}
