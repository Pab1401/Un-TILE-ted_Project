using TMPro;
using UnityEngine;

public class HPUpdate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;

    private void OnEnable()
    {
        // Subscribe to the event
        PlayerStatus.OnHealthChanged += UpdateHealthDisplay;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks or errors when the object is destroyed
        PlayerStatus.OnHealthChanged -= UpdateHealthDisplay;
    }

    private void UpdateHealthDisplay(float currentHealth)
    {
        // This only runs when the Action is Invoked in PlayerStatus
        healthText.text = currentHealth.ToString();
    }
}
