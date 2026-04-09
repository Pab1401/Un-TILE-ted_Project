using UnityEngine;

public class BulletStats : MonoBehaviour
{
    public float damage;

    void OnTriggerEnter(Collider other)
    {
        ITakeDamage damageable = other.gameObject.GetComponent<ITakeDamage>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
