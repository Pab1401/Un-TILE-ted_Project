using UnityEngine;

public class ScorpionStats : MonoBehaviour, ITakeDamage
{
    public GameManager manager;
    private float health = 80f;
    public float CollisionDamage = 15.0f;
    public float BulletDamage = 10f;
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            manager.EnemyCount--;
            Destroy(gameObject);
        }
            
    }
}
