using System.Collections;
using UnityEngine;

public class PlayerStatus : MonoBehaviour, ITakeDamage, IHealDamage
{
    [SerializeField] GameManager manager;
    // Set Stats
    private float health = 100;
    private float maxHealth = 100f;

    // Modifiable stats
    public float PlayerDamage = 10f;
    public int bulletBounce = 2;
    public int maxBullets = 5;
    private int currentBullets = 0;
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
    }
    public void TakeDamage(float damage)
    {
        if (invincible)
            return;
        health -=  damage;
        Debug.Log("Player took damage, now has: " + health);
        invincible = true;
        StartCoroutine(InvincibilityFrames());
        if (health <= 0)
            manager.GameEnd();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        invincible = false;
        currentBullets = maxBullets;
    }
    IEnumerator Reload()
    {
        yield return new WaitForSeconds(4f);
        currentBullets = maxBullets;
    }

    IEnumerator InvincibilityFrames()
    {
        yield return new WaitForSeconds(1.5f);
        invincible = false;
    }
}
