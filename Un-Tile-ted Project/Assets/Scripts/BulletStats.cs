using UnityEngine;

public class BulletStats : MonoBehaviour
{
    public float damage;
    public GameObject shooter;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == shooter)
            return;
        ITakeDamage damageable = other.gameObject.GetComponent<ITakeDamage>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
