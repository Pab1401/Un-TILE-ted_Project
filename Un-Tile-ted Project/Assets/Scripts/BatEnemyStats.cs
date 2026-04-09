using UnityEngine;

public class BatEnemyStats : MonoBehaviour, ITakeDamage
{
    private float health = 50f;
    public float damage = 8f;
    
    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
