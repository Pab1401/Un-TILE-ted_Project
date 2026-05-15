using UnityEngine;

public class Vault : MonoBehaviour
{
    public static Vault Instance { get; private set; }

    // Values to carry over
    public float health = 100f;
    public float maxHealth = 100f;
    public float playerDamage = 10f;
    public int bulletBounce = 2;
    public int maxBullets = 5;
    public float reloadTime = 4f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Call this before loading a new level
    public void SaveStats(float currentHealth, float MaxHealth, float damage, int bounce, int bullets, float reload)
    {
        health = currentHealth;
        maxHealth = MaxHealth;
        playerDamage = damage;
        bulletBounce = bounce;
        maxBullets = bullets;
        reloadTime = reload;
    }
}
