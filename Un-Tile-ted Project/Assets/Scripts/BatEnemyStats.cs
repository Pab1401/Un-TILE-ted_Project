using UnityEngine;

public class BatEnemyStats : MonoBehaviour, ITakeDamage
{
    public GameManager manager;
    private float health = 50f;
    public float damage = 8f;
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Bat took damage, now has: " + health);
        if (health <= 0)
        {
            manager.EnemyCount--;
            Destroy(gameObject);
        }
    }
}
