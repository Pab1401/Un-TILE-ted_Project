using UnityEngine;

public class BulletStats : MonoBehaviour
{
    public float damage;
    public GameObject shooter;
    public int bounce;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == shooter)
            return;
        if (other.gameObject.CompareTag("bullet"))
            return;
        if (other.gameObject.GetComponent<ICanCollide>() != null)
        {
            //Possible bounce logic
            Destroy(gameObject);
        }
        if (other.gameObject.GetComponent<ITakeDamage>() != null)
        {
            ITakeDamage damageable = other.gameObject.GetComponent<ITakeDamage>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
            Destroy(gameObject);    
        }
        
    }
}
