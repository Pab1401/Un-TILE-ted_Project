using System.Collections;
using UnityEngine;

public class PlayerStatus : MonoBehaviour, ITakeDamage, IHealDamage
{
    private float health = 100;
    private float maxHealth = 100f;
    private float PlayerDamage = 10f;

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
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        invincible = false;
    }

    IEnumerator InvincibilityFrames()
    {
        yield return new WaitForSeconds(1.5f);
        invincible = false;
    }
}
