using UnityEngine;

public class ScorpionStats : MonoBehaviour
{
    private float health = 80f;
    
    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
