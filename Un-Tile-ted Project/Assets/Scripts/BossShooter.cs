using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BossShooter : MonoBehaviour
{
    public Vector3 shootingPoint;
    public ShootLogic shootL;
    private BossStats stats;
    void Start()
    {
        stats = GetComponentInParent<BossStats>();
        shootingPoint = GetComponentInParent<Transform>().position + (Vector3.back*1.5f);
        Debug.Log(shootingPoint);
    }
    public async Task Beams()
    {
        
    }

    public async Task Shoot(GameObject bullet, int middle, Vector3 player)
    {
        int rand = Random.Range(-middle, middle);
        Debug.Log("shootin from " + (shootingPoint + (Vector3.right * rand)) + " to " + player);
        // shoot.Shoot(shootingPoint + (Vector3.right * rand), shootingPoint + (Vector3.right * rand) + Vector3.back, stats.bulletDamage, bullet, gameObject.GetComponentInParent<GameObject>());
        shootL.Shoot(shootingPoint + (Vector3.right * rand), player, stats.bulletDamage, bullet, gameObject);
        await Task.Delay(1000);
    }
}
