using UnityEngine;

public class ShootLogic : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 500f;
    public void Shoot(Vector3 startingPosition, Vector3 targetPosition, float damage, GameObject projectilePrefab, GameObject shooter)
    {
        GameObject bullet = Instantiate(projectilePrefab, startingPosition + (targetPosition - startingPosition).normalized * 1f, Quaternion.LookRotation(targetPosition - startingPosition));
        bullet.GetComponent<BulletStats>().damage = damage;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        bullet.GetComponent<BulletStats>().shooter = shooter;
        rb.AddForce((targetPosition - startingPosition).normalized * bulletSpeed);
        rb.useGravity = false;
    }
}
