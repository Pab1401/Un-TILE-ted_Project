using System.Collections;
using UnityEngine;

public class PlayerStatus : MonoBehaviour, ITakeDamage, IHealDamage
{
    public static event System.Action<float> OnHealthChanged;
    public static event System.Action<bool> OnPlayerReload;
    private GameManager manager;
    // Set Stats
    private float health = 100;
    public float Health
    {
        get {return health;}
    }
    private float maxHealth = 100f;
    public float MaxHealth
    {
        get {return maxHealth;}
    }

    // Modifiable stats
    public float PlayerDamage = 10f;
    public int bulletBounce = 2;
    public int maxBullets = 5;
    private int currentBullets = 0;

    public float reloadTime = 4;
    public int CurrentBullets
    {
        get { return currentBullets; }
        set 
        { 
            currentBullets = value;

            if (currentBullets == 0)
                StartCoroutine(Reload());
        }
    }

    private bool invincible;

    public void Heal(float healAmount)
    {
        health += healAmount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        OnHealthChanged?.Invoke(health);
    }
    public void TakeDamage(float damage)
    {
        if (invincible)
            return;
        health -=  damage;
        if (health <= 0)
        {
            AudioManager.Instance.PlayPlayerDeath();
            // Lógica de muerte...
        }
        else
        {
            AudioManager.Instance.PlayPlayerHurt();
        }
        Debug.Log("Player took damage, now has: " + health);
        invincible = true;
        StartCoroutine(InvincibilityFrames());
        OnHealthChanged?.Invoke(health);
        if (health <= 0)
            manager.GameEnd();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Vault.Instance != null)
        {
            health = Vault.Instance.health;
            maxHealth = Vault.Instance.maxHealth;
            PlayerDamage = Vault.Instance.playerDamage;
            bulletBounce = Vault.Instance.bulletBounce;
            maxBullets = Vault.Instance.maxBullets;
            reloadTime = Vault.Instance.reloadTime;
        }
        manager = FindFirstObjectByType<GameManager>();
        invincible = false;
        currentBullets = maxBullets;
        OnHealthChanged?.Invoke(health);
    }
    IEnumerator Reload()
    {
        OnPlayerReload?.Invoke(true);
        yield return new WaitForSeconds(reloadTime);
        currentBullets = maxBullets;
    }

    IEnumerator InvincibilityFrames()
    {
        yield return new WaitForSeconds(1.5f);
        invincible = false;
    }
}
