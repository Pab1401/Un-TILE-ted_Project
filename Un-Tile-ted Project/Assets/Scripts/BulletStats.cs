using UnityEngine;

public class BulletStats : MonoBehaviour
{
    public float damage;
    public GameObject shooter;
    public int bounce;
    public bool canIgnoreWalls = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == shooter.tag)
            return;
        if (other.gameObject.CompareTag("bullet"))
            return;
        if (other.gameObject.GetComponent<ICanCollide>() != null)
        {
            if (other.gameObject.tag == "wall")
            {
                Destroy(gameObject);
            }
            //Possible bounce logic
            
        }
        if (other.gameObject.GetComponent<ITakeDamage>() != null)
        {
            ITakeDamage damageable = other.gameObject.GetComponent<ITakeDamage>();
            damageable.TakeDamage(damage);
            Destroy(gameObject);
        }
        
    }
}
