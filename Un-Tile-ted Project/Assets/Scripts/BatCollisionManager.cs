using UnityEngine;

public class BatCollisionManager : MonoBehaviour
{
    public BatEnemyStats batStats;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision Detected");
        ITakeDamage damageable = other.gameObject.GetComponent<ITakeDamage>();
        if (damageable != null)
        {
            damageable.TakeDamage(batStats.damage);
        }
    }
    void Start()
    {
        Debug.Log("Collision Manager Started");
    }
}
